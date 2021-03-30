using System;
using System.Collections.Generic;
// using System.IO;
using System.Linq;
using System.Text;
using DatabaseSync.Database;
// using DatabaseSync.ResourceManagement.Util;
using Newtonsoft.Json.Linq;
// using System.Threading.Tasks;

using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Reporting;
using UnityEditor.Localization.Plugins.Google.Columns;

using UnityEngine;
// using UnityEngine.Localization;
// using UnityEngine.Localization.Tables;
using static UnityEngine.Localization.Tables.SharedTableData;
using Object = UnityEngine.Object;

namespace DatabaseSync.Localization.Plugins.JSON
{
	using Binary;
	using Fields;

	public class JsonTableSync
	{
		public static void OpenTableInBrowser(string tableId)
		{

		}

		/// <summary>
		/// The sheets provider is responsible for providing the SheetsService and configuring the type of access.
		/// <seealso cref="DatabaseConfig"/>.
		/// </summary>
		public ITableService SheetsService { get; private set; }

		protected internal virtual bool UsingApiKey => (SheetsService as DatabaseConfig)?.Authentication == true;

		/// <summary>
		/// The Id of the Google Sheet. This can be found by examining the url:
		/// https://docs.google.com/spreadsheets/d/<b>>SpreadsheetId</b>/edit#gid=<b>SheetId</b>
		/// Further information can be found <see href="https://developers.google.com/sheets/api/guides/concepts#spreadsheet_id">here.</see>
		/// </summary>
		public string TableId { get; set;  }

		/// <summary>
		/// Creates a new instance of a GoogleSheets connection.
		/// </summary>
		/// <param name="provider">The Google Sheets service provider. See <see cref="DatabaseConfig"/> for a default implementation.</param>
		public JsonTableSync(ITableService provider)
		{
			SheetsService = provider ?? throw new ArgumentNullException(nameof(provider));
		}

		/// <summary>
		/// Returns all the column titles(values from the first row) for the selected sheet inside of the Spreadsheet with id <see cref="TableId"/>.
		/// This method requires the <see cref="SheetsService"/> to use OAuth authorization as it uses a data filter which reuires elevated authorization.
		/// </summary>
		/// <returns>All the </returns>
		public IList<string> GetColumnTitles()
		{
			if (string.IsNullOrEmpty(TableId)) throw new Exception($"{nameof(TableId)} is required.");

			return new List<string>{ "text" };
		}

		/// <summary>
		/// Pulls data from the Spreadsheet with id <see cref="TableId"/> and uses <paramref name="fieldMapping"/>
		/// to populate the <paramref name="collection"/>.
		/// </summary>
		/// <param name="collection">The collection to insert the data into.</param>
		/// <param name="fieldMapping">The column mappings control what data will be extracted for each column of the sheet. The list must contain 1 <see cref="IPullKeyColumn"/>.</param>
		/// <param name="removeMissingEntries">After a pull has completed any keys that exist in the <paramref name="collection"/> but did not exist in the sheet are considered missing,
		/// this may be because they have been deleted from the sheet. A value of true will remove these missing entries where false will preserve them.</param>
		/// <param name="reporter">Optional reporter to display the progress and status of the task.</param>
		/// <param name="createUndo">Should an Undo be recorded so any changes can be reverted?</param>
		public void PullIntoStringTableCollection(StringTableCollection collection,
			IList<SheetColumn> fieldMapping, bool removeMissingEntries = false, ITaskReporter reporter = null,
			bool createUndo = false)
		{
			VerifyPushPullArguments(collection, fieldMapping, typeof(IPullKeyColumn));

			try
			{
				var modifiedAssets = collection.StringTables.Select(t => t as Object).ToList();
				modifiedAssets.Add(collection.SharedData);

				if (createUndo)
				{
					Undo.RecordObjects(modifiedAssets.ToArray(),
						$"Pull `{collection.TableCollectionName}` from JSON file");
				}

				reporter?.Start($"Pull `{collection.TableCollectionName}` from JSON file", "Preparing fields");

				// The response columns will be in the same order we request them, we need the key
				// before we can process any values so ensure the first column is the key column.
				// var sortedColumns = fieldMapping.OrderByDescending(c => c is IPullKeyColumn).ToList();

				// We can only use public API. No data filters.
				// We use a data filter when possible as it allows us to remove a lot of unnecessary information,
				// such as unneeded sheets and columns, which reduces the size of the response. A Data filter can only be used with OAuth authentication.
				reporter?.ReportProgress("Generating request", 0.1f);

				// TODO we need to get the json data.

				// ClientServiceRequest<Spreadsheet> pullReq = UsingApiKey
					// ? GeneratePullRequest()
					// : GenerateFilteredPullRequest(sheetId, fieldMapping);

				reporter?.ReportProgress("Sending request", 0.2f);

				var table = TableDatabase.Get.GetTable(TableId);

				reporter?.ReportProgress("Validating response", 0.5f);

				// When using an API key we get all the sheets so we need to extract the one we are pulling from.
				if (table == null) throw new Exception($"No table data available for {TableId}");

				// The data will be structured differently if we used a filter or not so we need to extract the parts we need.
				var pulledRows = new List<(uint valueIndex, TableRow rowData)>();

				if (UsingApiKey)
				{
					// When getting the whole sheet all the columns are stored in a single Data. We need to extract the correct value index for each column.
					// foreach (var sortedCol in sortedColumns)
					// {
						// TableRow row = table.Rows[0];
						// pulledRows.Add((row.Fields.Values, sortedCol.ColumnIndex));
					// }
				}
				else
				{

					// TODO check the fieldmapping count and see if it is equal to the object children amount
					// When using a filter each Data represents a single column.
					pulledRows = table.Rows.Select(t => (t.Key, t.Value)).ToList();
				}

				MergePull(pulledRows, collection, fieldMapping, removeMissingEntries, reporter);

				// There is a bug that causes Undo to not set assets dirty (case 1240528) so we always set the asset dirty.
				modifiedAssets.ForEach(EditorUtility.SetDirty);

				// if(LocalizationEditorSettings.EditorEvents.CollectionModified != null)
				LocalizationEditorSettings.EditorEvents.RaiseCollectionMod(this, collection);
				AssetDatabase.SaveAssets();
			}
			catch (Exception e)
			{
				reporter?.Fail(e.Message);
				throw;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="fieldMapping"></param>
		/// <param name="requiredKeyType"></param>
		/// <exception cref="Exception"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
		void VerifyPushPullArguments(StringTableCollection collection, IList<SheetColumn> fieldMapping, Type requiredKeyType)
		{
			if (string.IsNullOrEmpty(TableId))
				throw new Exception($"{nameof(TableId)} is required.");

			if (collection == null)
				throw new ArgumentNullException(nameof(collection));

			if (fieldMapping == null)
				throw new ArgumentNullException(nameof(fieldMapping));

			if (fieldMapping.Count == 0)
				throw new ArgumentException("Must include at least 1 column.", nameof(fieldMapping));

			if (fieldMapping.Count(c => requiredKeyType.IsAssignableFrom(c.GetType())) != 1)
				throw new ArgumentException($"Must include 1 {requiredKeyType.Name}.", nameof(fieldMapping));

			ThrowIfDuplicateColumnIds(fieldMapping);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="fieldMapping"></param>
		/// <exception cref="Exception"></exception>
		void ThrowIfDuplicateColumnIds(IList<SheetColumn> fieldMapping)
		{
			var ids = new HashSet<string>();
			foreach (var col in fieldMapping)
			{
				if (ids.Contains(col.Column))
					throw new Exception($"Duplicate column found. The Field {col.Column} is already in use");
				ids.Add(col.Column);
			}
		}

		void MergePull(List<(uint valueIndex, TableRow rowData)> rows, StringTableCollection collection, IList<SheetColumn> fieldMapping, bool removeMissingEntries, ITaskReporter reporter)
        {
            reporter?.ReportProgress("Preparing to merge", 0.55f);

            // Keep track of any issues for a single report instead of filling the console.
            var messages = new StringBuilder();

            var keyColumn = fieldMapping[0] as IPullKeyColumn;
            Debug.Assert(keyColumn != null, "Expected the first column to be a Key column");

            var rowCount = rows.Count;

            // Send the start message
            foreach (var col in fieldMapping)
            {
                col.PullBegin(collection);
            }

            reporter?.ReportProgress("Merging response into collection", 0.6f);
            var keysProcessed = new HashSet<long>();

            // We want to keep track of the order the entries are pulled in so we can match it
            var sortedEntries = new List<SharedTableEntry>(rowCount);

            long totalCellsProcessed = 0;
            for (int row = 0; row < rowCount; row++)
            {
	            var localeId = (row + 1).ToString();
	            var rowKeyEntry = keyColumn.PullKey(localeId, localeId);
	            sortedEntries.Add(rowKeyEntry);

	            var keyRowData = rows[row];
	            var keyValue = keyRowData.rowData.Find(JsonField.ColumnHeader);

	            // Skip rows with no key data
	            if (rowKeyEntry == null)
	            {
		            messages.AppendLine($"No key data was found for row {localeId} with Value '{keyValue}' and Note '{localeId}'.");
		            continue;
	            }

	            for (int map = 1; map < fieldMapping.Count; ++map)
	            {
		            // find the mapped value
		            var mapValue = fieldMapping[map];

		            if (mapValue == null)
			            continue;

		            // Record the id so we can check what key ids were missing later.
		            keysProcessed.Add(rowKeyEntry.Id);
		            totalCellsProcessed++;

		            // TODO retrieve the right mapped field
		            if (keyValue.Data is string)
		            {
						reporter?.Fail($"Json value is not of type object in {TableId}, key: {mapValue.Column}");
						break;
		            }
		            JObject jsonValue = keyValue.Data;
		            var mapJsonValue = jsonValue[mapValue.Column];

		            string value = null;
		            string note = null;

		            // Do we have data in this column for this row?
		            if (mapJsonValue != null)
		            {
			            value = mapJsonValue.ToObject<string>();
			            note = localeId;
			            totalCellsProcessed++;
		            }

		            // We always call PullCellData as its possible that data may have existed
		            // in a previous Pull and has now been removed. We call Pull so that the column
		            // is aware it is now null and can remove any metadata it may have added in the past. (LOC-134)
		            fieldMapping[map].PullCellData(rowKeyEntry, value, note);
	            }
            }

            // Send the end message
            foreach (var field in fieldMapping)
            {
                field.PullEnd();
            }

            reporter?.ReportProgress("Removing missing entries and matching sheet row order", 0.9f);
            HandleMissingEntriesAndMatchPullOrder(keysProcessed, sortedEntries, collection, messages, removeMissingEntries);
            reporter?.Completed($"Completed merge of {rowCount} rows and {totalCellsProcessed} cells from {fieldMapping.Count} columns successfully.\n{messages}");
        }

		void HandleMissingEntriesAndMatchPullOrder(HashSet<long> entriesToKeep, List<SharedTableEntry> sortedEntries,
			StringTableCollection collection, StringBuilder removedEntriesLog, bool removeMissingEntries)
		{
			// We either remove missing entries or add them to the end.

			var stringTables = collection.StringTables;

			removedEntriesLog.AppendLine("Removed missing entries:");
			for (int i = 0; i < collection.SharedData.Entries.Count; ++i)
			{
				var entry = collection.SharedData.Entries[i];
				if (entriesToKeep.Contains(entry.Id))
				{
					continue;
				}

				if (!removeMissingEntries)
				{
					// Missing entries that we want to keep go to the bottom of the list
					sortedEntries.Add(entry);
				}
				else
				{
					removedEntriesLog.AppendLine($"\t{entry.Key}({entry.Id})");

					// Remove the entry
					collection.SharedData.RemoveKey(entry.Id);
					i--;

					// Remove from tables
					foreach (var table in stringTables)
					{
						table.Remove(entry.Id);
					}
				}
			}

			// Now replace the old list with our new one that is in the correct order.
			Debug.Assert(collection.SharedData.Entries.Count == sortedEntries.Count, "Expected sorted entries to match unsorted.");
			collection.SharedData.Entries = sortedEntries;
		}
	}
}

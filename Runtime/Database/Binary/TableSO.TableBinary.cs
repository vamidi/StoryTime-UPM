using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;

namespace StoryTime.Database.ScriptableObjects
{
	using Binary;
	// ReSharper disable once InconsistentNaming
	public partial class TableSO
	{
		public class TableBinary
		{
			/// <summary>
			/// This container contains all the data we need to get the right column
			/// </summary>
			private UJSON _ujson = new()
			{
				JsonColumnEntries = new List<JsonEntry>()
			};

			// Size followed by actual data
			private readonly List<TableField> _data = new();

			private bool _listUpdated;
			private string _shownColumn;

			private readonly Dictionary<uint, string> _list = new()
			{
				{ UInt32.MaxValue, "None" }
			};

			/*
			public static string GetIdFromFile(string tableName)
			{
				var config = TableDatabase.Fetch();
				string destination = $"{config.dataPath}/{tableName}.json";

				if (!File.Exists(destination)) throw new ArgumentException("Cant find JSON file!");

				var stream = GetStream(destination);
				var token = GetTableData(stream);

				var id = token["id"].ToObject<string>() ??
				         throw new ArgumentException("Can't make find metadata from JSON file");

				return id;
			}

			public static Table GetTable(string tableName)
			{
				var tblStruct = new TableStruct
				{
					Ujson =
					{
						JsonColumnEntries = new List<JsonEntry>()
					},
					Data = new List<TableField>()
				};

				DatabaseConfigSO config = Fetch();
				if (config == null)
				{
#if UNITY_EDITOR
					Debug.LogWarning("Config file not found. Have you created the database config file?");
#else
				throw new ArgumentException($"{tableName} couldn't be found!");
#endif
					return tblStruct.Table;
				}

				string destination = $"{config.dataPath}/{tableName}.json";

				if (!File.Exists(destination))
#if UNITY_EDITOR
					Debug.LogWarning($"{tableName} couldn't be found!");
#else
				throw new ArgumentException($"{tableName} couldn't be found!");
#endif

				var table = ParseData(GetTableData(GetStream(destination)), ref tblStruct);
				return table;
			}

			public static TableRow GetRow(string tableName, uint entityID)
			{
				DatabaseConfigSO config = TableDatabase.Fetch();

				if (config == null)
				{
#if UNITY_EDITOR
					Debug.LogWarning("Config file not found. Have you created the database config file?");
#else
				throw new ArgumentException($"{tableName} couldn't be found!");
#endif
					return result;
				}

				string destination = $"{config.dataPath}/{tableName}.json";

				if (!File.Exists(destination))
#if UNITY_EDITOR
					Debug.LogWarning($"{tableName} couldn't be found!");
#else
				throw new ArgumentException($"{tableName} couldn't be found!");
#endif

				if (!File.Exists(destination))
				{
					throw new ArgumentException($"{tableName} couldn't be found!");
				}

				string jsonString = GetStream(destination);
				JToken tableData = GetTableData(jsonString);

				// The key-value data
				JArray arr = tableData["data"].Value<JArray>();

				// Invalid ID
				if (entityID >= arr.Count)
				{
					Debug.LogWarning($"{tableName}: EntityID: {entityID} does not exist!");
					return new TableRow();
				}

				var rowProperties = arr[(int)entityID].Children<JProperty>();

				uint i = 0;
				foreach (var entity in rowProperties)
				{
					TableField field = new TableField();

					switch (entity.Value.Type)
					{
						case JTokenType.Boolean:
							field.Data = entity.Value.ToObject<bool>();
							// Debug.Log("Bool");
							break;
						case JTokenType.Integer:
							field.Data = entity.Value.ToObject<double>();
							// Debug.Log("Number");
							break;
						case JTokenType.String:
							field.Data = entity.Value.ToObject<string>();
							// Debug.Log("String");
							break;
						case JTokenType.Object:
							field.Data = entity.Value.ToObject<JObject>();
							break;
					}

					result.Fields.Add(new TableRowInfo { ColumnName = entity.Name, ColumnID = i }, field);
					i++;
				}

				return result;
			}

			public TableField GetField(string tableName, uint columnID, uint entityID)
			{
				string destination = ""; // $"{m_ConfigFile.dataPath}/{tableName}.json";
				if (!File.Exists(destination))
				{
					throw new ArgumentException($"{tableName} couldn't be found!");
				}

				string jsonString = GetStream(destination);
				JToken tableData = GetTableData(jsonString);

				// The key-value data
				JArray arr = tableData["data"].Value<JArray>();

				TableField result = new TableField();

				// Invalid ID
				if (entityID >= arr.Count)
				{
					return new TableField();
				}

				var rowProperties = arr[(int)entityID].Children<JProperty>();
				uint i = 0;
				foreach (var entity in rowProperties)
				{
					if (i != columnID)
						continue;

					switch (entity.Value.Type)
					{
						case JTokenType.Boolean:
							result.Data = entity.Value.ToObject<bool>();
							break;
						case JTokenType.Integer:
							result.Data = entity.Value.ToObject<double>();
							break;
						case JTokenType.String:
							result.Data = entity.Value.ToObject<string>();
							break;
						case JTokenType.Object:
							result.Data = entity.Value.ToObject<JObject>();
							break;
					}

					i++;
				}

				return result;
			}

			public TableField GetField(string tableName, string columnName, uint entityID)
			{
				string destination = ""; // $"{m_ConfigFile.dataPath}/{tableName}.json";
				if (!File.Exists(destination))
				{
					throw new ArgumentException($"{tableName} couldn't be found!");
				}

				string jsonString = GetStream(destination);
				JToken tableData = GetTableData(jsonString);

				// The key-value data
				JArray arr = tableData["data"].Value<JArray>();

				TableField result = new TableField();

				// Invalid ID
				if (entityID >= arr.Count)
				{
					return new TableField();
				}

				var rowProperties = arr[(int)entityID].Children<JProperty>();

				foreach (var entity in rowProperties)
				{
					if (entity.Name == columnName)
					{
						switch (entity.Value.Type)
						{
							case JTokenType.Boolean:
								result.Data = entity.Value.ToObject<bool>();
								break;
							case JTokenType.Integer:
								result.Data = entity.Value.ToObject<double>();
								break;
							case JTokenType.String:
								result.Data = entity.Value.ToObject<string>();
								break;
							case JTokenType.Object:
								result.Data = entity.Value.ToObject<JObject>();
								break;
						}

						break;
					}
				}

				return result;
			}
			*/

			public uint GetColumnID(string columnName)
			{
				string cName = columnName;

				for (uint j = 0; j < _ujson.ColumnCount; ++j)
				{
					if (String.CompareOrdinal(_ujson.JsonColumnEntries[(int)j].Name, cName) == 0)
						return j;
				}

				return uint.MaxValue;
			}

			public string GetColumnName(uint columnId)
			{
				if (columnId > _ujson.ColumnCount)
					return "";

				return _ujson.JsonColumnEntries[(int)columnId].Name;
			}

			protected internal void Refresh(string stream, ref Dictionary<UInt32, TableRow> rows)
			{
				// clear out existing data
				_data.Clear();
				var token = GetTableData(stream);
				ParseData(token, ref rows);
			}

			/// <summary>
			/// Populate a list with a link to another table or a link within the same table.
			/// </summary>
			/// <param name="columnToShow"></param>
			/// <param name="linkedColumn"></param>
			/// <param name="linkedId"></param>
			/// <param name="isJObject"></param>
			/// <param name="otherTable"></param>
			/// <returns></returns>
			public Dictionary<uint, string> PopulateWithLink(string columnToShow, string linkedColumn, uint linkedId,
				out bool isJObject, string otherTable = "")
			{
				isJObject = false;
				return new Dictionary<uint, string>();
				/*
				// Find the other binary if we need to show the string that is inside that table.
				// If not that means the linked column is in the same table.
				TableBinary tableBinary = otherTable != String.Empty ? TableDatabase.Get.GetBinary(otherTable) : this;
				// Populate the <number, string value>
				Dictionary<uint, string> newList = tableBinary.Populate(columnToShow, out isJObject);
				// Get the index of the linked column
				uint uiLinkedID = linkedColumn != "" ? tableBinary.GetColumnID(linkedColumn) : uint.MaxValue;

				// Debug.Log($"otherTable: {otherTable}");
				// Debug.Log($"linkedColumn: {linkedColumn}");
				// Debug.Log($"linkId: {linkedId}");
				// Debug.Log($"index linkedID: {uiLinkedID}");

				// Remove if not linked
				// Remove if not linked
				if (uiLinkedID != uint.MaxValue && linkedColumn != "")
				{
					List<uint> keys = new List<uint>(newList.Keys);

					foreach (uint key in keys)
					{
						TableField currentField = GetField(_tableName, uiLinkedID, key);
						if (currentField != null)
						{
							if (currentField.Data != null)
							{
								uint id = (uint)currentField.Data;
								TableField otherfield = GetField(tableBinary._tableName, columnToShow, id);

								if (otherfield != null)
								{
									string d = "";
									if (otherfield.Data is JObject)
									{
										d += $"[{key}] {otherfield.Data["en"]}";
									}
									else
										d += $"[{key}] {otherfield.Data}";

									newList[key] = d;
								}
							}
						}

						// memcpy(&d, Field.Data.Get(), Field.Size);

						// if ((uint) d != linkedId && otherTable == String.Empty) newList.Remove(key);
					}

					// NewList.Compact();
				}

				return newList;
				*/
			}

			/// <summary>
			/// Generate a list with columns from the JSON data
			/// </summary>
			/// <param name="columnToShow"></param>
			/// <param name="isJsonObj"></param>
			/// <returns></returns>
			public Dictionary<uint, string> Populate(string columnToShow, out bool isJsonObj)
			{
				isJsonObj = false;

				uint uiColumnID = GetColumnID(columnToShow);

				if (_shownColumn == columnToShow && !_listUpdated)
				{
					var idx = (int)uiColumnID;
					if (idx > 0 && idx < _data.Count)
					{
						var data = _data[idx].Data;
						isJsonObj = data is JObject;
					}

					return _list;
				}

				_listUpdated = false;
				_shownColumn = columnToShow;

				for (uint i = 0; i < _ujson.EntityCount; ++i)
				{
					// if (!_UJSON.DSCT.Pointers[i]) { _list.Remove(i); continue; }
					string d = "";
					if (uiColumnID == uint.MaxValue)
						d = i.ToString();
					else
					{
						var data = _data[(int)(i * _ujson.ColumnCount + uiColumnID)].Data;
						if (data is JObject)
						{
							isJsonObj = true;
							d += $"[{i}] {data["en"]}";
						}
						else
							d += $"[{i}] {data}";
					}

					// override if already exists
					if (_list.ContainsKey(i))
						_list[i] = d;
					else
						_list.Add(i, d);
				}

				return _list;
			}

			/// <summary>
			/// Import the JSON data and read from it.
			/// </summary>
			/// <param name="tableID"></param>
			/// <param name="jsonTableData"></param>
			/// <param name="exportData"></param>
			protected internal void Import(string tableID, JToken jsonTableData, ref Dictionary<UInt32, TableRow> rows, out string exportData)
			{
				exportData = "";
				_listUpdated = true;

				if (tableID == "" || jsonTableData["data"] == null || jsonTableData["metadata"] == null)
					throw new ArgumentException("JSON does not contains the property id, data or metadata");

				// if (m_ConfigFile == null)
					// throw new ArgumentException("Database Config file could not be found");

				// The key-value data
				JArray entries = jsonTableData["data"].Value<JArray>();
				// The metadata from the json stream
				JObject metaData = jsonTableData["metadata"].Value<JObject>();

				// set the count of the amount of entities in the json
				_ujson.EntityCount = (uint)entries.Count;

				// {"Sellable":true,"created_at":1594979553,"deleted":false,"effectPrimaryValue":0,"effectTypeId":0,"name":"","sellValue":0,"updated_at":1594979553}

				// Check change in column
				// _UJSON.Entries = new List<JSONEntry>((int)_UJSON.EntityCount);

				uint i = 0;
				foreach (var row in entries)
				{
					JObject entity = row.ToObject<JObject>();

					// highestID = (uint)Mathf.Max(highestID, uint.Parse(row.Key))
					// UInt32 j = UInt32.Parse(r.Key) * _UJSON.ColumnCount;
					TableRow tblRow = new TableRow
					{
						RowId = i
					};

					uint j = 0;
					foreach (var column in entity)
					{
						TableField field = new TableField();
						switch (column.Value.Type)
						{
							case JTokenType.Boolean:
								field.Data = column.Value.ToObject<bool>();
								// Debug.Log("Bool");
								break;
							case JTokenType.Integer:
								field.Data = column.Value.ToObject<double>();
								// Debug.Log("Number");
								break;
							case JTokenType.String:
								field.Data = column.Value.ToObject<string>();
								// Debug.Log("String");
								break;
							case JTokenType.Object:
								field.Data = column.Value.ToObject<JObject>();
								break;
						}

						if (i == 0)
						{
							// Set the amount of columns existing in all the entities
							_ujson.ColumnCount = (uint)entity.Count;

							// We need to story all the data in one large array
							// _ujson.JsonColumnEntries.Capacity = (int)(_ujson.ColumnCount * _ujson.EntityCount);
						}

						// From there we need store the entries
						JsonEntry entry = new JsonEntry { Name = column.Key, Type = column.Value.Type };
						_ujson.JsonColumnEntries.Add(entry);
						_data.Add(field);

						// Add the field to the table row as well.
						tblRow.Fields.Add(new TableRowInfo { ColumnName = column.Key, ColumnID = j }, field);
						j++;

					}

					if (rows.ContainsKey(i))
						rows[i] = tblRow;
					else
						rows.Add(i, tblRow);
					i++;
				}

				// Set data
				// _ujson.EntityCount = (uint)Mathf.Max(
					// _ujson.EntityCount,
					// Mathf.Max((uint)entries.Count, highestID + 1)
				// );

				exportData = JObject.FromObject(new
				{
					id = tableID,
					metadata = metaData,
					data = entries,
				}).ToString();
			}

			/// <summary>
			/// Get the stream from the json file
			/// </summary>
			/// <param name="destination"></param>
			/// <returns></returns>
			private static string GetStream(string destination)
			{
				StreamReader reader = new StreamReader(destination);
				string jsonString = reader.ReadToEnd();
				reader.Close();

				return jsonString;
			}

			/// <summary>
			/// Parse the JSON string to a JSON token that can be read from.
			/// </summary>
			/// <param name="jsonString"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentException"></exception>
			private static JToken GetTableData(string jsonString)
			{
				JToken tableData = JToken.Parse(jsonString);
				if (tableData["id"] == null || tableData["data"] == null || tableData["metadata"] == null)
					throw new ArgumentException("JSON does not contains the property id, data or metadata");

				return tableData;
			}

			/// <summary>
			/// The table data is read from the JSON token and then inserted
			/// in the table struct.
			/// </summary>
			/// <param name="tableData"></param>
			/// <param ref="TableRow" name="rows"></param>
			/// <returns></returns>
			private void ParseData(JToken tableData, ref Dictionary<UInt32, TableRow> rows)
			{
				JArray entities = tableData["data"].Value<JArray>();

				// set the count of the amount of entities in the json
				_ujson.EntityCount = (uint)entities.Count;

				uint i = 0;

				// add ID if we dont have one
				// Debug.Log(rowProperties["id"]);
				// Debug.Log(rowProperties["id"] == null);
				// if (rowProperties["id"] == null)
				// {
				// result.Fields.Add(new TableRowInfo{ ColumnName = "id", ColumnID = i }, new TableField
				// {
				// Data = entityID
				// });
				// i++;
				// }

				// Loop through all the entities
				foreach (var el in entities)
				{
					var rowParameters = el.Children<JProperty>();
					TableRow tblRow = new TableRow
					{
						RowId = i
					};

					if (i == 0)
					{
						// Set the amount of columns existing in all the entities
						_ujson.ColumnCount = (uint)rowParameters.Count();

						// We need to story all the data in one large array
						// _ujson.JsonColumnEntries.Capacity = (int)(_ujson.ColumnCount * _ujson.EntityCount);
					}

					uint j = 0;
					foreach (var entity in rowParameters)
					{
						TableField field = new TableField();

						switch (entity.Value.Type)
						{
							case JTokenType.Boolean:
								field.Data = entity.ToObject<bool>();
								// Debug.Log("Bool");
								break;
							case JTokenType.Integer:
								field.Data = entity.Value.ToObject<double>();
								// Debug.Log("Number");
								break;
							case JTokenType.String:
								field.Data = entity.Value.ToObject<string>();
								// Debug.Log("String");
								break;
							case JTokenType.Object:
								field.Data = entity.Value.ToObject<JObject>();
								// Debug.Log(field.Data.ToString());
								break;
						}

						// From there we need store the entries
						JsonEntry entry = new JsonEntry { Name = entity.Name, Type = entity.Value.Type };
						_ujson.JsonColumnEntries.Add(entry);
						_data.Add(field);

						// Add the field to the table row as well.
						tblRow.Fields.Add(new TableRowInfo { ColumnName = entity.Name, ColumnID = j }, field);
						j++;
					}

					if (rows.ContainsKey(i))
						rows[i] = tblRow;
					else
						rows.Add(i, tblRow);
					i++;
				}
			}
		}
	}
}

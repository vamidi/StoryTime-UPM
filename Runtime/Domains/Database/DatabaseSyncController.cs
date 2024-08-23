using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StoryTime.Domains.Database
{
    using IO;
    using Utils;
    using Resource;
    using Services.Http;

    using StoryTime.Domains.Extensions.JSON;
    using StoryTime.Domains.Utilities.Threading;
    using StoryTime.Domains.Database.ScriptableObjects;

    public class DatabaseSyncController
    {
        private struct ProjectResponse
        {
            public string id;
            public string name;
            public string roles;

            public override string ToString()
            {
                return $"Project: {name} with ID: {id}";
            }
        }

        private struct TableResponse
        {
            public string uid;
            public string name;
            public string roles;
        }
        
        internal const string GroupName = "StoryTime-Assets-Shared";

        // TODO see if this is right location
        internal const string SettingsLocationPath = "Assets/Settings/StoryTime";
        
        private readonly IAPIService _apiService;
        private readonly TableService _tableService;
        private readonly TimeService _timeService;

        public DatabaseSyncController(IAPIService apiService, TableService tableService, TimeService timeService)
        {
            _apiService = apiService;
            _tableService = tableService;
            _timeService = timeService;
        }
        
        public async Task RequestAllProjects()
        {
            var destination = $"{SettingsLocationPath}/uptime_projects.txt";
            var lastTimeStampLoadedProjects = await _timeService.GetUptime(destination);
            
            var request = _apiService.RetrieveProjects(lastTimeStampLoadedProjects);
            
            while (!request.isDone) {
                await Task.Yield();
            }
            
            var responses = new List<ProjectResponse>();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
#if UNITY_EDITOR
                    Debug.LogWarningFormat("Connection error: {0}", request.error);
                    return;
#else
					throw new ArgumentException( $"Connection error: {request.error}");
#endif
                // case UnityWebRequest.Result.ProtocolError:
                // Debug.Log("Protocol error");
                // throw new ArgumentException( $"Protocol error: {request.error}");
            }
            
            switch (request.responseCode)
            {
                case 403:
#if UNITY_EDITOR
                    Debug.LogErrorFormat("Connection error: {0}", request.error);
                    return;
#else
					throw new ArgumentException( $"Connection error: {request.error}");
#endif
            }

            string str = request.downloadHandler.text;
            var response = JObject.Parse(str);
            
            if (response.ContainsKey("errors"))
            {
                return;
            }

            if (!response.TryGet("docs", out JArray data))
            {
                Debug.Log("No new project data found");
                return;
            }

            foreach (var token in data)
            {
                var resObj = token.Value<JObject>();

                resObj.TryGet("id", out string projectId);
                resObj.TryGet("id", out string projectName);
                
                var res = new ProjectResponse
                {
                    id = projectId,
                    name = projectName
                };
                responses.Add(res);

                // Also add it to the config file
                _apiService.AddProject(res.id, res.name);
            }

            // create file with current timestamp of retrieving latest projects
            // Update timestamp
            await _timeService.WriteUptime(destination);
        }

        /// <summary>
        /// Request all tables from the server
        /// </summary>
        public async Task RequestAllTables(bool save = false)
        {
            string destination = $"{SettingsLocationPath}/uptime_tables.txt";
            var lastTimeStampLoadedProjects = await _timeService.GetUptime(destination);
            
            var request = _apiService.RequestAllTables(lastTimeStampLoadedProjects);
            
            while (!request.isDone)
            {
                await Task.Yield();
            }

            var responses = new List<ProjectResponse>();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
#if UNITY_EDITOR
                    Debug.LogWarningFormat("Connection error: {0}", request.error);
                    return;
#else
            					throw new ArgumentException( $"Connection error: {request.error}");
#endif
            }
            
            // var tasks = new List<Task>();
            string tablesStr = request.downloadHandler.text;
            var response = JObject.Parse(tablesStr);
            
            if(response.ContainsKey("errors"))
            {
                Debug.LogWarning("error while fetching tables");
                return;
            }

            if(!response.TryGet("docs", out JArray data))
            {
                Debug.Log("No new table data found");
                return;
            }

            Task[] tasks = new Task[data.Count];

            int idx = 0;
            foreach (var element in data)
            {
                // Debug.Log(title);

                // Download all tables.
                /*
                var tableMetadata = new TableMetaData
                {
                    title = title
                };
                */
                // TableDatabase.Get.AddTable($"{GetDataPath()}/{tableMetadata.title}.asset", tableMetadata);
                // ExportTableToAddressable(tableMetadata);
                
                if(element["id"] == null)
                    continue;
                
                tasks[idx] = RequestTable(element["id"].ToString());
                idx++;
            }

            // TODO create async on main thread
            await Task.WhenAll(tasks);
            await _timeService.WriteUptime(destination, TableSO.GroupName);
        }

        /// <summary>
        /// Grab the table from the server
        /// </summary>
        /// <param name="tableID"></param>
        /// <param name="save"></param>
        public async void RequestSingleTable(string tableID, bool save = false)
        {
            // Request table directly
            await RequestTable(tableID, save);
            RunOnMainThread.Schedule(() =>
            {
                // Save all the created assets
                ResourceHelper.RefreshAssets();

                // TODO see if unity has notify system
                Debug.Log("Fetching complete");
            }, 0);
            
            string destination = $"{SettingsLocationPath}/uptime_tables.txt";
            var lastTimeStampLoadedProjects = await _timeService.GetUptime(destination);
            
            var request = _apiService.RequestTable(tableID, lastTimeStampLoadedProjects);
            
            while (!request.isDone)
            {
                await Task.Yield();
            }

            var responses = new List<ProjectResponse>();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
#if UNITY_EDITOR
                    Debug.LogWarningFormat("Connection error: {0}", request.error);
                    return;
#else
            					throw new ArgumentException( $"Connection error: {request.error}");
#endif
                // case UnityWebRequest.Result.ProtocolError:
                // Debug.Log("Protocol error");
                // throw new ArgumentException( $"Protocol error: {request.error}");
            }

            string str = request.downloadHandler.text;
            var response = JObject.Parse(str);
            
            if (response.ContainsKey("errors"))
            {
                return;
            }

            JArray data = response["docs"].Value<JArray>();

            foreach (var token in data)
            {
                var resObj = token.Value<JObject>();
                var res = new ProjectResponse
                {
                    id = resObj["id"].ToString(),
                    name = resObj["id"].ToString()
                };
                responses.Add(res);

                // Also add it to the config file
                _apiService.AddProject(res.id, res.name);
            }

            // create file with current timestamp of retrieving latest projects
            // Update timestamp
            await _timeService.WriteUptime(destination, GroupName);

            // TODO see if unity has notify system
            Debug.Log("Fetching Projects completed");
        }
        
        /// <summary>
        /// Add table to
        /// </summary>
        /// <param name="tableID"></param>
        /// <param name="save"></param>
        /// <exception cref="ArgumentException"></exception>
        private async Task<TableSO> RequestTable(string tableID, bool save = false)
        {
            var request = _apiService.RequestTable(tableID);
            
            while (!request.isDone) {
                await Task.Yield();
            }

            // var tasks = new List<Task>();
            string tablesStr = request.downloadHandler.text;
            var response = JObject.Parse(tablesStr);

            bool success = response["success"]?.ToObject<bool>() ?? false;

            if (!success || response["data"] == null)
            {
                Debug.LogWarningFormat("Could not fetch data from table: {0}", tableID);
                return null;
            }

            var item = response["data"].ToObject<JObject>();

            if (item["metadata"] == null)
                throw new ArgumentException("Can't make Table from JSON file");

            TableMetaData tableMetadata = item["metadata"].ToObject<TableMetaData>();
            tableMetadata.id = tableID;
            // Export to addressable is handle in the Table itself --> SOLID PATTERN

            // Debug.LogFormat("Fetching table {0}, {1}", tableID, tableMetadata.title);

            // First fetch the table async
            string destination = $"{GetDataPath()}/Data/{tableMetadata.title}.asset";

            TableService.TableResult tableResult = _tableService.CreateTable(destination, item, tableMetadata);
            if (tableResult.Table != null && tableResult.Item != null)
            {
                TableSO table = tableResult.Table;

                table.Import(tableID, tableResult.Item);
                return table;
            }

            return null;
        }
        
        private string GetDataPath()
        {
            string dir = $"{SettingsLocationPath}/Data";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }
    }
}
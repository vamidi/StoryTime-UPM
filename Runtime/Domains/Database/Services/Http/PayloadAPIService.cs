using UnityEngine;
using UnityEngine.Networking;

namespace StoryTime.Domains.Database.Services.Http
{
    using Resource;
    using Settings.ScriptableObjects;

    public class PayloadAPIService : IAPIService
    {
        private readonly StoryTimeSettingsSO _apiConfig = ResourceHelper.GetAsset<StoryTimeSettingsSO>($"{Application.dataPath}/Settings/StoryTime/StoryTimeSettings.asset", true);
        
        public UnityWebRequest RequestAllTables(long lastTimeStamp)
        {
            var request = UnityWebRequest.Get($"{_apiConfig.ApiUrl}/projects/{_apiConfig.ProjectID}/tables?time={lastTimeStamp}");
            SetAuthorization(request);
            request.SendWebRequest();
            
            return request;
        }

        public UnityWebRequest RequestTable(string tableID, long lastTimeStamp = 0)
        {
            var request = UnityWebRequest.Get($"{_apiConfig.ApiUrl}/projects/{_apiConfig.ProjectID}/tables/{tableID}?time={lastTimeStamp}");
            SetAuthorization(request);
            request.SendWebRequest();

            return request;
        }

        public UnityWebRequest RetrieveProjects(long lastTimeStampLoadedProjects)
        {
            var request =  UnityWebRequest.Get($"{_apiConfig.ApiUrl}/projects?time={lastTimeStampLoadedProjects}");
            request.timeout = 1000;
            SetAuthorization(request);
            request.SendWebRequest();

            return request;
        }

        public void AddProject(string uid, string name)
        {
            _apiConfig.AddProject(uid, name);
        }

        private void SetAuthorization(UnityWebRequest request)
        {
            request.SetRequestHeader("Authorization", _apiConfig.ApiKey);
        }
    }
}

using UnityEngine.Networking;

namespace StoryTime.Domains.Database.Service.Http
{
    public interface IAPIService
    {
        public UnityWebRequest RetrieveProjects(long lastTimeStampLoadedProjects);
        public UnityWebRequest RequestAllTables(long lastTimeStampLoadedProjects);
        public UnityWebRequest RequestTable(string tableID, long lastTimeStampLoadedProjects = 0);
        public void AddProject(string uid, string name);
    }
}
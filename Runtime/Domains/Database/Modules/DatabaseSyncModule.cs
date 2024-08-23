using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace StoryTime.Domains.Database.Modules
{
    /// <summary>
    /// The DatabaseSync Module is in charge of making the connection to
    /// the database. Once that data is sync it will create or find the existing
    /// table scriptable object and update the data.
    /// </summary>
    public class DatabaseSyncModule
    {
        [Inject] private DatabaseSyncController DatabaseSyncController { get; set; }

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static DatabaseSyncModule Get => new();

        private DatabaseSyncModule()
        {
            StaticContext.Container.Inject(this);
            Initialize();
        }
        
        private async void Initialize()
        {
            // retrieve the projects data
            // Create a new progress indicator
            int progressId = Progress.Start("StoryTime: DatabaseSync Module");
            
            // Retrieve the projects of the user
            Progress.Report(progressId, 0.0f / 100.0f, "Retrieving projects");
            await DatabaseSyncController.RequestAllProjects();
            
            Progress.Report(progressId, 25 / 100.0f, "Retrieving tables");
            // await DatabaseSyncController.RequestAllTables();

            // The task is finished. Remove the associated progress indicator.
            Progress.Report(progressId, 100.0f, "Done Syncing Database");
            
            Progress.Remove(progressId);
        }
    }
}
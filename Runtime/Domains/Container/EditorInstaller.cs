using StoryTime.Domains.IO;
using UnityEditor;

using Zenject;

namespace StoryTime.Domains.Container
{
    using Database;
    using Database.Module;
    using Database.Service.Http;
    using Addressables.Providers;
    
    [InitializeOnLoad]
    public class EditorInstaller : EditorStaticInstaller<EditorInstaller>
    {
        private DatabaseSyncModule Module;
        
        static EditorInstaller()
        {
            Install();
        }
    
        public override void InstallBindings()
        {
            Container.Bind<IAPIService>().To<PayloadAPIService>().AsTransient();
            Container.Bind<DatabaseSyncController>().AsSingle().Lazy();
            Container.Bind<AddressableSettingsProvider>().AsSingle().Lazy();
            Container.Bind<TimeService>().AsTransient().Lazy();
            Container.Bind<TableService>().AsTransient().Lazy();

            Module = DatabaseSyncModule.Get;
        }
    }
}
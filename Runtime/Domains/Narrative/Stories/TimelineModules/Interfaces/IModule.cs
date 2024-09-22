namespace StoryTime.Domains.Narrative.Stories.TimelineModules.Interfaces
{
    public enum ModuleCategory
    {
        Main,
        Flow,
        Logic
    }
    
    public interface IModule
    {
        /// <summary>
        /// This represents the ID of the module.
        /// </summary>
        public string ID { get; }
        
        /// <summary>
        /// This represents the key with which we can identify this module.
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        /// This represents the category of the module.
        /// </summary>
        public ModuleCategory Category { get; }
    }
}
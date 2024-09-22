using System;

using UnityEngine;

namespace StoryTime.Domains.Narrative.Stories.TimelineModules
{
    using Interfaces;

    public abstract class Module : ScriptableObject, IModule
    {
        public string ID { get; } = Guid.NewGuid().ToString();
        public abstract string Key { get; }
        
        public abstract ModuleCategory Category { get; }
        
        public IReadOnlyStory Story { get; internal set; }
    }
}
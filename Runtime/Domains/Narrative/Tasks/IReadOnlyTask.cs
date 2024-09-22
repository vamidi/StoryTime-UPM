using System.Collections.Generic;

namespace StoryTime.Domains.Narrative.Tasks
{
    using StoryTime.Domains.Game.Characters.ScriptableObjects;
    using StoryTime.Domains.Game.NPC.Enemies.ScriptableObjects;
    using StoryTime.Domains.ItemManagement.Inventory;
    using StoryTime.Domains.Narrative.Stories;
    using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
    using StoryTime.Domains.Narrative.Tasks.ScriptableObjects;
    using StoryTime.Domains.Narrative.Tasks.ScriptableObjects.Events;

    public interface IReadOnlyTask
    {
        public string NextId { get; }
        public string Title { get; }
        public string Description { get; }
        public int Distance { get; }
        public bool Hidden { get; }
        public string Npc { get; }
        public EnemySO.EnemyCategory EnemyCategory { get; }
        public IReadOnlyStory Parent { get; }
        public uint RequiredCount { get; }
        public List<ItemStack> Items { get; }
        public TaskCompletionType Type { get; }
        public IReadOnlyStory StoryBeforeTask { get; }
        public IReadOnlyStory WinStory { get; }
        public IReadOnlyStory LoseStory { get; }
        public bool IsDone { get; }
        public CharacterSO Character { get; }
        public TaskEventChannelSO StartTaskEvent { get; }
        public TaskEventChannelSO EndTaskEvent { get; }
    }
}
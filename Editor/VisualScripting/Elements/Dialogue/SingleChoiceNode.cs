using System;
using UnityEngine;

using StoryTime.VisualScripting.Data.ScriptableObjects;

namespace StoryTime.Editor.VisualScripting.Elements
{
	using Utilities;

	public class SingleChoiceNode : DialogueNode
    {
	    public SingleChoiceNode(Node node) : base(node)
	    {
	    }

        public override void Init(DialogueGraphView graphView, Rect position, Type type)
        {
            if (Characters.Count <= 0 || Emotions.Count <= 0)
            {
                Debug.LogError("Please be sure that you created at least 1 Character");
                return;
            }

            base.Init(graphView, position, type /* NodeTypes.SingleChoice */);

            DialogType = type;
            // DialogType = NodeTypes.SingleChoice;
            // Choices.Add("Next Dialogue");
            Choices.Add(new ());
        }

        public override void Draw()
        {
            if (Characters.Count <= 0 || Emotions.Count <= 0)
            {
                Debug.LogError("Please be sure that you created at least 1 Character");
                return;
            }

            base.Draw();
            foreach (var choice in Choices)
            {
	            string choiceTitle = choice != null && !choice.Sentence.IsEmpty ? choice.Sentence.GetLocalizedString() : "";
	            var portChoice = this.CreatePort(choiceTitle);

                outputContainer.Add(portChoice);
            }

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}

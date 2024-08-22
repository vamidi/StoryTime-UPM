using System.Collections.Generic;

using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using StoryTime.Components;
using Node = StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node;

namespace StoryTime.Editor.VisualScripting.Elements
{
	using Utilities;

	public class DialogueNode : NodeView
	{
		public Port input;
		public readonly List<Port> outputs = new ();

		public DialogueNode(BaseGraphView graphView, Node node) : base(graphView, node) { }

		public override void Draw()
        {
	        base.Draw();

	        // Input
	        input = this.CreatePort("Dialogue Connection", direction: Direction.Input,
                capacity: Port.Capacity.Multi);

            inputContainer.Add(input);

            // Show next dialogue connection

            var portChoice = this.CreatePort("Next Dialogue");
            output = portChoice;
            outputContainer.Add(portChoice);

            // Show choices

            if (node is StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues.DialogueNode dialogueNode)
            {
	            var addChoiceButton = ElementsUtilities.CreateButton("Add Choice", () =>
	            {
		            Undo.RecordObject(dialogueNode, "Dialogue Graphview (Create Node Choice)");
		            var outputPortCount = outputContainer.Query("connector").ToList().Count;
		            var choice = new DialogueChoice();
					CreateChoicePort($"New Choice {outputPortCount}", choice);
		            dialogueNode.Choices.Add(choice); // $"New Choice {outputPortCount}"
	            });
	            addChoiceButton.AddToClassList("prata-node_button");
	            mainContainer.Insert(1, addChoiceButton);

	            foreach (var choice in dialogueNode.Choices)
	            {
		            string choiceTitle = choice != null && !choice.Sentence.IsEmpty
			            ? choice.Sentence.GetLocalizedString()
			            : "";
		            CreateChoicePort(choiceTitle, choice);
	            }
            }

            // keep track of undo records
            Undo.undoRedoPerformed += OnUndoRedo;
            RefreshExpandedState();
            RefreshPorts();
        }

		private void OnUndoRedo()
		{
			if (node is StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues.DialogueNode dialogueNode)
			{
				foreach (var choice in dialogueNode.Choices)
				{
					string choiceTitle = choice != null && !choice.Sentence.IsEmpty
						? choice.Sentence.GetLocalizedString()
						: "";
					CreateChoicePort(choiceTitle, choice);
				}
			}
		}

		private void CreateChoicePort(string nodeTitle, DialogueChoice choice)
		{
			var portChoice = this.CreatePort(nodeTitle);

			var deleteChoiceButton = ElementsUtilities.CreateButton("X", () =>
			{
				RemoveFromChoices(choice);
				RemovePort(portChoice);
			});
			deleteChoiceButton.AddToClassList("prata-node_button");

			portChoice.Add(deleteChoiceButton);
			outputContainer.Add(portChoice);
			outputs.Add(portChoice);
		}

		private void RemoveFromChoices(DialogueChoice choice)
		{
			if (node is StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues.DialogueNode dialogueNode)
			{
				Undo.RecordObject(dialogueNode, "Dialogue Graphview (Remove Node Choice)");
				dialogueNode.Choices.Remove(choice);
			}
		}
	}
}

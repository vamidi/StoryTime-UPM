using System;

using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using UnityEngine;

using Node = StoryTime.VisualScripting.Data.ScriptableObjects.Node;

namespace StoryTime.Editor.VisualScripting.Elements
{
	using Utilities;

	public class MultipleChoiceNode : DialogueNode
    {
	    public MultipleChoiceNode(Node node) : base(node)
	    {
	    }

        public override void Init(DialogueGraphView graphView, Rect position, Type type)
        {
            if (Characters.Count <= 0 || Emotions.Count <= 0)
            {
                Debug.LogError("Please be sure that you created at least 1 Character");
                return;
            }

            base.Init(graphView, position, type);
            DialogType = type;
            // DialogType = NodeTypes.MultipleChoice;
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

            var addChoiceButton = ElementsUtilities.CreateButton("Add Choice", () =>
            {
	            var outputPortCount = outputContainer.Query("connector").ToList().Count;
	            CreateChoicePort($"New Choice {outputPortCount}");
	            // Choices.Add($"New Choice {outputPortCount}");
            });

            addChoiceButton.AddToClassList("prata-node_button");

            mainContainer.Insert(1, addChoiceButton);

            foreach (var choice in Choices)
            {
	            string choiceTitle = choice != null && !choice.Sentence.IsEmpty ? choice.Sentence.GetLocalizedString() : "";
	            CreateChoicePort(choiceTitle);
            }

            RefreshExpandedState();
            RefreshPorts();
        }

        private void CreateChoicePort(string choice)
        {

            var portChoice = this.CreatePort(choice);

            var deleteChoiceButton = ElementsUtilities.CreateButton("X", () => RemovePort(portChoice));

            deleteChoiceButton.AddToClassList("prata-node_button");

            // var so = new InspectorElement(new DialogueChoice());
			// portChoice.Add(so);

            var choiceTextField = ElementsUtilities.CreateTextField(choice, (evt) =>
            {
	            var prev = evt.previousValue;
	            // var index = Choices.FindIndex(c => c == prev);
	            // Choices[index] = evt.newValue;
	            // portChoice.portName = evt.newValue.ID.ToString();
            });
            // choiceTextField.objectType = typeof(DialogueChoice);



            choiceTextField.AddClasses(
                "prata-node_textfield",
                "prata-node_choice-textfield",
                "prata-node_textfield_hidden"
            );

            portChoice.Add(choiceTextField);
            portChoice.Add(deleteChoiceButton);

            outputContainer.Add(portChoice);
        }

        private void RemovePort(Port port)
        {
            _graphView.RemovePort(this, port);
        }
    }
}

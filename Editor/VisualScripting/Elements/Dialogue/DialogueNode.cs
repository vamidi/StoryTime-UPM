using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using StoryTime.Components;
using StoryTime.VisualScripting.Data;
using StoryTime.Components.ScriptableObjects;
using Node = StoryTime.VisualScripting.Data.ScriptableObjects.Node;

namespace StoryTime.Editor.VisualScripting.Elements
{
	using Data;
	using Utilities;

	public class DialogueNode : NodeView
	{
		/// <summary>
		/// Unique
		/// </summary>
		public string GUID { get; set; } = Guid.NewGuid().ToString();

		public List<DialogueChoice> Choices { get; private set; }

		public Type DialogType { get; set; }

		public Content Content { get; set; }

		protected DialogueGraphView _graphView;

		/// <summary>
		/// All characters in the project.
		/// </summary>
		protected List<CharacterSO> Characters = ResourceManagement.HelperClass.FindAssetsByType<CharacterSO>().ToList();

		/// <summary>
		/// All the available emotions in the project.
		/// </summary>
		protected List<Emotion> Emotions => Utils.HelperClass.GetEnumValues<Emotion>().ToList();

		public DialogueNode(Node node) : base(node)
		{
		}

		public override void Init(DialogueGraphView graphView, DialogueNodeData nodeData, Type type)
        {
            _graphView = graphView;
            Choices = nodeData.Choices;
            Content = nodeData.Content;
            GUID = nodeData.Guid;
            DialogType = type;

            SetPosition(new Rect(nodeData.Position, Vector2.zero));

            mainContainer.AddToClassList("prata-node_main-container");
            extensionContainer.AddToClassList("prata-node_extension-container");
        }

        public override void Init(DialogueGraphView graphView, Rect position, Type type)
        {
            _graphView = graphView;
            Choices = new ();
            Content = new DialogContent
            {
                emotion = Emotions[0],
                characterID = Characters[0].ID
            };
            DialogType = type;

            SetPosition(position);

            mainContainer.AddToClassList("prata-node_main-container");
            extensionContainer.AddToClassList("prata-node_extension-container");
        }

        public override void Draw()
        {
	        title = Content.dialogueText;

            // create the character who is talking
            var characterSelector = ElementsUtilities.CreateDropDownMenu("Characters");

            characterSelector.RegisterValueChangedCallback(evt =>
            {
                var index = Characters.FindIndex(character => character.CharacterName.GetLocalizedString() == evt.newValue);
                Content.characterID = Characters[index].ID;
            });

            characterSelector.AppendCharacterAction(Characters, Content.characterID,
                action => { characterSelector.text = ((CharacterSO)action.userData).CharacterName.GetLocalizedString(); });
            mainContainer.Insert(1, characterSelector);

            var emotionSelector = ElementsUtilities.CreateDropDownMenu("Emotions");

            emotionSelector.RegisterValueChangedCallback((evt) =>
            {
                var index = Emotions.FindIndex(emotion => emotion.ToString().Equals(evt.newValue));
                Content.emotion = Emotions[index];
            });

            emotionSelector.AppendEmotionsAction(Emotions, Content.emotion,
                action => { emotionSelector.text = ((Emotion)action.userData).ToString(); });
            mainContainer.Insert(2, emotionSelector);

            // Input

            var inputPort = this.CreatePort("Dialogue Connection", direction: Direction.Input,
                capacity: Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            // Foldout
            var customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("prata-node_custom-data-container");

            var textFoldout = ElementsUtilities.CreateFoldout("Dialogue text");

            var textTextField = ElementsUtilities.CreateTextField(Content.dialogueText, (evt) =>
            {
                Content.dialogueText = evt.newValue;
                title = evt.newValue;
            });

            textTextField.AddClasses("prata-node_textfield", "prata-node_quote-textfield");

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        public void RemoveFromChoices(string choice)
        {
	        Choices.RemoveAll((c) => c.ID == UInt32.Parse(choice));
        }
	}
}

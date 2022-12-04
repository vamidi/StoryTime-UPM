using System;
using System.Collections.Generic;

using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

using UnityEngine.UIElements;
using TextElement = UnityEngine.UIElements.TextElement;

using StoryTime.Components;
using StoryTime.Components.ScriptableObjects;

namespace StoryTime.Editor.VisualScripting.Utilities
{
	using Elements;
	public static class ElementsUtilities
    {
	    public static Button CreateButton(string title, Action onClick = null)
        {
            var button = new Button(onClick)
            {
                text = title
            };
            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            var foldout = new Foldout
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Port CreatePort(
            this NodeView node,
            string portName = "",
            Orientation orientation = Orientation.Horizontal,
            Direction direction = Direction.Output,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            var port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;

            return port;
        }

        public static TextElement CreateTextElement(string value = null)
        {
            var textElement = new TextElement
            {
                text = value
            };

            return textElement;
        }

        public static TextField CreateTextField(
            string value = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null
        )
        {
            var textTextField = new TextField
            {
                value = value,
            };

            if (onValueChanged != null) textTextField.RegisterValueChangedCallback(onValueChanged);

            return textTextField;
        }

        public static TextField CreateTextField(
            string value = null,
            string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null
        )
        {
            var textTextField = new TextField
            {
                label = label,
                value = value,
            };

            if (onValueChanged != null) textTextField.RegisterValueChangedCallback(onValueChanged);

            return textTextField;
        }

        public static TextField CreateTextArea(
            string value = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null
        )
        {
            var textArea = CreateTextField(value, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }

        public static ToolbarMenu CreateDropDownMenu(string title = "")
        {
            return new ToolbarMenu { text = title };
        }

        public static void AppendCharacterAction(
            this ToolbarMenu toolbarMenu,
            List<CharacterSO> characters,
            UInt32 savedCharacterId = UInt32.MaxValue,
            Action<DropdownMenuAction> action = null
        )
        {
            if (savedCharacterId == UInt32.MaxValue)
            {
                toolbarMenu.text = characters[0].CharacterName.GetLocalizedString();
            }
            else
            {
                var savedCharacter = characters.Find(c => c.ID == savedCharacterId);
                toolbarMenu.text = savedCharacter.CharacterName.GetLocalizedString();
            }

            foreach (var character in characters)
            {
                toolbarMenu.menu.AppendAction(
                    actionName: character.CharacterName.GetLocalizedString(),
                    action: action,
                    actionStatusCallback: a => DropdownMenuAction.Status.Normal,
                    userData: character);
            }
        }

        public static void AppendEmotionsAction(
            this ToolbarMenu toolbarMenu,
            List<Emotion> emotions,
            Emotion savedEmotion,
            Action<DropdownMenuAction> action = null
        )
        {
            var savedCharacter = emotions.Find(c => c.Equals(savedEmotion));
            toolbarMenu.text = savedCharacter.ToString();

            foreach (var emotion in emotions)
            {
                toolbarMenu.menu.AppendAction(
                    actionName: emotion.ToString(),
                    action: action,
                    actionStatusCallback: a => DropdownMenuAction.Status.Normal,
                    userData: emotion);
            }
        }

        public static ObjectField CreateObjectField<T>(
            string title = "",
            EventCallback<ChangeEvent<UnityEngine.Object>> onValueChanged = null
        )
        {
	        var objectField = new ObjectField
            {
                objectType = typeof(T),
                label = title
            };

            if (onValueChanged != null)
                objectField.RegisterCallback(onValueChanged);

            return objectField;
        }
    }
}

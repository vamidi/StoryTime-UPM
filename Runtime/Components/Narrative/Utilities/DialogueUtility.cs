using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

namespace StoryTime.Components
{
	public enum DialogueCommandType
	{
		Pause,
		TextSpeedChange,
		AnimStart,
		AnimEnd,
		Emotion,
		Action
	}

	public enum TextAnimationType
	{
		None,
		Shake,
		Wave
	}

	public enum Emotion
	{
		Normal,
		Angry,
		Happy,
		Sad,
		Scared,
		Surprised,
		Blank
		// Extend to your liking.
	}

	public struct DialogueCommand
	{
		// public Vector2Int Positions;
		public int Position;
		public DialogueCommandType Type;
		public float FloatValue;
		public string StringValue;
		public TextAnimationType TextAnimValue;
		public Emotion TextEmotionValue;
	}

	public class DialogueUtility
	{
	    // grab the remainder of the text until ">" or end of string
        private const string REMAINDER_REGEX = "(.*?((?=>)|(/|$)))";

        private const string PAUSE_REGEX_STRING = "<pause=(?<pause>" + REMAINDER_REGEX + ")>";
        private static readonly Regex PauseRegex = new Regex(PAUSE_REGEX_STRING);

        private const string SPEED_REGEX_STRING = "<speed=(?<speed>" + REMAINDER_REGEX + ")>";
        private static readonly Regex SpeedRegex = new Regex(SPEED_REGEX_STRING);

        private const string EMOTION_REGEX_STRING = "<emotion=(?<emotion>" + REMAINDER_REGEX + ")>";
        private static readonly Regex EmotionRegex = new Regex(EMOTION_REGEX_STRING);

        private const string ACTION_REGEX_STRING = "<action=(?<action>" + REMAINDER_REGEX + ")>";
        private static readonly Regex ActionRegex = new Regex(ACTION_REGEX_STRING);

        private const string ANIM_START_REGEX_STRING = "<anim=(?<anim>" + REMAINDER_REGEX + ")>";
        private static readonly Regex AnimStartRegex = new Regex(ANIM_START_REGEX_STRING);
        private const string ANIM_END_REGEX_STRING = "</anim>";
        private static readonly Regex AnimEndRegex = new Regex(ANIM_END_REGEX_STRING);

		// Extend to enable more regular expressions

        /*private readonly Dictionary<string, TagExpression> m_Expressions = new Dictionary<string, TagExpression>{
	        { "action", new TagExpression(@"<action=(.*?)>", @"(?<=<action=)(.*?)(?=>)") }, // Get action => these are things that We should fix
	        { "emotion", new TagExpression(@"<emotion=(.*?)>", @"(?<=<emotion=)(.*?)(?=>)") }, // Get emotion => these are things that We should fix
	        { "speed", new TagExpression(@"<speed=(.*?)>", @"(?<=<speed=)(.*?)(?=>)") }, // Get emotion => these are things that We should fix
	        { "tmp_tag", new TagExpression(@"(?<=<(.*?)>)(.*?)(?=<\/\k<1>>)", "") }, // Get tags with value that should be handled by TMP
	        { "tmp_tag_value", new TagExpression(@"(?=<(.*?)=(.*?)>)(.*?)</\k<1>>", @"(?=<(.*?)=(.*?)>)(.*?)</\k<1>>") } // Get tags with value that should be handled by TMP
        };*/

        public static List<DialogueCommand> ProcessInputString(string message, out string processedMessage)
        {
	        List<DialogueCommand> result = new List<DialogueCommand>();
	        processedMessage = message;

	        // TODO make one function to handle all the normal regular expressions.
	        processedMessage = HandlePauseTags(processedMessage, result);
	        processedMessage = HandleSpeedTags(processedMessage, result);
	        processedMessage = HandleEmotionTags(processedMessage, result);
	        processedMessage = HandleActionTags(processedMessage, result);
	        processedMessage = HandleAnimStartTags(processedMessage, result);
	        processedMessage = HandleAnimEndTags(processedMessage, result);

	        return result;
        }

        private static string HandlePauseTags(string processedMessage, List<DialogueCommand> result)
        {
	        MatchCollection pauseMatches = PauseRegex.Matches(processedMessage);
	        foreach (Match match in pauseMatches)
	        {
		        string val = match.Groups["pause"].Value;
		        Debug.Assert(float.TryParse(val, out var pauseValue), "Pause value could not be parsed");
		        result.Add(new DialogueCommand
		        {
			        // Positions = VisibleCharactersUpToIndex(match),
			        Position = VisibleCharactersUpToIndex(processedMessage, match.Index),
			        Type = DialogueCommandType.Pause,
			        FloatValue = pauseValue
		        });
	        }
	        processedMessage = Regex.Replace(processedMessage, PAUSE_REGEX_STRING, "");
	        return processedMessage;
        }

        private static string HandleSpeedTags(string processedMessage, List<DialogueCommand> result)
        {
	        MatchCollection speedMatches = SpeedRegex.Matches(processedMessage);
	        foreach (Match match in speedMatches)
	        {
		        string stringVal = match.Groups["speed"].Value;
		        if (!float.TryParse(stringVal, out float val))
		        {
			        val = 150f;
		        }
		        result.Add(new DialogueCommand
		        {
			        // Positions = VisibleCharactersUpToIndex(match),
			        Position = VisibleCharactersUpToIndex(processedMessage, match.Index),
			        Type = DialogueCommandType.TextSpeedChange,
			        FloatValue = val
		        });
	        }
	        processedMessage = Regex.Replace(processedMessage, SPEED_REGEX_STRING, "");
	        return processedMessage;
        }

        private static string HandleAnimStartTags(string processedMessage, List<DialogueCommand> result)
        {
	        MatchCollection animStartMatches = AnimStartRegex.Matches(processedMessage);
	        foreach (Match match in animStartMatches)
	        {
		        string stringVal = match.Groups["anim"].Value;
		        result.Add(new DialogueCommand
		        {
			        Position = VisibleCharactersUpToIndex(processedMessage, match.Index),
			        Type = DialogueCommandType.AnimStart,
			        TextAnimValue = GetTextActionType<TextAnimationType>(stringVal)
		        });
	        }
	        processedMessage = Regex.Replace(processedMessage, ANIM_START_REGEX_STRING, "");
	        return processedMessage;
        }

        private static string HandleAnimEndTags(string processedMessage, List<DialogueCommand> result)
        {
	        MatchCollection animEndMatches = AnimEndRegex.Matches(processedMessage);
	        foreach (Match match in animEndMatches)
	        {
		        var command = new DialogueCommand
		        {
			        Position = VisibleCharactersUpToIndex(processedMessage, match.Index),
			        Type = DialogueCommandType.AnimEnd,
		        };
		        result.Add(command);
	        }
	        processedMessage = Regex.Replace(processedMessage, ANIM_END_REGEX_STRING, "");
	        return processedMessage;
        }

        private static string HandleEmotionTags(string processedMessage, List<DialogueCommand> result)
        {
	        MatchCollection emotionMatches = EmotionRegex.Matches(processedMessage);
	        foreach (Match match in emotionMatches)
	        {
		        string stringVal = match.Groups["emotion"].Value;
		        result.Add(new DialogueCommand
		        {
			        // Positions = VisibleCharactersUpToIndex(match),
			        Position = VisibleCharactersUpToIndex(processedMessage, match.Index),
			        Type = DialogueCommandType.Emotion,
			        TextEmotionValue = GetTextActionType<Emotion>(stringVal)
		        });
	        }
	        processedMessage = Regex.Replace(processedMessage, EMOTION_REGEX_STRING, "");
	        return processedMessage;
        }

        private static string HandleActionTags(string processedMessage, List<DialogueCommand> result)
        {
	        MatchCollection actionMatches = ActionRegex.Matches(processedMessage);
	        foreach (Match match in actionMatches)
	        {
		        result.Add(new DialogueCommand
		        {
			        // Positions = VisibleCharactersUpToIndex(match),
			        Position = VisibleCharactersUpToIndex(processedMessage, match.Index),
			        Type = DialogueCommandType.Action,
			        StringValue = match.Groups["action"].Value
		        });
	        }
	        processedMessage = Regex.Replace(processedMessage, ACTION_REGEX_STRING, "");
	        return processedMessage;
        }

        // Add more tags handler to your liking.

        private static T GetTextActionType<T>(string stringVal) where T: Enum
        {
	        T result;
	        try
	        {
		        result = (T)Enum.Parse(typeof(T), stringVal, true);
	        }
	        catch (ArgumentException)
	        {
		        Debug.LogError("Invalid Text Type: " + stringVal);
		        result = default;
	        }
	        return result;
        }

        private static int VisibleCharactersUpToIndex(string message, int index)
        {
	        int result = 0;
	        bool insideBrackets = false;
	        for (int i = 0; i < index; i++)
	        {
		        if (message[i] == '<')
		        {
			        insideBrackets = true;
		        }
		        else if (message[i] == '>')
		        {
			        insideBrackets = false;
			        result--;
		        }
		        if (!insideBrackets)
		        {
			        result++;
		        }
		        else if (i + 6 < index && message.Substring(i, 6) == "sprite")
		        {
			        result++;
		        }
	        }
	        return result;
        }
	}
}

using System;

namespace StoryTime.Components.Debugging
{
	public class DebugCommandBase
	{
		public string CommandId => m_CommandId;
		public string CommandDescription => m_CommandDescription;
		public string CommandFormat => m_CommandFormat;

		private string m_CommandId;
		private string m_CommandDescription;
		private string m_CommandFormat;

		public DebugCommandBase(string id, string description, string format)
		{
			m_CommandId = id;
			m_CommandDescription = description;
			m_CommandFormat = format;
		}
	}

	public class DebugCommand : DebugCommandBase
	{
		private Action m_Command;

		public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
		{
			m_Command = command;
		}

		public void Invoke()
		{
			m_Command.Invoke();
		}
	}

	public class DebugCommand<T1> : DebugCommandBase
	{
		private Action<T1> m_Command;

		public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
		{
			m_Command = command;
		}

		public void Invoke(T1 value)
		{
			m_Command.Invoke(value);
		}
	}
}

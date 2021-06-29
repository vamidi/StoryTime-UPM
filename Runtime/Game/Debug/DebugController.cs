using System.Collections.Generic;
using UnityEngine;

namespace DatabaseSync.Game.Components
{
	using Input;
	public class DebugController : MonoBehaviour
	{
		public static DebugCommand HELP;
		public static DebugCommand KILL_ALL;
		public static DebugCommand<int> SET_GOLD;

		[SerializeField] private BaseInputReader inputReader;

		[Header("Commands")]
		public List<object> commandList;

		private bool m_ShowConsole;
		private bool m_ShowHelp;

		private string m_Input;
		private Vector2 m_Scroll;
		public void OnToggleDebug()
		{
			m_ShowConsole = !m_ShowConsole;
		}

		public void OnReturn()
		{
			if (m_ShowConsole)
			{
				HandleInput();
				m_Input = "";
			}
		}

		private void Awake()
		{
			KILL_ALL = new DebugCommand(
				"kill_all",
				"Removes all heroes from the scene",
				"kill_all",
				() =>
				{
					Debug.Log("Kill all heroes");
				}
			);

			SET_GOLD = new DebugCommand<int>(
				"set_gold",
				"Sets the amount of gold",
				"set_gold <gold_amount>",
				x => Debug.Log(x)
			);

			HELP = new DebugCommand(
				"help",
				"Shows a list of commands",
				"help",
				() =>
				{
					m_ShowHelp = true;
				}
			);

			commandList = new List<object>
			{
				HELP,
				KILL_ALL,
				SET_GOLD
			};
		}

		private void Start()
		{
			if (inputReader) inputReader.toggleDebugEvent += OnToggleDebug;
			if (inputReader) inputReader.returnEvent += OnReturn;
		}

		private void OnGUI()
		{
			if (!m_ShowConsole) return;

			float y = 0f;

			if (m_ShowHelp)
			{
				GUI.Box(new Rect(0, y, Screen.width, 100), "");
				Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
				m_Scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), m_Scroll, viewport);

				for (int i = 0; i < commandList.Count; i++)
				{
					DebugCommandBase command = commandList[i] as DebugCommandBase;

					string label = $"{command.CommandFormat} - {command.CommandDescription}";

					Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
					GUI.Label(labelRect, label);
				}

				GUI.EndScrollView();

				y += 100;
			}

			GUI.Box(new Rect(0, y, Screen.width, 30), "");
			GUI.backgroundColor = new Color(0, 0, 0, 0);

			m_Input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), m_Input);
		}

		private void HandleInput()
		{
			string[] properties = m_Input.Split(' ');
			foreach (DebugCommandBase command in commandList)
			{
				if (m_Input.Contains(command.CommandId))
				{
					if (command is DebugCommand debugCommand)
					{
						debugCommand.Invoke();
					}
					else if (command is DebugCommand<int> debugIntCommand)
					{
						debugIntCommand.Invoke(int.Parse(properties[1]));
					}
				}
			}
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

namespace DatabaseSync.Components
{
	[CreateAssetMenu(fileName = "Quest", menuName = "DatabaseSync/Quests/Quest", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class QuestSO : TableBehaviour
	{
		[Tooltip("The collection of tasks composing the Quest")]

		[SerializeField]
		private List<TaskSO> tasks = new List<TaskSO>();

		[SerializeField]
		bool isDone;

		public List<QuestEvent> QuestEvents = new List<QuestEvent>();

		public void Print()
		{
			foreach (var questEvent in QuestEvents)
			{
				Debug.Log($"title: {questEvent.Title} order: {questEvent.Order}");
			}
		}

		// TODO will be automatic next time
		public QuestEvent AddQuestEvent(uint id, string title, string description)
		{
			QuestEvent questEvent = new QuestEvent
			{
				ID = id,
				Title = title,
				Description = description
			};
			QuestEvents.Add(questEvent);
			return questEvent;
		}

		public void AddPath(uint eventFromID, uint eventToID)
		{
			QuestEvent from = FindQuestEvent(eventFromID);
			QuestEvent to = FindQuestEvent(eventToID);

			if(from != null && to != null)
			{
				QuestPath questPath = new QuestPath
				{
					StartEvent = from,
					EndEvent = to
				};
				from.QuestPaths.Add(questPath);
			}
		}

		// ReSharper disable once InconsistentNaming
		public void BFS(uint id, int orderNum = 1)
		{
			QuestEvent thisEvent = FindQuestEvent(id);
			thisEvent.Order = orderNum;

			foreach (var path in thisEvent.QuestPaths)
			{
				if (path.EndEvent.Order == -1)
					BFS(path.EndEvent.ID, orderNum + 1);
			}
		}

		protected QuestEvent FindQuestEvent(uint eventID)
		{
			foreach (var qEvent in QuestEvents)
			{
				if (qEvent.ID == eventID)
					return qEvent;
			}

			return null;
		}

		public List<TaskSO> Tasks => tasks;

		public bool IsDone => isDone;

		public void FinishQuest()
		{
			isDone = true;
		}

		public QuestSO() : base("quests", "title") { }
	}
}

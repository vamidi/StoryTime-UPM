using System;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.VisualScripting.Data;
using StoryTime.VisualScripting.Data.ScriptableObjects;
namespace StoryTime.Editor.VisualScripting.Utilities
{
	using Data;
	using Elements;

	public class GraphUtilities
	{
		private DialogueGraphView _targetGraphView;
		private DialogueContainerSO _containerCached;
		private List<Edge> Edges => _targetGraphView.edges.ToList();
		private Dictionary<string, DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>()
			.ToDictionary(node => node.GUID, node => node);

		public static GraphUtilities Instance(DialogueGraphView targetGraphView) => new() {_targetGraphView = targetGraphView};

		GraphUtilities()
		{
			// creates an Resources folder if there's none
			GenerateFolders();
		}

		internal void SaveGraph(string fileName)
		{
			var path = $"{Constants.FOLDER_GRAPH}/{fileName}.asset";
			var prevAsset = Resources.Load<DialogueContainerSO>(path);
			if (prevAsset == null)
			{
				SaveNewGraph(path);
			}
			else
			{
				UpdateGraph(prevAsset);
			}
		}

		internal void LoadGraph(DialogueContainerSO dialogContainer)
		{
			_containerCached = dialogContainer;
			if (!_containerCached)
			{
				EditorUtility.DisplayDialog("File not Found", "Target dialogue graph file does not exists!", "OK");
				return;
			}

			// ClearGraph();
			CreateNodes();
			// ConnectNodes();
			// CreateExposedProperties();
		}

		internal void ClearAll()
		{
			// _targetGraphView.DeleteElements(Nodes.Values.Where(t => t.DialogType != NodeTypes.Start));
			// remove edges connected to node
			_targetGraphView.DeleteElements(Edges);
		}

		private void SaveNewGraph(string path)
		{
			Debug.Log($"New graph {path}");
			var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainerSO>();

			SaveSockets(dialogueContainer);
			SaveNodes(dialogueContainer);
			SaveExposedProperties(dialogueContainer);

			Debug.Log(dialogueContainer);
			SaveAsset(dialogueContainer, path);
		}

		private void UpdateGraph(DialogueContainerSO dialogueContainer)
		{
			SaveSockets(dialogueContainer);
			SaveNodes(dialogueContainer);
			SaveExposedProperties(dialogueContainer);
			SaveAsset(dialogueContainer);
		}

		private void SaveAsset(DialogueContainerSO dialogueContainer, string fileName = "", bool update = false)
		{
			if (!update && fileName != String.Empty)
			{
				AssetDatabase.CreateAsset(dialogueContainer, fileName);
			}
			EditorUtility.SetDirty(dialogueContainer);
			AssetDatabase.SaveAssets();
			Selection.activeObject = dialogueContainer;
			SceneView.FrameLastActiveSceneView();
		}

		private void GenerateFolders()
		{
			if (!AssetDatabase.IsValidFolder("Assets/Resources"))
			{
				AssetDatabase.CreateFolder("Assets", "Resources");
			}

			if (!AssetDatabase.IsValidFolder(Constants.FOLDER_GRAPH))
			{
				AssetDatabase.CreateFolder("Assets/Resources", "Graphs");
			}
		}

		private void CreateExposedProperties()
		{
			_targetGraphView.ClearBlackBoardAndExposedProperties();
			foreach (var exposedProperty in _containerCached.ExposedProperties)
			{
				_targetGraphView.AddPropertyToBlackBoard(exposedProperty);
			}
		}

		private void SaveExposedProperties(DialogueContainerSO dialogueContainer)
		{
			dialogueContainer.ExposedProperties.AddRange(_targetGraphView.ExposedProperties);
		}

		private void ClearGraph()
		{
			// set entry point discard existing guid
			// Nodes.Values.First(x => x.DialogType == NodeTypes.Start).GUID =
				// _containerCached.NodeLinks[0].BaseNodeGuid;

			ClearAll();
		}

		private void CreateNodes()
		{
			foreach (var nodeData in _containerCached.DialogueNodes)
			{
				var tempNode = _targetGraphView.CreateNode(nodeData);
				_targetGraphView.AddElement(tempNode);
			}
		}

		private void SaveSockets(DialogueContainerSO dialogueContainer)
		{
			if (!Edges.Any()) return; // if there are no edges

			var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
			foreach (var connectedSocket in connectedSockets)
			{
				var outputNode = connectedSocket.output.node as DialogueNode;
				var inputNode = connectedSocket.input.node as DialogueNode;
				var index = dialogueContainer.NodeLinks
					.FindIndex(l => l.BaseNodeGuid == outputNode.GUID &&
					                l.TargetNodeGuid == inputNode.GUID);

				NodeLinkData linkData = new()
				{
					BaseNodeGuid = outputNode.GUID,
					PortName = connectedSocket.output.portName,
					TargetNodeGuid = inputNode.GUID
				};

				// Update if we found a socket.
				if (index >= 0)
				{
					dialogueContainer.NodeLinks[index] = linkData;
				}
				else
				{
					dialogueContainer.NodeLinks.Add(linkData);
				}
			}
		}

		private void SaveNodes(DialogueContainerSO dialogueContainer)
		{
			/*
			foreach (var node in Nodes.Values.Where(node => node.DialogType != NodeTypes.Start))
			{
				DialogueNodeData dialogueNode = new(node);

				var index = dialogueContainer.DialogueNodes.FindIndex(p => p.Guid == node.GUID);
				// Update if we found a socket.
				if (index >= 0)
				{
					dialogueContainer.DialogueNodes[index] = dialogueNode;
				}
				else
				{
					dialogueContainer.DialogueNodes.Add(dialogueNode);
				}
			}
			*/
		}

		private void ConnectNodes()
		{
			for (var i = 0; i < Nodes.Count; i++)
			{
				var k = i; //Prevent access to modified closure
				/*
				var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[k].GUID).ToList();
				for (var j = 0; j < connections.Count; j++)
				{
					var targetNodeGUID = connections[j].TargetNodeGuid;
					var targetNode = Nodes.First(x => x.Value.GUID == targetNodeGUID);
					LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.Value.inputContainer[0]);
					targetNode.Value.SetPosition(new Rect(
						_containerCache.DialogueNodes.First(x => x.Guid == targetNodeGUID).Position,
						_targetGraphView.DefaultNodeSize));
				}
				*/
			}
		}

		private void LinkNodes(Port output, Port input)
		{
			var tempEdge = new Edge
			{
				output = output,
				input = input
			};

			tempEdge?.input.Connect(tempEdge);
			tempEdge?.output.Connect(tempEdge);
			_targetGraphView.Add(tempEdge);
		}
	}
}

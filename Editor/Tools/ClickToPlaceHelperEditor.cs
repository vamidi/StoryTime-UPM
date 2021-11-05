using UnityEngine;

namespace StoryTime.Editor
{

	[UnityEditor.CustomEditor(typeof(ClickToPlaceHelper))]
	public class ClickToPlaceHelperEditor : UnityEditor.Editor
	{
		private ClickToPlaceHelper clickHelper => target as ClickToPlaceHelper;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Place at Mouse cursor") && !clickHelper.IsTargeting)
			{
				clickHelper.BeginTargeting();
				UnityEditor.SceneView.duringSceneGui += DuringSceneGui;
			}
		}

		private void DuringSceneGui(UnityEditor.SceneView sceneView)
		{
			Event currentGUIEvent = Event.current;

			Vector3 mousePos = currentGUIEvent.mousePosition;
			float pixelsPerPoint = UnityEditor.EditorGUIUtility.pixelsPerPoint;
			mousePos.y = sceneView.camera.pixelHeight - mousePos.y * pixelsPerPoint;
			mousePos.x *= pixelsPerPoint;

			Ray ray = sceneView.camera.ScreenPointToRay(mousePos);

			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				clickHelper.UpdateTargeting(hit.point);
			}

			switch (currentGUIEvent.type)
			{
				case EventType.MouseMove:
					UnityEditor.HandleUtility.Repaint();
					break;
				case EventType.MouseDown:
					if (currentGUIEvent.button == 0) // Wait for Left mouse button down
					{
						clickHelper.EndTargeting();
						UnityEditor.SceneView.duringSceneGui -= DuringSceneGui;
						currentGUIEvent
							.Use(); // This consumes the event, so that other controls/buttons won't be able to use it
					}
					break;
			}
		}
	}
}


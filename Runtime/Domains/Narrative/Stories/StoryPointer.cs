using UnityEngine;
using UnityEngine.UI;

namespace StoryTime.Domains.Narrative.Stories
{
	using Utilities.Extensions;

	public class StoryPointer : MonoBehaviour
	{
		[SerializeField] private Camera uiCamera;
		[SerializeField] private float borderSize = 100f;
		[SerializeField, Tooltip("UI text distance in meters")] private Text meter;

		private Vector3 targetPosition;
		private RectTransform pointerRectTransform;

		private void Awake()
		{
			targetPosition = new Vector3(200, 45);
			pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
		}

		private void Update()
		{
			Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
			bool isOffScreen = targetPositionScreenPoint.x <= 0 || targetPositionScreenPoint.x > Screen.width ||
			                   targetPositionScreenPoint.y <= 0 || targetPositionScreenPoint.y > Screen.height;

			if (isOffScreen)
			{
				RotatePointerTowardsTargetPosition();
				Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
				if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
				if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
				if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.x = borderSize;
				if (cappedTargetScreenPosition.y <= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height -borderSize;

				Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
				pointerRectTransform.position = pointerWorldPosition;
				var localPosition = pointerRectTransform.localPosition;
				localPosition = new Vector3(localPosition.x, localPosition.y, 0f);
				pointerRectTransform.localPosition = localPosition;
			}
			else
			{
				Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
				pointerRectTransform.position = pointerWorldPosition;
				var localPosition = pointerRectTransform.localPosition;
				localPosition = new Vector3(localPosition.x, localPosition.y, 0f);
				pointerRectTransform.localPosition = localPosition;

				pointerRectTransform.localEulerAngles = Vector3.zero;
			}

			if(meter)
				meter.text = Vector3.Distance(targetPosition, transform.position).ToString("0") + "m";
		}

		private void RotatePointerTowardsTargetPosition()
		{
			Vector3 toPosition = targetPosition;
			Vector3 fromPosition = Camera.main.transform.position;
			fromPosition.z = 0f;
			Vector3 dir = (toPosition - fromPosition).normalized;
			float angle = this.GetAngleFromVectorFloat(dir);
			pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
		}
	}
}

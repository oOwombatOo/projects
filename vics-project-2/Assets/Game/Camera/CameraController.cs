using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	private static readonly float CAMERA_X_ON_SPACE_ZERO = -10;
	private static readonly float CAMERA_Y_ON_SPACE_ZERO = -10;
	private static readonly float CAMERA_Z_ON_SPACE_ZERO = 10;
	private static readonly Vector3 CAMERA_FOCUSSED_AT_ZERO = new Vector3(CAMERA_X_ON_SPACE_ZERO, CAMERA_Y_ON_SPACE_ZERO, CAMERA_Z_ON_SPACE_ZERO);

	// This is to give a mathematically true orthographic camera angle (according the Google Gemini)
	private readonly Vector3 TRUE_ORTHOGRAPHIC_CAMERA_ANGLE = new Vector3(35.26439f, -45, 0);

	// This is to rotate the GameObject that contains the camera to align with out world view 
	// (that is, Z-being up and looking at a 45 degree angle to the scene
	private readonly Vector3 CAMERA_PIVOT_ANGLE = new Vector3(270, 180, 0);

	[SerializeField] private Camera childCamera;

	public static EventHandler<RequestCameraFocusEventArgs> OnRequestCameraFocus;

	public class RequestCameraFocusEventArgs : EventArgs
	{
		public Vector3 RequestedFocusWorldPosition;
	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
	{
		if (this.childCamera != null)
		{
			Vector3 orthoCameraAngle = TRUE_ORTHOGRAPHIC_CAMERA_ANGLE;
			this.childCamera.transform.localEulerAngles = orthoCameraAngle;
			this.childCamera.orthographic = true;

			/* From eyeballing it, it seems that a size of "1" is squal to 2 Unity Units (i.e 2m) on
			the vertical plane */
			this.childCamera.orthographicSize = 10;
		}

		Vector3 pivotAngle = CAMERA_PIVOT_ANGLE;
		this.transform.localPosition = CAMERA_FOCUSSED_AT_ZERO;
		this.transform.localEulerAngles = pivotAngle;

	}


	private void HandleRequestCameraFocus(object sender, CameraController.RequestCameraFocusEventArgs eventArgs)
	{
		Vector3 worldPosition = eventArgs.RequestedFocusWorldPosition;
		Vector3 cameraPivotPos = CAMERA_FOCUSSED_AT_ZERO + new Vector3(worldPosition.x, worldPosition.y, 0f);
		this.transform.localPosition = cameraPivotPos;
	}

	private void OnEnable()
	{
		CameraController.OnRequestCameraFocus += this.HandleRequestCameraFocus;
	}


	private void OnDisable()
	{
		CameraController.OnRequestCameraFocus -= this.HandleRequestCameraFocus;
	}

}

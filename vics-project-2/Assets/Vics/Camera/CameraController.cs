using UnityEngine;

public class CameraController : MonoBehaviour
{

	private readonly float zeroedX = -50;
	private readonly float zeroedY = -50;
	private readonly float zeroedZ = 50;

	[SerializeField] private Camera childCamera;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (this.childCamera != null)
		{
			GridSpace.OnGridSpaceFocused += gridSpace_FocusOn;
			Vector3 orthoCameraAngle = new Vector3(35, -45, 0);
			this.childCamera.transform.localEulerAngles = orthoCameraAngle;
		}

		Vector3 pivotAngle = new Vector3(270, 180, 0);
		this.transform.position = new Vector3(this.zeroedX, this.zeroedY, this.zeroedZ);
		this.transform.eulerAngles = pivotAngle;
	}


	private void gridSpace_FocusOn(object sender, GridSpace.OnGridSpaceFocusedEventArgs e)
	{
		GridSpace gridSpace = e.gridSpace;
		this.FocusOn(gridSpace);
	}


	public void FocusOn(GridSpace gridSpace)
	{
		Vector3 spacePosition = gridSpace.GetWorldPosition();
		float x = this.zeroedX + spacePosition.x;
		float y = this.zeroedY + spacePosition.y;

		Vector3 cameraPivotPos = new Vector3(x, y, this.zeroedZ);
		this.transform.position = cameraPivotPos;

		Debug.Log("Focused Camera: " + x + "," + y);
	}

}

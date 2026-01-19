using UnityEngine;

public class CameraResolution : MonoBehaviour
{

	[SerializeField] private int width = 640;
	[SerializeField] private int height = 360;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
	{
		Screen.SetResolution(width, height, false);
		Debug.Log("Resolution set to: " + width + "x" + height);
	}
}

using UnityEngine;

public class Player : MonoBehaviour
{

	[SerializeField] private float moveSpeed = 7;

	// Update is called once per frame
	private void Update()
	{

		Vector2 inputVector = new Vector2(0, 0);

		if (Input.GetKey(KeyCode.W))
		{
			inputVector.y = +1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			inputVector.y = -1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			inputVector.x = -1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			inputVector.x = +1;
		}

		Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);

		transform.position += moveDirection * moveSpeed * Time.deltaTime;

		inputVector.Normalize();

		// Using this variable is unecessary, I'm doing it for clarity
		// (i.e. it will still be the old one if we use transform.forward in the
		// Slerp function, but it's not obvious at first glance)
		Vector3 currentFacingVector = transform.forward;

		// moveDirection = "the facing direction we want to have"
		// Time.deltaTime = "roughly, how much time does one frame take"
		// Slerp means to interpolate between two vectors
		// "Interpolate" means to guess a value in between two known values
		// The last value t (time) is really nothing to do with game time. It's
		// an arbitrary value between 0 and 1 that indicates how far along the
		// interpolation we want to be. 0 means "all the way at the first vector",
		// 1 means "all the way at the second vector", and 0.5 means "halfway between the two".
		// The Time.delatTime just gives me a nice result.
		transform.forward = Vector3.Slerp(currentFacingVector, moveDirection, Time.deltaTime * 7);
	}
}


using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

	[SerializeField] private ClearCounter clearCounter;
	[SerializeField] private GameObject visualGameObject;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
	{
		Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
	}

	private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
	{
		bool isThisCounter = e.selectedCounter == this.clearCounter;

		if (isThisCounter) this.Show();
		else this.Hide();
	}

	private void Show()
	{
		visualGameObject.SetActive(true);
	}

	private void Hide()
	{
		visualGameObject.SetActive(false);
	}
}

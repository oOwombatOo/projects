using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

	private Animator animator;

	private const string IS_WALKING = "IsWalking";

	[SerializeField] private Player player;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		animator.SetBool(IS_WALKING, false);
	}

	private void Update()
	{
		bool isWalking = player.IsWalking();
		animator.SetBool(IS_WALKING, isWalking);
	}

}

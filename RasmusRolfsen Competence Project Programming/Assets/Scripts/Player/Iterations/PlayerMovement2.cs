using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 5;
	[SerializeField]
	private float jumpPower = 30;

	private enum PlayerState { idle, running, jumping, blocking, dashing };
	[SerializeField]
	private PlayerState currentState = PlayerState.idle;

	[SerializeField]
	private KeyCode moveLeft = KeyCode.A;
	[SerializeField]
	private KeyCode moveRight = KeyCode.D;
	[SerializeField]
	private KeyCode jumpKey = KeyCode.Space;

	[SerializeField]
	private bool secondJump = false;
	private Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		if (currentState == PlayerState.idle)
		{
			Idle();
		}
		else if (currentState == PlayerState.running)
		{
			Running();
		}
	}

	private void FixedUpdate()
	{
		if (rb.velocity.y != 0)
		{
			currentState = PlayerState.jumping;
		}

		if (currentState == PlayerState.jumping)
		{
			Jumping();
		}
	}

	// player stats
	void Idle()
	{
		if (Input.GetKey(moveLeft) || Input.GetKey(moveRight))
		{
			currentState = PlayerState.running;
		}
		if (Input.GetKeyDown(jumpKey))
		{
			Jump();
		}
	}

	void Running()
	{
		if (Input.GetKeyDown(jumpKey))
		{
			Jump();
		}

		if (Input.GetKey(moveLeft) || Input.GetKey(moveRight))
		{
			if (Input.GetKey(moveLeft))
			{
				MovePlayer(-1);
			}
			else
			{
				MovePlayer(1);
			}
		}
		else
		{
			currentState = PlayerState.idle;
		}
	}


	void Jumping()
	{
		if (rb.velocity.y == 0)
		{
			secondJump = false;
			currentState = PlayerState.idle;
			return;
		}

		if (Input.GetKey(moveLeft))
		{
			MovePlayer(-1);
		}
		if (Input.GetKey(moveRight))
		{
			MovePlayer(1);
		}

		if (Input.GetKeyDown(jumpKey) && secondJump == false)
		{
			Debug.Log("a");
			Jump();
			secondJump = true;
		}
	}

	// Player Movements
	void MovePlayer(int dir)
	{
		rb.MovePosition(transform.position + dir * transform.right * Time.deltaTime * movementSpeed);
	}

	void Jump()
	{
		rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
	}
}
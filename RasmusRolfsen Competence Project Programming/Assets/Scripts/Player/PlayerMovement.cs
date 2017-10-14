using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 5;
	[SerializeField]
	private float jumpPower = 30;

	private enum PlayerState { idle, running, jumping, blocking, dashing };
	private enum MovementDir { left, right, none };
	[SerializeField]
	private PlayerState currentState = PlayerState.idle;
	[SerializeField]
	private MovementDir currentDir = MovementDir.right;

	[SerializeField]
	private KeyCode moveLeft = KeyCode.A;
	[SerializeField]
	private KeyCode moveRight = KeyCode.D;
	[SerializeField]
	private KeyCode jumpKey = KeyCode.Space;

	private Rigidbody rb;
	private float prevY;
	private bool doubleJump;
	private bool doubleJumpStep;

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
		else if (currentState == PlayerState.jumping)
		{
			Jumping();
		}
	}

	private void FixedUpdate()
	{
		if (rb.velocity.y != 0)
		{
			currentState = PlayerState.jumping;
		}
	}

	void Idle()
	{
		if (Input.GetKey(moveLeft) || Input.GetKey(moveRight))
		{
			if (Input.GetKey(moveLeft))
			{
				currentDir = MovementDir.left;
			}
			else
			{
				currentDir = MovementDir.right;
			}
			currentState = PlayerState.running;
		}

		if (Input.GetKeyDown(jumpKey))
		{
			Jump();
			currentState = PlayerState.jumping;
		}
	}

	void Running()
	{
		movePlayer();

		if (Input.GetKey(moveRight) == false && currentDir == MovementDir.right)
		{
			currentState = PlayerState.idle;
		}
		else if (Input.GetKey(moveLeft) == false && currentDir == MovementDir.left)
		{
			currentState = PlayerState.idle;
		}

		if (Input.GetKeyDown(jumpKey))
		{
			Jump();
			currentState = PlayerState.jumping;
		}
	}

	void Jump()
	{
		rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
		rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
	}

	void movePlayer()
	{
		if (currentDir == MovementDir.left)
		{
			transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
		}
		if (currentDir == MovementDir.right)
		{
			transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
		}
	} 

	void Jumping()
	{
		if (Input.GetKeyDown(jumpKey) && doubleJump == false)
		{
			Jump();
			doubleJump = true;
			doubleJumpStep = true;
		}

		movePlayer();
		
		if (Input.GetKey(moveRight) == false && currentDir == MovementDir.right)
		{
			currentDir = MovementDir.none;
		}
		else if (Input.GetKey(moveLeft) == false && currentDir == MovementDir.left)
		{
			currentDir = MovementDir.none;
		}

		if (rb.velocity.y == 0 && doubleJumpStep == false)
		{
			doubleJump = false;
			currentState = PlayerState.idle;
		}
		doubleJumpStep = false;
	}
}
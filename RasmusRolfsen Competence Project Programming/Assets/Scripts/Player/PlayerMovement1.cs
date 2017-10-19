using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
	// jumping state specify and falling
	public enum PlayerState { idle, running, jumping, blocking, dashing, falling };
	public enum MovementDir { left, right, none };
	[SerializeField]
	public PlayerState currentState = PlayerState.idle;
	[SerializeField]
	public MovementDir currentDir = MovementDir.right;

	[SerializeField]
	private float movementSpeed = 5;
	[SerializeField]
	private float jumpPower = 30;
	[SerializeField]
	private float blockMaxDuration = 3;
	[SerializeField]
	private float blockCooldown = 3;

	[SerializeField]
	private KeyCode blockKey = KeyCode.LeftControl;
	[SerializeField]
	private KeyCode moveLeft = KeyCode.A;
	[SerializeField]
	private KeyCode moveRight = KeyCode.D;
	[SerializeField]
	private KeyCode jumpKey = KeyCode.Space;

	private bool blockReady = true;
	private bool doubleJump = false;
	private bool floorCol = false;

	private Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		EventManager.StartListening("GroundCollision", GroundCol);
		blockReady = true;
	}


	// Update is called once per frame
	private void Update()
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
		else if (currentState == PlayerState.blocking)
		{
			Blocking();
		}
		else if (currentState == PlayerState.falling)
		{
			Falling();
		}
		else
			Debug.Assert(false);
		// Insert debug message

		floorCol = false;
	}

	private void FixedUpdate()
	{
		if (rb.velocity.y != 0 && currentState != PlayerState.falling)
		{
			currentState = PlayerState.jumping;
		}
	}


	// Functions that handle players states and execute player movement
	private void Idle()
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
			return;
		}

		if (Input.GetKeyDown(jumpKey))
		{
			Jump(jumpPower);
			currentState = PlayerState.jumping;
			return;
		}
		AnyStateActions();
	}

	private void Running()
	{
		movePlayer();
		AnyStateActions();

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
			currentState = PlayerState.jumping;
			Jump(jumpPower);
		}
	}
	// MidAirJumping
	private void Jumping()
	{
		if (rb.velocity.y == 0 && floorCol)
		{
			doubleJump = false;
			currentState = PlayerState.idle;
			Debug.Log("Jump Over");
			return;
		}

		if (Input.GetKeyDown(jumpKey) && doubleJump == false)
		{
			Jump(jumpPower);
			doubleJump = true;
		}

		movePlayer();
		AnyStateActions();

		if (Input.GetKey(moveLeft))
		{
			currentDir = MovementDir.left;
		}
		else
		{
			currentDir = MovementDir.right;
		}

		if (Input.GetKey(moveRight) == false && currentDir == MovementDir.right)
		{
			currentDir = MovementDir.none;
		}
		else if (Input.GetKey(moveLeft) == false && currentDir == MovementDir.left)
		{
			currentDir = MovementDir.none;
		}
	}

	private void Falling()
	{
		if (rb.velocity.y >= 0)
		{
			//rb.velocity = (rb.velocity.x, 0, rb.velocity.z);
			//rb.velocity.y = 0;
			currentState = PlayerState.idle;
		}
	}


	// Functions that control player movement
	private void AnyStateActions()
	{
		if (Input.GetKey(blockKey) && blockReady)
		{
			StartCoroutine(BlockTimer());
			currentState = PlayerState.blocking;
		}
	}

	private void movePlayer()
	{
		if (currentDir == MovementDir.left)
		{
			rb.MovePosition(transform.position + -transform.right * movementSpeed * Time.deltaTime);
		}
		if (currentDir == MovementDir.right)
		{
			rb.MovePosition(transform.position + transform.right * movementSpeed * Time.deltaTime);
		}
	}
	private void Blocking()
	{
		rb.velocity = Vector3.zero;
		if (Input.GetKeyUp(blockKey))
		{
			BlockDone();
		}
	}

	public void Jump(float jumpStrength)
	{
		rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
	}

	void BlockDone ()
	{
		blockReady = false;
		StartCoroutine(BlockCooldown());
		currentState = PlayerState.falling;
	}

	IEnumerator BlockTimer()
	{
		yield return new WaitForSeconds(blockMaxDuration);
		BlockDone();
	}
	IEnumerator BlockCooldown()
	{
		yield return new WaitForSeconds(blockCooldown);
		blockReady = true;
	}

	private void GroundCol (object e)
	{
		floorCol = true;
	}
}
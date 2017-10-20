using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	// jumping state specify and falling
	public enum PlayerState { idle, running, airMovement, blocking, dashing, afterBlock };
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
	private bool isJumpKeyPressed = false;
	private bool haveDoubleJumped = false;
	private bool floorCol = false;
	private bool startBlock = false;

	private Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		EventManager.StartListening("GroundCollision", GroundCol);
		blockReady = true;
		isJumpKeyPressed = false;
		startBlock = false;
		floorCol = false;

	}


	// Update is called once per frame
	private void Update()
	{
		switch (currentState)
		{
			case PlayerState.idle: // player on ground without moving
				IdleStateHandler();
				break;
			case PlayerState.running: // player running on the ground
				RunningStateHandler();
				break;
			case PlayerState.airMovement: // player is moving in the middle of the air
				InAirStateHandler();
				break;
			case PlayerState.blocking: // player is using blocking
				BlockingStateHandler();
				break;
			case PlayerState.afterBlock: // state entered after blocking is over until colliding with ground
				AfterBlockStateHandler();
				break;
			default:
				Debug.Assert(false);
				break;
		}

	}

	private void FixedUpdate()
	{
		if (startBlock)
		{
			startBlock = false;
			currentState = PlayerState.blocking;
			EventManager.TriggerEvent("PlayerBlockState", true);
			StartCoroutine(BlockTimer());
		}

		else if (currentState == PlayerState.blocking)
		{
			
			rb.velocity = Vector3.zero;
		} 
		else
		{
			if (rb.velocity.y != 0 && currentState != PlayerState.afterBlock)
			{
				currentState = PlayerState.airMovement;
			}
			if (isJumpKeyPressed)
			{
				Jump(jumpPower);
				currentState = PlayerState.airMovement;
				isJumpKeyPressed = false;
			}
			if (currentState == PlayerState.running || currentState == PlayerState.airMovement)
			{
				movePlayer();
			}
		}
	}


	// Functions that changes the player state and processes keyinputs
	private void IdleStateHandler()
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
			isJumpKeyPressed = true;
			return;
		}

		AnyStateActions();
	}

	private void RunningStateHandler()
	{
		if (Input.GetKey(moveRight) == false && currentDir == MovementDir.right)
		{
			currentState = PlayerState.idle;
			return;
		}
		else if (Input.GetKey(moveLeft) == false && currentDir == MovementDir.left)
		{
			currentState = PlayerState.idle;
			return;
		}

		if (Input.GetKeyDown(jumpKey))
		{
			isJumpKeyPressed = true;
			return;
		}
		AnyStateActions();
	}

	private void InAirStateHandler()
	{
		if (rb.velocity.y == 0 && floorCol)
		{
			haveDoubleJumped = false;
			currentState = PlayerState.idle;
			floorCol = false;
			return;
		}

		if (Input.GetKeyDown(jumpKey) && haveDoubleJumped == false)
		{
			isJumpKeyPressed = true;
			haveDoubleJumped = true;
		}

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

	private void AfterBlockStateHandler()
	{
		if (rb.velocity.y >= 0)
		{
			haveDoubleJumped = false;
			currentState = PlayerState.idle;
		}
	}

	private void BlockingStateHandler()
	{
		
		if (Input.GetKeyUp(blockKey))
		{
			BlockDone();
		}
	}

	// Functions that control player movement and actions in states
	private void AnyStateActions()
	{
		if (Input.GetKey(blockKey) && blockReady)
		{
			startBlock = true;
		}
	}

	private void movePlayer()
	{
		if (currentDir == MovementDir.left)
		{
			rb.MovePosition(transform.position + -transform.right * movementSpeed * Time.deltaTime);
			return;
		}

		if (currentDir == MovementDir.right)
		{
			rb.MovePosition(transform.position + transform.right * movementSpeed * Time.deltaTime);
			return;
		}
	}


	public void Jump(float jumpStrength)
	{
		rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
	}

	void BlockDone()
	{
		EventManager.TriggerEvent("PlayerBlockState",false);
		blockReady = false;
		StartCoroutine(BlockCooldown());
		currentState = PlayerState.afterBlock;
	}

	private void GroundCol(object e)
	{
		floorCol = true;
	}

	public void RayCastDir (Vector3 dir)
	{
		Ray horizontalRay = new Ray(transform.position,dir);
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


}
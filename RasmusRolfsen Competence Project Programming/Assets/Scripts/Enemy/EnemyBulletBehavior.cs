using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{

	public int damage;
	public float moveSpeed;
	public Vector3 moveDir;

	[SerializeField]
	private float lifeTime = 6;

	private void Start()
	{
		StartCoroutine(DestroyMe());
	}

	private void Update()
	{

		transform.Translate(moveDir*moveSpeed*Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
			case ("Player"):
				PlayerCollision();
				break;
			case ("Ground"):
				DefaulCollision();
				break;
			default:
				break;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
			case ("Ground"):
				DefaulCollision();
				break;
			default:
				break;
		}
	}

	void DefaulCollision()
	{
		Destroy(gameObject);
	}

	void PlayerCollision()
	{
		EventManager.TriggerEvent("PlayerHealth", -damage,null);
		DefaulCollision();
	}

	IEnumerator DestroyMe ()
	{
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
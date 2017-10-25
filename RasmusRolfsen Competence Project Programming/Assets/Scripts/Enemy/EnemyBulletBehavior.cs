using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{

	public float damage;
	public float moveSpeed;
	public Vector3 moveDir;
	public List<ParticleEffect> particleEffects;

	[SerializeField]
	private float lifeTime = 6;

	private void Start()
	{
		StartCoroutine(DestroyMe());
	}

	private void Update()
	{

		transform.Translate(moveDir * moveSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
			case ("Player"):
				PlayerCollision();
				break;
			case ("Ground"):
				GroundCollision();
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
				GroundCollision();
				break;
			default:
				break;
		}
	}

	private void GroundCollision()
	{
		if (particleEffects.Count > 1)
		{
			EventManager.TriggerEvent(particleEffects[1].eventName,transform.position, particleEffects[1].amountOfParticles);
		}
		DefaulCollision();
	}

	private void DefaulCollision()
	{
		Destroy(gameObject);
	}

	private void PlayerCollision()
	{
		EventManager.TriggerEvent("PlayerHealth", -damage, null);
		if (particleEffects.Count > 0)
		{
			EventManager.TriggerEvent(particleEffects[0].eventName, transform.position, particleEffects[0].amountOfParticles);
		}
		DefaulCollision();
	}

	IEnumerator DestroyMe()
	{
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
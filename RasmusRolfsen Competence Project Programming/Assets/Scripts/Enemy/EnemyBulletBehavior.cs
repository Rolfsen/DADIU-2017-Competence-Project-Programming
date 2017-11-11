using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{

	public float damage;
	public float moveSpeed;
	public Vector3 moveDirectionNormilized;
	public List<ParticleEffect> particleEffects;

	[SerializeField]
	private float lifeTime = 6;

	private void Start()
	{
		StartCoroutine(DestroyMe());
	}

	private void Update()
	{
		Ray ray = new Ray(transform.position, moveDirectionNormilized);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, moveSpeed * Time.deltaTime))
		{
			if (hit.transform.tag == "Player")
			{
				PlayerCollision(hit.point);
			}
			else if (hit.transform.tag == "Ground")
			{
				GroundCollision(hit.point);
			}
		}
		else
		{
			transform.Translate(moveDirectionNormilized * moveSpeed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
			case ("Player"):
				PlayerCollision(other.transform.position);
				break;
			case ("Ground"):
				GroundCollision(other.transform.position);
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
				GroundCollision(collision.transform.position);
				break;
			default:
				break;
		}
	}

	private void GroundCollision(Vector3 point)
	{
		if (particleEffects.Count > 1)
		{
			EventManager.TriggerEvent(particleEffects[1].eventName, point, particleEffects[1].amountOfParticles);
		}
		DefaulCollision();
	}

	private void DefaulCollision()
	{
		Destroy(gameObject);
	}

	private void PlayerCollision(Vector3 point)
	{
		EventManager.TriggerEvent("PlayerHealth", -damage, null);
		if (particleEffects.Count > 0)
		{
			EventManager.TriggerEvent(particleEffects[0].eventName, point, particleEffects[0].amountOfParticles);
		}
		DefaulCollision();
	}

	IEnumerator DestroyMe()
	{
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
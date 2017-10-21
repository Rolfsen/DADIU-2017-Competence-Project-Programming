using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour
{


	[SerializeField]
	private Transform particleEffect;

	[SerializeField]
	private string triggerName;

	private void Awake()
	{
		EventManager.StartListening(triggerName, EmitParticles);
	}

	private void EmitParticles(object spawnPosition, object amount)
	{
		int particleAmount = (int)amount;

		Vector3 position = (Vector3)spawnPosition;
		Transform bloodEmitter = Instantiate(particleEffect);
		bloodEmitter.transform.position = position;
		var em = bloodEmitter.GetComponent<ParticleSystem>().emission;
		em.enabled = false;
		bloodEmitter.GetComponent<ParticleSystem>().Emit(particleAmount);

		StartCoroutine(DestroyParticle(bloodEmitter.gameObject, bloodEmitter.GetComponent<ParticleSystem>().main.duration));
	}

	IEnumerator DestroyParticle (GameObject particle, float particleLifetime)
	{
		yield return new WaitForSeconds(particleLifetime);
		Destroy(particle);
	}
}

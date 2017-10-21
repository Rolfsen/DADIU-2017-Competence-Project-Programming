using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour
{


	[SerializeField]
	Transform particleEffect;

	private void Awake()
	{
		EventManager.StartListening("SpawnBlood", SpawnBlood);
	}

	private void SpawnBlood(object spawnPosition)
	{
		
		Vector3 position = (Vector3)spawnPosition;
		Transform bloodEmitter = Instantiate(particleEffect);
		bloodEmitter.transform.position = position;
		var em = bloodEmitter.GetComponent<ParticleSystem>().emission;
		em.enabled = false;
		bloodEmitter.GetComponent<ParticleSystem>().Emit(50);
	}
}

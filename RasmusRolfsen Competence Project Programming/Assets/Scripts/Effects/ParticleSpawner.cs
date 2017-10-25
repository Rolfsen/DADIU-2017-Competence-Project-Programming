using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {

	public ParticleSystem pSystem;

	public void EmitParticle(ParticleSystem particleSystem, int amount, float liveTime)
	{
		particleSystem.Emit(amount);
		StartCoroutine(DestroyEffect(liveTime));
	}

	IEnumerator DestroyEffect (float time)
	{
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}
}

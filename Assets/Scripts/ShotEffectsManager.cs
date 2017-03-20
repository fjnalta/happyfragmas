using UnityEngine;

public class ShotEffectsManager : MonoBehaviour {

	[SerializeField] ParticleSystem sparks;
	[SerializeField] GameObject impactPrefab;

	ParticleSystem impactEffect;

	public void Initialize() {
		impactEffect = Instantiate (impactPrefab).GetComponent<ParticleSystem> ();
	}

	public void PlayImpactEffect(Vector3 impactPosition) {
		impactEffect.transform.position = impactPosition;
		impactEffect.Stop ();
		impactEffect.Play ();
	}
}

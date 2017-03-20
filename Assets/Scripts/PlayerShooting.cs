using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour {

	[SerializeField] float shotCooldown = .3f;
	[SerializeField] Transform firePosition;
	[SerializeField] ShotEffectsManager shotEffects;

	float ellapsedTime;
	bool canShoot;

	void Start() {
		shotEffects.Initialize ();

		if (isLocalPlayer) {
			canShoot = true;
		}
	}

	void Update() {
		if (!canShoot) {
			return;
		}

		ellapsedTime += Time.deltaTime;

		if (Input.GetButtonDown ("Fire1") && ellapsedTime > shotCooldown) {
			ellapsedTime = 0f;

			//Server needs to handle Shooting for security - command
			CmdFireShot (firePosition.position,firePosition.forward);
		}
	}

	// Code only processed on Server
	[Command]
	void CmdFireShot(Vector3 origin, Vector3 direction) {
		RaycastHit hit;
		Ray ray = new Ray (origin, direction);

		Debug.DrawRay (ray.origin, ray.direction * 3f, Color.red, 1f);

		bool result = Physics.Raycast (ray, out hit, 50f);

		if (result) {
			// if we hit reduce Health
			PlayerHealth enemy = hit.transform.GetComponent<PlayerHealth> ();
			// If there is a Player Health script on the Object
			if (enemy != null) {
				enemy.TakeDamage ();
			}
		}

		RpcProcessShotEffects (result, hit.point);
	}


	// Tell all Clients
	[ClientRpc]
	void RpcProcessShotEffects(bool playImpact, Vector3 point) {

		if (playImpact) {
			shotEffects.PlayImpactEffect (point);
		}
	}

}

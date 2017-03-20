﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

	[SerializeField] int maxHealth = 3;

	Player player;
	int health;

	void Awake () {
		player = GetComponent<Player> ();
	}

	// Method can only run on the Server - doesn't exist on clients
	[ServerCallback]
	void OnEnable() {
		health = maxHealth;
	}

	// Only Server is allowed to run the Method
	[Server]
	public bool TakeDamage() {
		bool died = false;

		if (health <= 0) {
			return died;
		}

		health--;
		died = health <= 0;

		RpcTakeDamage (died);

		return died;
	}

	// all Clients need to Process
	[ClientRpc]
	void RpcTakeDamage(bool died) {
		if (died) {
			player.Die ();
		}
	}
}
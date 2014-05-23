using UnityEngine;
using System.Collections;

public class Deathbox : MonoBehaviour {

	void OnTriggerEnter(Collider c){
		if(c.gameObject.CompareTag("Player")){
			c.GetComponent<CheckingMan>()._currentCheckpoint.GetComponent<Checkpoint>().StartRollback(true);
		}
	}

	void OnTriggerExit(Collider c){
		if(c.gameObject.CompareTag("Player")){
			c.GetComponent<CheckingMan>()._currentCheckpoint.GetComponent<Checkpoint>().StartRollback(false);
		}
	}

}

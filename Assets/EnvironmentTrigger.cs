using UnityEngine;
using System.Collections;

public class EnvironmentTrigger : MonoBehaviour {

	AmbientSounds _amb;

	void Awake(){

		_amb = GetComponent<AmbientSounds>();

	}
	
	void OnTriggerEnter(Collider c){
		if(c.CompareTag("Player"))
			_amb.enabled = true;
		
	}

	void OnTriggerExit(Collider c){
		if(c.CompareTag("Player"))
			_amb.enabled = false;
		
	}

}

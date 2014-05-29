using UnityEngine;
using System.Collections;

public class EnvironmentTrigger : MonoBehaviour {

	AmbientSounds _amb;

	void Awake(){

		_amb = GetComponent<AmbientSounds>();

	}
	
	void OnTriggerEnter(Collider c){
		
		_amb.enabled = true;
		
	}

	void OnTriggerExit(Collider c){
		
		_amb.enabled = false;
		
	}

}

using UnityEngine;
using System.Collections;

public class KillThrowObject : MonoBehaviour {

	string _playername = "HajPojken";

	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision (transform.collider, GameObject.FindWithTag("Player").collider, true); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision deadthing){

		if(deadthing.gameObject.tag == "Destructable" || deadthing.gameObject.light != null){
			deadthing.gameObject.light.enabled = false;
			deadthing.gameObject.audio.Play();
			Destroy(deadthing.gameObject,deadthing.gameObject.audio.clip.length);
		}
		Destroy (gameObject);
	}

}

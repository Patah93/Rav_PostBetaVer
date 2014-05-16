using UnityEngine;
using System.Collections;

public class KillThrowObject : MonoBehaviour {

	bool _isThrown = false;

	// Use this for initialization
	void Start () {
		_isThrown = false;
		Physics.IgnoreCollision (transform.collider, GameObject.FindWithTag("Player").collider, true); 
		audio.Stop();
	}
	
	// Update is called once per frame
	void Update () {

		if(transform.parent == null){
			_isThrown = true;
		}
	}

	void OnCollisionEnter(Collision deadthing){
		if(_isThrown){
			Debug.Log("Collided with:" + deadthing.transform.name);
			if(deadthing.gameObject.tag == "Destructable" || deadthing.gameObject.light != null){
				deadthing.gameObject.light.enabled = false;
				deadthing.gameObject.audio.Play();
				Destroy(deadthing.gameObject,deadthing.gameObject.audio.clip.length);
			}

			

			//audio.Play();
			if(!deadthing.gameObject.name.Equals("ThrowCube(Clone)")){
				audio.Play();
				Destroy (gameObject, 0.5f);
			}
		}

	}



}

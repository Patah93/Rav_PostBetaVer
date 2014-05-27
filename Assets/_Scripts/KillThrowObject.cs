using UnityEngine;
using System.Collections;

public class KillThrowObject : MonoBehaviour {

	public string _sandName = "Terrain"; 
	bool _isThrown = false;
	bool _JUSTDOITONCEFFS = false;
	bool _lock = false;
	Throw _throw;
	float _countdown;
//	AudioClip _currentSound;

	// Use this for initialization
	void Start () {
		_throw = GameObject.FindWithTag("Player").GetComponent<Throw>();
		_isThrown = false;
		_countdown = 0;
		//audio.Stop();
		_lock = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(!_lock){
			if(!_JUSTDOITONCEFFS){
				if(_throw.ThrowState()){
					_JUSTDOITONCEFFS = true;
					_lock = true;
				}
			}
		}

		if(transform.parent == null){
			_isThrown = true;
			Physics.IgnoreCollision (transform.collider, GameObject.FindWithTag("Player").collider, true); 
		}
	}

	void OnCollisionEnter(Collision deadthing){
		if(_isThrown){
			Debug.Log("Collided with:" + deadthing.transform.name);
			if(deadthing.gameObject.tag == "Destructable" || deadthing.gameObject.light != null){
				transform.audio.clip = gameObject.GetComponent<AudioClips>()._audios[1];
				deadthing.gameObject.light.enabled = false;
				transform.audio.Play();
				Destroy(deadthing.gameObject,transform.audio.clip.length);
				Debug.Log("JAG DOG AV EN LAMPA");
			}

			//audio.Play();
			else if(!deadthing.gameObject.name.Equals(transform.name)){
				transform.audio.clip =  gameObject.GetComponent<AudioClips>()._audios[0];
				if(!transform.audio.isPlaying && !deadthing.gameObject.name.Equals(_sandName) && _JUSTDOITONCEFFS){ //DEN HÄR FUNKAR INTE :(
					transform.audio.Play();
					_countdown =transform.audio.clip.length;
					_JUSTDOITONCEFFS = false;
				}
				Debug.Log("JAG DOG AV NÅGOT");
				Destroy (transform.gameObject, _countdown);
				Debug.Log ("Kuben dog, gg");
			}
		}
	}
}

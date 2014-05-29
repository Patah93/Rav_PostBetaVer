using UnityEngine;
using System.Collections;

public class KillThrowObject : MonoBehaviour {

	public string _sandName = "Terrain"; 
	bool _isThrown = false;
	bool _JUSTDOITONCEFFS = false;
	bool _lock = false;
	Throw _throw;
	float _countdown;

	AudioClips _groundRock, _groundSand, _groundMetall;
	AudioClips _sounds;
//	AudioClip _currentSound;
	
	// Use this for initialization
	void Start () {
		_sounds = GetComponent<AudioClips>();
		_throw = GameObject.FindWithTag("Player").GetComponent<Throw>();
		_isThrown = false;
		_countdown = 5;
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
			//Debug.Log("Collided with:" + deadthing.transform.name);
			if(deadthing.gameObject.tag == "Destructable" || deadthing.gameObject.light != null){
				transform.audio.clip = gameObject.GetComponent<AudioClips>()._audios[1];
				deadthing.gameObject.light.enabled = false;
				transform.audio.Play();
				Destroy(deadthing.gameObject,transform.audio.clip.length);
				//Debug.Log("JAG DOG AV EN LAMPA");
			}

			//audio.Play();
			else if(!deadthing.gameObject.name.Equals(transform.name)){
				//transform.audio.clip =  gameObject.GetComponent<AudioClips>()._audios[0];
				if(deadthing.gameObject.GetComponent<GroundType>().GetType() == 0){
					transform.audio.clip = _sounds._stoneSounds[Random.Range(0, _sounds._stoneSounds.Length)];
				}
				if(deadthing.gameObject.GetComponent<GroundType>().GetType() == 1){
					transform.audio.clip = _sounds._sandSounds[Random.Range(0, _sounds._sandSounds.Length)];
				}
				if(deadthing.gameObject.GetComponent<GroundType>().GetType() == 2){
					transform.audio.clip = _sounds._metalSounds[Random.Range(0, _sounds._metalSounds.Length)];
				}

				if(!transform.audio.isPlaying /*&& !deadthing.gameObject.name.Equals(_sandName) && _JUSTDOITONCEFFS*/){ //DEN HÄR FUNKAR INTE :(
					transform.audio.Play();
					//transform.audio.clip.length;
					//_JUSTDOITONCEFFS = false;
				}
				//Debug.Log("JAG DOG AV NÅGOT");
				Destroy (transform.gameObject, transform.audio.clip.length);
				//Debug.Log ("Kuben dog, gg");
			}
		}
	}
}

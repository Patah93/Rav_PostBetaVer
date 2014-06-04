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

		////Checks if object is thrown when it has collided with another object.
		if(_isThrown){

			////Checks if the object it collided with is destructable or if it has a lightsource.
			/// If true, it sets the audio, turns of light, plays audio and then destroys the object.
			if(deadthing.gameObject.tag == "Destructable" || deadthing.gameObject.light != null){
				transform.audio.clip = gameObject.GetComponent<AudioClips>()._audios[1];
				deadthing.gameObject.light.enabled = false;
				transform.audio.Play();
				Destroy(deadthing.gameObject,transform.audio.clip.length);
			}

			////Checks so the other object isn't the same as itself. If it isn't, it checks what kind of object it collided with and plays audio corresponding to the type.
			/// With type, is what kind of surface the other object has.
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

				////Plays audio if it's not playing
				if(!transform.audio.isPlaying){
					transform.audio.Play();

				}

				////Kills the object when the audioclip has finished playing
				Destroy (transform.gameObject, transform.audio.clip.length);

			}
		}
	}
}

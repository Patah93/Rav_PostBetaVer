using UnityEngine;
using System.Collections;

public class AmbientSounds : MonoBehaviour {

	public GameObject _audioSauce;

	public bool _createNewSauce;

	public AudioClip[] _clips;

	[Range(0,60)]
	public float _random, _minTime, _maxTime;

	float clock = 0;

	void Update () {
		clock += Time.deltaTime;
		if((clock > _minTime && Random.Range(0, _random) == 0) || clock > _maxTime){
			if(_createNewSauce || audio == null){
			GameObject sauce = (GameObject)Instantiate (_audioSauce, transform.position, Quaternion.identity);
			sauce.audio.clip = getRandomClip ();
			sauce.audio.Play ();
			} else {
				audio.PlayOneShot(getRandomClip ());
			}
			clock = 0;
		} 
	}

	AudioClip getRandomClip(){
		if (_clips.Length > 0)
			return _clips[Random.Range(0, _clips.Length)];
		return null;
	}

}

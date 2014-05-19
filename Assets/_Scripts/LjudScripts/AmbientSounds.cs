using UnityEngine;
using System.Collections;

public class AmbientSounds : MonoBehaviour {

	public GameObject _audioSauce;

	public AudioClip[] _clips;



	void Start () {
	
	}

	void Update () {
		GameObject sauce = (GameObject)Instantiate (_audioSauce, GameObject.Find ("L_foot_joint").transform.position, Quaternion.identity);
		sauce.audio.clip = getRandomClip ();
		sauce.audio.Play ();
	}

	AudioClip getRandomClip(){
		return null;
	}

}

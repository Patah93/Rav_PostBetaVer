using UnityEngine;
using System.Collections;

public class PlayRandomSoundOnTriggerMan : TriggerAction {
	
	public AudioClip[] _audioClips;

	private AudioSource _audioSource;

	void Start(){
		_audioSource = GetComponent<AudioSource> ();
	}
	
	private AudioClip getRandomClip(){
		return _audioClips[Random.Range(0,_audioClips.Length)];
	}
	
	public override void onActive(){
		_audioSource.clip = getRandomClip ();
		_audioSource.Play ();
	}
	
	public override void onInactive(){

	}
}
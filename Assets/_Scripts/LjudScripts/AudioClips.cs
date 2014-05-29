using UnityEngine;
using System.Collections;

public class AudioClips : MonoBehaviour {

	public AudioClip[]_audios;
	public AudioClip[] _stoneSounds;
	public AudioClip[] _sandSounds;
	public AudioClip[] _metalSounds;

	public AudioClip GetRandomClip(){
		return _audios[Random.Range(0, _audios.Length)];
	}

}

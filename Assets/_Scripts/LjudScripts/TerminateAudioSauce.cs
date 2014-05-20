using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class TerminateAudioSauce : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying)
						Destroy (this.gameObject);
	}
}

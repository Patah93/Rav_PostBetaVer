using UnityEngine;
using System.Collections;

public class TerminateAudioSauce : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying)
						Destroy (this.gameObject);
	}
}

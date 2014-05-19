using UnityEngine;
using System.Collections;

public class FadeSound : MonoBehaviour {

	public bool _activated;

	[Range(0f,1f)]
	public float _fadeSpeed;
	
	// Update is called once per frame
	void Update () {
	
		if(_activated)
			GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 1, Time.deltaTime * _fadeSpeed);
		else 
			GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0, Time.deltaTime * _fadeSpeed);
	
	}

	public void ChangeState(){
		_activated = !_activated;
	}

}

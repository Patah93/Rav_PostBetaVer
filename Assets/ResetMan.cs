using UnityEngine;
using System.Collections;

public class ResetMan : MonoBehaviour {

	FadeCamera _fade;

	bool _reset = false;

	void Awake(){

		_fade = GameObject.Find ("ScreenFade").GetComponent<FadeCamera>();

	}

	void Update(){

		if(Input.GetButtonDown("Reset") && !_reset){

			_reset = true;
			_fade.Fading(true);

		}
		if(_reset && _fade.FadeState()){
			Application.LoadLevel(0);
		}

	}

}

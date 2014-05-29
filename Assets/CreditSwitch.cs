using UnityEngine;
using System.Collections;

public class CreditSwitch : TriggerAction {

	FadeCamera _fade;

	bool _check = false;

	void Start(){

		_fade = GameObject.Find ("ScreenFade").GetComponent<FadeCamera> ();

	}

	void Update(){

		if(_check && _fade.FadeState()){

			Application.LoadLevel("Credits");

		}

	}

	public override void onActive(){
		
		_check = true;
		GetComponent<EndMusic> ().Deactivate ();
		
	}
	
	public override void onInactive(){

	}

}

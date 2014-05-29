using UnityEngine;
using System.Collections;

public class TriggerFade : TriggerAction {

	public Texture _fadeTex;

	public Color _fadeColor = Color.black;

	FadeCamera _fade;

	[Range(0,1)]
	public float _newFadeSpeed = 0.5f;

	public bool _reversable = false; 

	void Awake(){
		_fade = GameObject.Find ("ScreenFade").GetComponent<FadeCamera> ();
	}

	public override void onActive(){

		_fade.Fading (true);
		_fade.SetFadeSpeed (_newFadeSpeed);
		_fade.SetFadeColor (_fadeColor);
		_fade.SetFadeTexture (_fadeTex);
	
	}

	public override void onInactive(){
		if(_reversable)
			_fade.Fading (false);
	}
}

using UnityEngine;
using System.Collections;

public class FadeCamera : MonoBehaviour {

	public float _fadeSpeed = 1.0f;
	public bool[] _fadeStates;

	void Awake(){
		guiTexture.pixelInset = new Rect(-Screen.width/2f, -Screen.height/2f, Screen.width, Screen.height);
		_fadeStates = new bool[2];
		_fadeStates[0] = false;
		_fadeStates[1] = false;
	}

	// Update is called once per frame
	void Update () {
		if(!_fadeStates[0] && guiTexture.color.a != 0.0f)
			StartFadeIn();
		else if(_fadeStates[0] && guiTexture.color.a != 1.0f)
			StartFadeOut(); 
	}

	private void StartFadeIn(){

		guiTexture.enabled = true;
		FadeIn ();
		
		if(guiTexture.color.a < 0.05f){
			_fadeStates[1] = false;
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
		}
	}

	private void StartFadeOut(){

		guiTexture.enabled = true;
		FadeOut ();

		if(guiTexture.color.a > 0.95f){
			_fadeStates[1] = true;
			guiTexture.color = Color.black;
		}
	}
	
	private void FadeIn(){
		guiTexture.color = Color.Lerp (guiTexture.color, Color.clear, _fadeSpeed * Time.deltaTime);
	}
	
	private void FadeOut(){
		guiTexture.color = Color.Lerp (guiTexture.color, Color.black, _fadeSpeed * Time.deltaTime);
	}

	public void Fading(bool b){
		_fadeStates[0] = b;
	}

	public bool FadeState(){
		return _fadeStates[0] && _fadeStates[1];
	}
}

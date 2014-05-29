using UnityEngine;
using System.Collections;

public class FadeCamera : MonoBehaviour {

	public float _fadeSpeed = 1.0f;
	public bool[] _fadeStates;

	public float _fadeLimit;

	//public bool _fadeToWhite = false;

	Color _fadeToColor;

	Vector2 _screenSize;

	void Awake(){
		guiTexture.pixelInset = new Rect(-Screen.width/2f, -Screen.height/2f, Screen.width, Screen.height);
		_fadeStates = new bool[2];
		_fadeStates[0] = false;
		_fadeStates[1] = false;

		_screenSize = new Vector2(Screen.width, Screen.height);

	}

	void Update () {
		if(!_fadeStates[0] && guiTexture.color.a != 0.0f)
			StartFadeIn();
		else if(_fadeStates[0] && guiTexture.color.a != 1.0f)
			StartFadeOut(); 

		if(_screenSize != new Vector2(Screen.width, Screen.height)){

			_screenSize = new Vector2(Screen.width, Screen.height);
			guiTexture.pixelInset = new Rect(-Screen.width/2f, -Screen.height/2f, Screen.width, Screen.height);

		}

		//_fadeToColor = (_fadeToWhite) ? Color.white : Color.black;

	}

	private void StartFadeIn(){

		guiTexture.enabled = true;
		FadeIn ();
		
		if(guiTexture.color.a < 1-_fadeLimit){
			_fadeStates[1] = false;
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
		}
	}

	private void StartFadeOut(){

		guiTexture.enabled = true;
		FadeOut ();

		if(guiTexture.color.a > _fadeLimit){
			_fadeStates[1] = true;
			guiTexture.color = _fadeToColor;
		}
	}
	
	private void FadeIn(){
		guiTexture.color = Color.Lerp (guiTexture.color, Color.clear, _fadeSpeed * Time.deltaTime);
	}
	
	public void FadeOut(){
		guiTexture.color = Color.Lerp (guiTexture.color, _fadeToColor, _fadeSpeed * Time.deltaTime);
	}

	public void Fading(bool b){
		_fadeStates[0] = b;
	}

	public bool FadeState(){
		return _fadeStates[0] && _fadeStates[1];
	}

	public void SetFadeLimit(float f){
		_fadeLimit = f;
	}

	public void SetFadeSpeed(float f){
		_fadeSpeed = f;
	}

	public void SetFadeColor(Color c){

		_fadeToColor = c;

	}

	public void SetFadeTexture(Texture tex){

		GetComponent<GUITexture> ().texture = tex;

	}
}

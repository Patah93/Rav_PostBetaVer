using UnityEngine;
using System.Collections;

public class FadeCamera : MonoBehaviour {

	public float _fadeSpeed = 1.0f;
	public bool _fadeStart = true;

	void Awake(){
		guiTexture.pixelInset = new Rect(-Screen.width/2f, -Screen.height/2f, Screen.width, Screen.height);
	}

	// Update is called once per frame
	void Update () {
		if(_fadeStart && guiTexture.color.a != 0.0f)
			StartFadeIn();
		else if(!_fadeStart && guiTexture.color.a != 1.0f)
			StartFadeOut(); 
	}

	private void StartFadeIn(){

		guiTexture.enabled = true;
		FadeIn ();
		
		if(guiTexture.color.a < 0.05f){
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
		}
	}

	private void StartFadeOut(){

		guiTexture.enabled = true;
		FadeOut ();

		if(guiTexture.color.a > 0.95f){
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
		_fadeStart = b;
	}

	//*-*-*-*-*-*-*-*-*-*-*-*-*-* TO-DO *-*-*-*-*-*-*-*-*-*-*-*-*//
	//   Returnera state så man kan köra rollback i Checkpoint	 //
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*//

	public bool FadeState(){
		return true;
	}
}

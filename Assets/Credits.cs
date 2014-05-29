using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	public MovieTexture _creditsM;
	public Texture2D _creditsT;
	bool _drawMovie = true;
	// Use this for initialization
	void Start () {
	
		_creditsM.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (_drawMovie) {
			if (!_creditsM.isPlaying) {
					_drawMovie = false;
			}
		}
		else {
			if (Input.anyKeyDown) {
				Application.Quit ();
			}
		}
	}

	void OnGUI(){
		if (_drawMovie) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), _creditsM, ScaleMode.StretchToFill, false, 0);
		} 
		else {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), _creditsT, ScaleMode.StretchToFill, false, 0);
		}
	}
}

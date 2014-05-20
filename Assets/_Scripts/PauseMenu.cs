using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public Rect _pauseWindow = new Rect(7.5f,5.5f,1.5f,1.5f);
	public Texture2D _pauseWindowTexture;
	public Rect _exitButton = new Rect(4,2.5f,6,7);
	public Texture2D _exitButtonTexture;

	bool _paused;
	float _delay;

	// Use this for initialization
	void Start () {
		_paused = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Pause")){
			if(!_paused){
				_paused = true;
				Time.timeScale = 0;
			}
			else{
				_paused = false;
				Time.timeScale = 1;
			}
		}
	}

	Rect scaleRect(Rect r){
		return new Rect(Screen.width/r.x,Screen.height/r.y,Screen.width/r.width,Screen.height/r.height);
	}

	void OnGUI(){
		if(_paused){
			GUI.Window(0,scaleRect(_pauseWindow),PauseWindow,"Game Paused");
			GUI.DrawTexture(scaleRect(_pauseWindow),_pauseWindowTexture,ScaleMode.StretchToFill,false,0);
		}
	}

	void PauseWindow(int id){

		GUI.DrawTexture(scaleRect(_exitButton),_exitButtonTexture,ScaleMode.StretchToFill,false,0);
		if(GUI.Button(scaleRect(_exitButton),"")){
			Application.Quit();
		}
	}
}

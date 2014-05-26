using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public Rect _pauseWindow = new Rect(7.5f,5.5f,1.5f,1.5f);
	public Texture2D _pauseWindowTexture;
	public Rect _exitButton = new Rect(4,2.6f,6,7);
	public Texture2D _exitButtonTexture;
	public Rect _resumeButton = new Rect(4,13,6,7);
	public Texture2D _resumeButtonTexture;
	public Rect _settingsButton = new Rect(4,4.3f,6,7);
	public Texture2D _settingsButtonTexture;

	public Rect _volumeButtonPos = new Rect(4,10,6,7);
	public Texture2D _volumeButtonTexture;
	public Rect _volumeSliderPos = new Rect(4,4,6,7);

	public Rect _returnToOptionsButton = new Rect(4,2.5f,6,7);
	public Texture2D _returnToOptionsButtonTexture;

	bool _paused;
	bool _settings;

	// Use this for initialization
	void Start () {
		_paused = false;
		_settings = false;
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
				_settings = false;
				Time.timeScale = 1;
			}
		}
	}

	Rect scaleRect(Rect r){
		return new Rect(Screen.width/r.x,Screen.height/r.y,Screen.width/r.width,Screen.height/r.height);
	}

	void OnGUI(){
		if(_paused){
			if(_settings){
				GUI.Window(1,scaleRect(_pauseWindow),SettingsWindow,"Settings");
				GUI.DrawTexture(scaleRect(_pauseWindow),_settingsButtonTexture,ScaleMode.StretchToFill,false,0);
			}
			else{
				GUI.Window(0,scaleRect(_pauseWindow),PauseWindow,"Game Paused");
				GUI.DrawTexture(scaleRect(_pauseWindow),_pauseWindowTexture,ScaleMode.StretchToFill,false,0);
			}
		}
	}

	void PauseWindow(int id){

		GUI.DrawTexture(scaleRect(_exitButton),_exitButtonTexture,ScaleMode.StretchToFill,false,0);
		GUI.DrawTexture (scaleRect (_resumeButton),_resumeButtonTexture,ScaleMode.StretchToFill,false,0);
		GUI.DrawTexture(scaleRect(_settingsButton),_settingsButtonTexture,ScaleMode.StretchToFill,false,0);
		if(GUI.Button(scaleRect(_exitButton),"")){
			Application.Quit();
		}
		if(GUI.Button (scaleRect(_resumeButton),"")){
			_paused = false;
			Time.timeScale = 1;
		}
		if(GUI.Button (scaleRect(_settingsButton),"")){
			_settings = true;
		}
	}

	void SettingsWindow(int id){
		GUI.DrawTexture(scaleRect(_returnToOptionsButton),_returnToOptionsButtonTexture,ScaleMode.StretchToFill,false,0);
		GUI.DrawTexture(scaleRect (_volumeButtonPos),_volumeButtonTexture,ScaleMode.ScaleToFit,false,0);
		if(GUI.Button (scaleRect(_returnToOptionsButton),"")){
			_settings = false;
		}

		AudioListener.volume = GUI.HorizontalSlider(scaleRect(_volumeSliderPos),AudioListener.volume,0,1);
	}
}

using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public Vector2 _position = new Vector2(0,0);
	public Texture2D _startScreen;
	public Rect _buttonSizes = new Rect(3,1.5f,3.5f,5);
	public Texture2D _buttontexture;
	public string _firstSceneName;
	public Texture2D _loadingScreen;

	Texture2D _background;
	bool _drawButtons;
	bool _loadlevel;

	// Use this for initialization
	void Start () {
		_background = _startScreen;
		_drawButtons = true;
		_loadlevel = false;
		//_buttonSizes = new Rect(Screen.width/_buttonSizes.x,Screen.height/_buttonSizes.y,Screen.width/_buttonSizes.width,Screen.height/_buttonSizes.height);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		//GUILayout.BeginArea(new Rect(_position.x,_position.y,Screen.width,Screen.height));
		//GUI.Box((new Rect(_position.x,_position.y,Screen.width,Screen.height)),_background);
		GUI.DrawTexture(new Rect(_position.x,_position.y,Screen.width,Screen.height),_background,ScaleMode.StretchToFill,false,0);
		if(_drawButtons){
			GUI.DrawTexture(scaleRect(_buttonSizes),_buttontexture,ScaleMode.StretchToFill,false,0);
			if(GUI.Button(scaleRect(_buttonSizes),"")){
				_loadlevel = true;
			}
		}
		if(_loadlevel){
//			Color _col = _startScreen;
			Application.LoadLevelAsync(_firstSceneName);
			GUI.color = Color.Lerp(Color.white,Color.black,0.1f);
			_background = _loadingScreen;
			_drawButtons = false;
		}

		Debug.Log("Load level is " + _loadlevel);
	//	GUILayout.EndArea();
	}

	Rect scaleRect(Rect r){
		return new Rect(Screen.width/r.x,Screen.height/r.y,Screen.width/r.width,Screen.height/r.height);
	}
}

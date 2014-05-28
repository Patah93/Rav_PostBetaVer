using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public Vector2 _position = new Vector2(0,0);
	public Texture2D _startScreen;
	public Rect _buttonSizes = new Rect(3,1.5f,3.5f,5);
	public Material _buttontexture;
	public int _nrOfSheetImages;
	public string _firstSceneName;
	public Texture2D _loadingScreen;
	public float _fadeSpeed = 0.05f;
	public float _nrOfRows;
	public float _nrOfColumns;
	public float _frameTime;
	public float _screenTime;

	public Rect _loadPos = new Rect(3,4,5,6);

	Texture2D _background;
	bool _drawButtons;
	bool _loadlevel;
	bool _fadeFrom;
	bool _fadeTo;
	bool _disableClick;
	bool _startScene;
	bool _yield = false;
	bool _waitBro = true;
	//FadeCamera _fade;
	Texture2D _fadeScreen;
	AsyncOperation _load;
	TextureAnimation _ani;
	public GameObject[] _loadingObjects;

	// Use this for initialization
	void Start () {
		
		_startScene = false;
		_disableClick = false;
		_fadeFrom = false;
		_fadeTo = false;
		_fadeScreen = new Texture2D(1,1);
		_fadeScreen.SetPixel(1,1,Color.clear);
		_fadeScreen.Apply();
		_background = _startScreen;
		_drawButtons = true;
		_loadlevel = false;
		_ani = gameObject.GetComponent<TextureAnimation>();
		_ani.setSheet (_buttontexture, _nrOfColumns, _nrOfRows, _frameTime);
		foreach (GameObject i in _loadingObjects) {
			TextureAnimation _temp = i.gameObject.GetComponent<TextureAnimation> ();
			_temp.setSheet (_temp._sheet, _temp._nrOfColumns, _temp._nrOfRows, _temp._frameTime);
		}

		//_buttonSizes = new Rect(Screen.width/_buttonSizes.x,Screen.height/_buttonSizes.y,Screen.width/_buttonSizes.width,Screen.height/_buttonSizes.height);
	}
	
	// Update is called once per frame
	void Update () {
		//_buttontexture = _ani.getSheet;
	}

	IEnumerator wait(){

		Debug.Log("waiting");
		yield return new WaitForSeconds (_screenTime);
		_load.allowSceneActivation = true;

	//	_load.
		//Time.timeScale = 0;
			//yield WaitForSeconds((1/(Time.Time*_fadeSpeed)));
	}

	void OnGUI(){
		GUI.DrawTexture(new Rect(_position.x,_position.y,Screen.width,Screen.height),_background,ScaleMode.StretchToFill,false,0);
		if(_drawButtons){
			//GUI.DrawTexture(scaleRect(_buttonSizes),_buttontexture,ScaleMode.StretchToFill,false,0);
			GUI.DrawTextureWithTexCoords(scaleRect(_buttonSizes),_buttontexture.mainTexture,_ani.horunge());
			if(!_disableClick){
				if(GUI.Button(scaleRect(_buttonSizes),"")){
					_loadlevel = true;
					_fadeTo = true;
					_disableClick = true;
					//_fade.FadeOut();

				}
			}
		} 
		if(_loadlevel){
//			Color _col = _startScreen;
			//guiTexture.color = Color.Lerp
			if(_fadeTo){

				_fadeScreen.SetPixel(1,1,Color.Lerp((_fadeScreen.GetPixel(1,1)),Color.black,Time.time*_fadeSpeed));
				_fadeScreen.Apply();
				if(_fadeScreen.GetPixel(1,1).a >= 0.92f){
				//GUI.color = Color.Lerp(Color.white,Color.blue,0.1f);
					_background = _loadingScreen;
				//	_fadeScreen.SetPixel(1,1,Color.clear);
					//_fadeScreen.Apply();
					_fadeTo = false;
					_fadeFrom = true;
					Debug.Log("I'm here");
				}
			}
			else if(_fadeFrom){
				Debug.Log ("Fading from");
				_drawButtons = false;
				_fadeScreen.SetPixel(1,1,Color.Lerp((_fadeScreen.GetPixel(1,1)),Color.clear,Time.time*_fadeSpeed));
				_fadeScreen.Apply();
				if(_fadeScreen.GetPixel(1,1).a <= 0.1f){
					_fadeFrom = false;
					_startScene = true;
					_load = Application.LoadLevelAsync(_firstSceneName);
				}
			}
			if(_startScene){

			//	WWW ingenaning = new WWW(Application.LoadLevelAsync(_firstSceneName))
				
				foreach(GameObject i in _loadingObjects){
					TextureAnimation _temp = i.gameObject.GetComponent<TextureAnimation>();
				//	_temp.setSheet(_temp._sheet,_temp._nrOfColumns,_temp._nrOfRows,_temp._frameTime);
					GUI.DrawTextureWithTexCoords(scaleRect(_loadPos),_temp._sheet.mainTexture,_temp.horunge());
					//Debug.Log(_temp._clock);
			}
				if(_load.progress > 0.8){

					if(!_yield){
						_load.allowSceneActivation = false;
						StartCoroutine(wait ());
						_yield = true;
					}

					Debug.Log("ey");
					_fadeScreen.SetPixel(1,1,Color.Lerp((_fadeScreen.GetPixel(1,1)),Color.black,Time.time*_fadeSpeed));
					_fadeScreen.Apply();

				//	if(_fadeScreen.GetPixel(1,1).a <= 0.97f){
						Debug.Log("if-sats");
						
					//	_yield = true;
						Debug.Log("hallå");
					//	_yield = true;
				//	}
				}
				Debug.Log(_load.progress);
			}
			//_background = Color.Lerp(Color.white,Color.black,Time.deltaTime);
		}

		//Debug.Log("Load level is " + _loadlevel);

		GUI.DrawTexture(new Rect(_position.x,_position.y,Screen.width,Screen.height),_fadeScreen);
	//	GUILayout.EndArea();
	}

	 
	Rect scaleRect(Rect r){
		return new Rect(Screen.width/r.x,Screen.height/r.y,Screen.width/r.width,Screen.height/r.height);
	}
}

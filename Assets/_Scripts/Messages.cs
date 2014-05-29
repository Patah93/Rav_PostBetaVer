using UnityEngine;
using System.Collections;

public class Messages : TriggerAction {

	public Rect _messageBox = new Rect(10,15,1.2f,5.5f);
	public Texture2D _boxTexture; 
	public string _message;
	public int _fontSize = 24;
	public bool _display = false;
	public bool _disableWithKey = false;
	public string _disableKey;
	public Color _fontColor = new Color(0,0,0,255);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_disableWithKey) {
			if(Input.GetButtonDown(_disableKey)){
				_display = false;
				Destroy(this);
			}
		}
	}

	public override void onActive(){
		_display = true;
	}
	
	public override void onInactive(){
		if (!_disableWithKey) {
			_display = false;
		}
	}

	void OnGUI(){
		if (_display) {
	
			GUIStyle tempStyle =  new GUIStyle(GUI.skin.box);
			tempStyle.normal.background = _boxTexture;
			tempStyle.fontSize = Screen.width/_fontSize;
			tempStyle.alignment = TextAnchor.MiddleCenter;
			tempStyle.wordWrap = true;
			tempStyle.normal.textColor = _fontColor;
			GUI.Box(scaleRect(_messageBox),_message, tempStyle);
		}
	}

	Rect scaleRect(Rect r){
		return new Rect(Screen.width/r.x,Screen.height/r.y,Screen.width/r.width,Screen.height/r.height);
	}
}

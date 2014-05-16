using UnityEngine;
using System.Collections;

public class ScreenSwitch : MonoBehaviour {

	public Material _material;
	public float _screenTime = 2;
	public float _numberOfScreens = 2;
	float _timer; 
	int _currentScreen = 0;

	// Use this for initialization
	void Start () {
		_timer = Time.time;
		_material.SetTextureScale("_MainTex",new Vector2(1/_numberOfScreens,1));
		_material.SetTextureOffset("_MainTex", Vector2.zero);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - _timer > _screenTime){
			_currentScreen += 1;
			if(_currentScreen>=_numberOfScreens){
				_currentScreen = 0;
			}
			_material.SetTextureOffset("_MainTex",new Vector2(_currentScreen*1/_numberOfScreens,0));
			_timer = Time.time;
		}
	}
}

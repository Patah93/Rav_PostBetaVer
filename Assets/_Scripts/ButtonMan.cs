using UnityEngine;
using System.Collections;

public class ButtonMan : MonoBehaviour {

//	Button _button;
	bool _pressedMode = false;

	// Use this for initialization
	void Start () {
		//_button = gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ButtonPressed(){
		_pressedMode = true;
		//c.GetComponent<Button>().setPressed(true);
		//_button.setPressed(true);
		Debug.Log("This happened");
	}

	public bool isPressed(){
		Debug.Log ("isPressed is " + _pressedMode);
		return _pressedMode;
	}

	public void setPressed(bool ispressed){
		_pressedMode = ispressed;
	}
}

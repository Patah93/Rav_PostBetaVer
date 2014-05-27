using UnityEngine;
using System.Collections;

public class CollapseMan : TriggerAction {

	public bool _triggered = false;
	public float _countDown = 0.1f;

	public GameObject[] _fallingThings;

	// Use this for initialization
	void Start () {
		GameObject[] _tempFall = _fallingThings;
		for(int i = 0; i < _tempFall.Length; i++){
			_tempFall[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(_triggered && _countDown > 0.0f){
			_countDown -= Time.deltaTime;
		}

		if(_countDown <= 0.0f){
			GameObject[] _tempFall = _fallingThings;
			for(int i = 0; i < _tempFall.Length; i++){
				_tempFall[i].rigidbody.constraints = RigidbodyConstraints.None;
				_tempFall[i].rigidbody.useGravity = true;
			}
		}

	}

	public override void onActive(){
		_triggered = true;
		
	}
	
	public override void onInactive(){
		_triggered = true;
	}
}

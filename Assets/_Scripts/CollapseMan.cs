using UnityEngine;
using System.Collections;

public class CollapseMan : TriggerAction {

	public bool _triggered = false;
	public float _countDown = 0.1f;

	public GameObject[] _fallingThings;

	// Use this for initialization
	void Start () {

		////Puts all objects into an array contrains everything on their rigidbody
		GameObject[] _tempFall = _fallingThings;
		for(int i = 0; i < _tempFall.Length; i++){
			_tempFall[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
	}
	
	// Update is called once per frame
	void Update () {

		////Counts down when _triggered is true and _countDown is more than 0.0f
		if(_triggered && _countDown > 0.0f){
			_countDown -= Time.deltaTime;
		}

		////Turns on gravity and takes away restrictons when _countDown is 0.0f or less.
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

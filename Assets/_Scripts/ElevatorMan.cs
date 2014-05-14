using UnityEngine;
using System.Collections;

public class ElevatorMan : TriggerAction {
	
	public GameObject _pos1;
	public GameObject _pos2;

	[Range(0, 1)]
	public float _moveSpeed = 0.2f;

	public bool _triggered;
	bool _goingUp;
	float _offSet;
	public float _countMax;
	float _counter;


	// Use this for initialization
	void Start () {
		_triggered = true;
		_goingUp = true;
		_offSet = 0.01f;
	}
	
	// Update is called once per frame
	void Update () {

		_counter += Time.deltaTime;

		if(_triggered){
			if(_goingUp && _counter >= _countMax){

				Vector3 targetMove = Vector3.Lerp(transform.position, _pos2.transform.position, _moveSpeed);
				transform.position = targetMove;

				if((transform.position - _pos2.transform.position).magnitude <= _offSet){
					transform.position = _pos2.transform.position;
					_goingUp = !_goingUp;
					_counter = 0;
				}
			}


			else if(_counter >= _countMax){

				Vector3 targetMove = Vector3.Lerp(transform.position, _pos1.transform.position, _moveSpeed);
				transform.position = targetMove;

				if((transform.position - _pos1.transform.position).magnitude <= _offSet){
					transform.position =_pos1.transform.position;
					_goingUp = !_goingUp;
					_counter = 0;
				}
			}

		}
	}
	
	public override void onActive(){

		Debug.Log("ACTIVE?!");

		_triggered = true;
		if(gameObject.CompareTag("Player")){
			gameObject.transform.parent = transform;
		}
	}
	
	public override void onInactive(){
		_triggered = false;
	}


}

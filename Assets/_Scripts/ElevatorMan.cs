using UnityEngine;
using System.Collections;

public class ElevatorMan : TriggerAction {
	
	public GameObject _pos1;
	public GameObject _pos2;

	[Range(0, 20)]
	public float _moveSpeedUp = 3.0f;
	[Range(0, 20)]
	public float _moveSpeedDown = 3.0f;

	public bool _triggered;
	bool _goingUp;
	float _offSet;
	public float _countMax;
	float _counter;

	float _clock;


	// Use this for initialization
	void Start () {
		_triggered = false;
		_goingUp = true;
		_offSet = 0.01f;
		_clock = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		_counter += Time.deltaTime;

		if(_triggered){
			if(_goingUp && _counter >= _countMax){

				//Vector3 targetMove = Vector3.Lerp(transform.position, _pos2.transform.position, _moveSpeed);
				Vector3 targetMove = Vector3.Lerp(_pos1.transform.position, _pos2.transform.position, (Time.time - _clock) * _moveSpeedDown);
				transform.position = targetMove;

				if((transform.position - _pos2.transform.position).magnitude <= _offSet){
					transform.position = _pos2.transform.position;
					_goingUp = !_goingUp;
					_clock = Time.time;
					_counter = 0;
				}
			}


			else if(_counter >= _countMax){

				//Vector3 targetMove = Vector3.Lerp(transform.position, _pos1.transform.position, _moveSpeed);
				Vector3 targetMove = Vector3.Lerp(_pos2.transform.position, _pos1.transform.position, (Time.time - _clock) * _moveSpeedUp);
				transform.position = targetMove;

				if((transform.position - _pos1.transform.position).magnitude <= _offSet){
					transform.position =_pos1.transform.position;
					_goingUp = !_goingUp;
					_clock = Time.time;
					_counter = 0;
				}
			}
			else{
				_clock = Time.time;
			}
		}
	}
	
	public override void onActive(){

		//Debug.Log("ACTIVE?!");

		_triggered = true;
		_clock = Time.time;
		if(gameObject.CompareTag("Player")){
			gameObject.transform.parent = transform;
		}
	}
	
	public override void onInactive(){
		_triggered = false;
		_clock = Time.time;
	}


}

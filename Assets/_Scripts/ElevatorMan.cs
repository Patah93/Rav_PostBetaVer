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

		///Counts up
		_counter += Time.deltaTime;


		////Active when trigger is true. Moves an object between two positions and waits at each position for n-seconds
		if(_triggered){
			////If it's going up and counter is max or more, then it's going to run
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


	//What happens when on the trigger area. Here the trigger turns true and the clock is set. The player also moves along
	public override void onActive(){

		////Debug.Log("ACTIVE?!");

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

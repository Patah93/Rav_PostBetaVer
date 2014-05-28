using UnityEngine;
using System.Collections;

public class MoveOnceOnTrigger : TriggerAction {
	
	Vector3 _originalPos;
	
	public Vector3 _offset = new Vector3(0, 5, 0);
	
	public float _moveTime = 0.02f;

	private float clock;
	
	bool _isMoving = false;
	
	// Use this for initialization
	void Start () {
		_originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(_isMoving){
			transform.position = Vector3.Lerp(transform.position, _offset + _originalPos, (Time.time - clock) * _moveTime);
		}
		if ((transform.position - (_offset + _originalPos)).sqrMagnitude < 0.1f) {
			_isMoving = false;
			transform.position = _offset + _originalPos;
		}
		
	}
	
	public override void onActive(){
		//_originalPos = transform.position;
		_isMoving = true;
		clock = Time.time;
	}
	
	public override void onInactive(){
	}
}

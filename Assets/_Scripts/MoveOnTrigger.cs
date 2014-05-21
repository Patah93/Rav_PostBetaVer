using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class MoveOnTrigger : TriggerAction {

	Vector3 _originalPos;

	public Vector3 _offset = new Vector3(0, 5, 0);
	public float _moveTime = 0.02f;
	public Vector3 _rotateAngles;
	public float _rotateSpeed;
	public bool _isMoving = false;

	private Quaternion _startRotation;

	bool _audioPlaying = false;

	// Use this for initialization
	void Start () {
		_originalPos = transform.position;
		_startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		if(_isMoving){
			transform.rotation = Quaternion.Slerp(transform.rotation, (_startRotation * Quaternion.Euler(_rotateAngles)), _rotateSpeed);
			transform.position = Vector3.Lerp(gameObject.transform.position, _offset + _originalPos, _moveTime);
			if(Mathf.Abs ((transform.position - (_offset + _originalPos)).magnitude) > 0.03 || Quaternion.Angle(transform.rotation, _startRotation) > 2){
				if(!transform.audio.isPlaying){
					transform.audio.Play();
				}
			}
			else if(transform.audio.isPlaying){
				transform.audio.Stop ();
			}
		}
		else{
			transform.rotation = Quaternion.Slerp(transform.rotation, _startRotation, _rotateSpeed);
			transform.position = Vector3.Lerp(gameObject.transform.position, _originalPos, _moveTime);

			/* TODO Ljud för rotation här... FIND THE DERP CHALLNGE ;) */
			if(Mathf.Abs ((transform.position -  _originalPos).magnitude) > 0.03  || Quaternion.Angle(transform.rotation, _startRotation) > 2){
				if(!transform.audio.isPlaying){
					transform.audio.Play();
				}
			}
			else if(transform.audio.isPlaying){
				transform.audio.Stop ();
			}
		}
	
	}

	public override void onActive(){
		_isMoving = true;

	}

	public override void onInactive(){
		_isMoving = false;
	}
}

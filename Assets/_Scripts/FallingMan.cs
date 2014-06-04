﻿using UnityEngine;
using System.Collections;

public class FallingMan : MonoBehaviour {

	[Range (0.01f, 2.0f)]
	public float _length = 0.15f;

	Animator _ani;
	private Vector3 _fallVelocity;
	CharacterController _controller;

	// Use this for initialization
	void Start () {
		_ani = gameObject.GetComponent<Animator> ();
		_controller = gameObject.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {

		////Sets the objects velocity in x- and z-axis to use when falling down, to simulate gravity without using rootmotion from the animations.

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Run")){
			_fallVelocity.x = _controller.velocity.x;
			_fallVelocity.z = _controller.velocity.z;
		
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Jump")){
			_controller.Move(_fallVelocity*_length*Time.deltaTime);	
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Fallin'")){
		//	////Debug.Log ("FallVelocity: " + _fallVelocity);
			_controller.Move(_fallVelocity*_length*Time.deltaTime);
			 
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle Jump") && _ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25){
			_fallVelocity.x = _controller.velocity.x;
			_fallVelocity.z = _controller.velocity.z;	
		}
	

	}
}

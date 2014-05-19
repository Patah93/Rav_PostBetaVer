using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKFeetMan : MonoBehaviour {

	Animator _anim;

	bool[] _ikFeet;

	RaycastHit[] _outHit;

	Vector3[] _targetPosition, _targetForward;

	Quaternion[] _targetRotation;

	Transform _playerTransform, _leftFoot, _rightFoot;

	[Range(0,1)]
	public float _rayOffset, _scale, _scaleOffset, _lerpAmount, _maxtime;

	float _leftWeight, _rightWeight, _clock;

	// Use this for initialization
	void Start () {

		_anim = GetComponent<Animator>();

		_ikFeet = new bool[2];
		_outHit = new RaycastHit[2];
		_targetPosition = new Vector3[2];

		_targetForward = new Vector3[2];
		_targetRotation = new Quaternion[2];

		_playerTransform = transform;
		_leftFoot = GameObject.Find ("L_foot_joint").transform;
		_rightFoot = GameObject.Find ("R_foot_joint").transform;
	}

	void OnAnimatorIK(int layerIndex){

		if(_anim){

			if(Physics.Raycast(_leftFoot.position + Vector3.up *_scaleOffset, Vector3.down, out _outHit[0], 0.5f) &&
			   Physics.Raycast(_leftFoot.position + _playerTransform.forward * _rayOffset + Vector3.up *_scaleOffset, Vector3.down, out _outHit[1], 1f) &&
			   (_ikFeet[0] || Time.time - _clock > _maxtime)){	
				
				_targetForward[0] = _outHit[1].point - _outHit[0].point;
				_targetRotation[0].SetLookRotation(_targetForward[0], _outHit[0].normal+ _outHit[1].normal);
				_targetPosition[0] = _outHit[0].point + Vector3.up * _scale;
				
				_leftWeight = (_leftWeight > 0.9f) ? 1.0f : Mathf.Lerp(_leftWeight, 1, Time.deltaTime * _lerpAmount);
				
			} else {_leftWeight = Mathf.Lerp(_leftWeight, 0, Time.deltaTime * _lerpAmount);}
			if(Physics.Raycast(_rightFoot.position + Vector3.up * _scaleOffset, Vector3.down, out _outHit[0], 0.5f) &&
			   Physics.Raycast(_rightFoot.position + _playerTransform.forward * _rayOffset + Vector3.up * _scaleOffset, Vector3.down, out _outHit[1], 1f) &&
			   (_ikFeet[1] || Time.time - _clock > _maxtime)){
				
				_targetForward[1] = _outHit[1].point - _outHit[0].point;
				_targetRotation[1].SetLookRotation(_targetForward[1], _outHit[0].normal+ _outHit[1].normal);
				_targetPosition[1] = _outHit[0].point + Vector3.up * _scale;
				
				_rightWeight = (_rightWeight > 0.9f) ? 1.0f : Mathf.Lerp(_rightWeight, 1, Time.deltaTime * _lerpAmount);
			} else {_rightWeight = Mathf.Lerp(_rightWeight, 0, Time.deltaTime * _lerpAmount);}

				_anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _leftWeight);
				_anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _leftWeight);

				_anim.SetIKPosition(AvatarIKGoal.LeftFoot, _targetPosition[0]);
				_anim.SetIKRotation(AvatarIKGoal.LeftFoot, _targetRotation[0]);

				_anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, _rightWeight);
				_anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, _rightWeight);
				
				_anim.SetIKPosition(AvatarIKGoal.RightFoot, _targetPosition[1]);
				_anim.SetIKRotation(AvatarIKGoal.RightFoot, _targetRotation[1]);

		}

	}

	void LeftFootIK(string s){
		_ikFeet[0] = (s.Equals("True")) ? true : false;
		_clock = Time.time;
	}

	void RightFootIK(string s){
		_ikFeet[1] = (s.Equals("True")) ? true : false;
		_clock = Time.time;
	}

}

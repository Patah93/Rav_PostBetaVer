using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKHeadMan : MonoBehaviour {

	public GameObject _lookAt;

	RaycastHit _outHit;

	Vector3 _targetForward;

	Quaternion _targetRotation;

	Animator _anim;

	float _weight;

	[Range(0,50)]
	public float _maxDistance;

	[Range(0,1)]
	public float _lerpAmount;

	void Start(){

		_anim = GetComponent<Animator>();

	}

	void OnAnimatorIK(int layerIndex){

		Vector3 distance = _lookAt.transform.position - transform.position;

		_weight = (distance.magnitude < _maxDistance) ? Mathf.Lerp(_weight, 1, _lerpAmount * Time.deltaTime) : Mathf.Lerp(_weight, 0, _lerpAmount * Time.deltaTime);
		if(distance.magnitude < _maxDistance && Physics.Raycast(transform.position, _lookAt.transform.position, out _outHit)){
			_anim.SetLookAtWeight(_weight);
			_anim.SetLookAtPosition(_lookAt.transform.position);
		}
	}


}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKHeadMan : MonoBehaviour {

	public GameObject _lookAt;

	RaycastHit _outHit;

	Vector3 _targetForward;

	Quaternion _targetRotation;

	Animator _anim;

	void Start(){

		_anim = GetComponent<Animator>();

	}

	void OnAnimatorIK(int layerIndex){

		_targetForward = _lookAt.transform.position - transform.position;
		_targetRotation.SetLookRotation(_targetForward);

		_anim.SetLookAtWeight(1);
		_anim.SetLookAtPosition(_lookAt.transform.position);

	}


}

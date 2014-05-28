using UnityEngine;
using System.Collections;

public class SyncAnimators : MonoBehaviour {

	Animator _anim;

	Animator _targetAnim;
	
	void Start () {

		_anim = GameObject.FindWithTag("Player").GetComponent<Animator>();

		_targetAnim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		
		_targetAnim.SetFloat("Speed", _anim.GetFloat("Speed"));
		_targetAnim.SetBool("Pushing", _anim.GetBool("Pushing"));
		_targetAnim.SetBool("Jump", _anim.GetBool("Jump"));
		_targetAnim.SetBool("Falling", _anim.GetBool("Falling"));
		_targetAnim.SetBool("ThrowMode", _anim.GetBool("ThrowMode"));
		_targetAnim.SetBool("Throw", _anim.GetBool("Throw"));
		_targetAnim.SetBool("PushButton", _anim.GetBool("PushButton"));
		
	}
}

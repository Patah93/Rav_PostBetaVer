using UnityEngine;
using System.Collections;

public class CollapseMan : MonoBehaviour {

	public bool _boyTrigger = false;
	public bool _foxTrigger = false;
	public float _countDown = 5.0f;

	public GameObject[] _fallingThings;

	// Use this for initialization
	void Start () {
		GameObject[] _tempFall = _fallingThings;
		for(int i = 0; i < _tempFall.Length; i++){
			_tempFall[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(_boyTrigger && _foxTrigger && _countDown > 0.0f){
			_countDown -= Time.deltaTime;
		}

		if(_countDown <= 0.0f){
			GameObject[] _tempFall = _fallingThings;
			for(int i = 0; i < _tempFall.Length; i++){
				_tempFall[i].rigidbody.constraints = RigidbodyConstraints.None;
				_tempFall[i].rigidbody.useGravity = true;
			}
		}

	}

	void OnTriggerEnter(Collider thingy){
		if(thingy.collider.CompareTag("Player")){
			_boyTrigger = true;
		}

		else if(thingy.collider.CompareTag("Fox")){
			_foxTrigger = true;
		}
	}
}

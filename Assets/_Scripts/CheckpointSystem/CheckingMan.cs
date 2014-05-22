using UnityEngine;
using System.Collections;

public class CheckingMan : MonoBehaviour {

	public CheckpointSystem _CheckpointSystem;

	public GameObject _currentCheckpoint;

	JumpingMan _jumpM;

	void Start(){

		_jumpM = GetComponent<JumpingMan>();

	}

	void Update(){

		if(_jumpM._dead)
			Debug.Log("Test");

	}

	void OnTriggerEnter(Collider c){

		if(c.CompareTag("Checkpoint")){

			if(_currentCheckpoint != null && _currentCheckpoint != c.gameObject){
				_currentCheckpoint = _CheckpointSystem.CompareCheckpoints(_currentCheckpoint, c.gameObject);
			}
			else 
				_currentCheckpoint = c.gameObject;

		}

	}

}

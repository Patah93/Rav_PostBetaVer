using UnityEngine;
using System.Collections;

public class CheckingMan : MonoBehaviour {

	public CheckpointSystem _CheckpointSystem;

	public GameObject _currentCheckpoint;

	Checkpoint _checkpointScript;

	JumpingMan _jumpM;

	bool _fading = false;

	void Start(){

		_jumpM = GetComponent<JumpingMan>();

	}

	void Update(){

		if(_checkpointScript != null){

			if(_jumpM._dead && !_fading){

					_checkpointScript.StartRollback(true);
					_fading = true;

			} else if(!_jumpM._dead && _fading) {

				_checkpointScript.StartRollback(false);
				_fading = false;

			}

		}

	}

	void OnTriggerEnter(Collider c){

		if(c.CompareTag("Checkpoint") && _currentCheckpoint != c.gameObject){

			if(_currentCheckpoint != null){
				_currentCheckpoint = _CheckpointSystem.CompareCheckpoints(_currentCheckpoint, c.gameObject);
			}
			else {
				_currentCheckpoint = c.gameObject;
			}

			_checkpointScript = _currentCheckpoint.GetComponent<Checkpoint>();
		}

	}

}

using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public FadeCamera ScreenFade;
	public GameObject _boyRespawnPosition;
	public GameObject[] _checkpointObjects;
	public Vector3[] _objectPositions;
	GameObject _player;
	bool _used = false;
	bool _rollback = false;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag ("Player");
		_objectPositions = new Vector3[_checkpointObjects.Length];
		for(int i = 0; i < _checkpointObjects.Length; i++){
			_objectPositions[i] = _checkpointObjects[i].transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(_rollback && ScreenFade.GetComponent<FadeCamera>().FadeState())
			Rollback();
	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.CompareTag("Player") && !_used){
			_used = true;
			for(int i = 0; i < _checkpointObjects.Length; i++){
				_objectPositions[i] = _checkpointObjects[i].transform.position;
			}
		}
	}

	void Rollback(){
		for(int i = 0; i < _checkpointObjects.Length; i++){
			_checkpointObjects[i].transform.position = _objectPositions[i];
		}

		_player.transform.position = (_boyRespawnPosition != null) ? _boyRespawnPosition.transform.position : transform.position;
		_player.GetComponent<JumpingMan>()._dead = false;

		RaycastHit hit;
		Physics.Raycast (transform.position, Vector3.down, out hit);
		_player.transform.position = hit.point;
	}

	public void StartRollback(bool b){
		_rollback = b;
		if(_rollback){
			ScreenFade.Fading(true);
		} else if(!_rollback){
			ScreenFade.Fading(false);
		}
	}

}

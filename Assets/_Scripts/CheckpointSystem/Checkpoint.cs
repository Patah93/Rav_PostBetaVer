using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public GameObject[] _checkpointObjects;
	public Vector3[] _objectPositions;
	bool _used = false;

	// Use this for initialization
	void Start () {
		_objectPositions = new Vector3[_checkpointObjects.Length];
		for(int i = 0; i < _checkpointObjects.Length; i++){
			_objectPositions[i] = _checkpointObjects[i].transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.CompareTag("Player") && !_used){
			_used = true;
			for(int i = 0; i < _checkpointObjects.Length; i++){
				_objectPositions[i] = _checkpointObjects[i].transform.position;
			}
		}
	}

	public void Rollback(){
		for(int i = 0; i < _checkpointObjects.Length; i++){
			_checkpointObjects[i].transform.position = _objectPositions[i];
		}
	}
}

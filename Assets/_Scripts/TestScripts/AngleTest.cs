using UnityEngine;
using System.Collections;

public class AngleTest : MonoBehaviour {

	Transform _playerTransform;
	RaycastHit[] _outHit;
	[Range(0,1)]
	public float _offset;


	// Use this for initialization
	void Start () {
		_playerTransform = GetComponent<Transform>();
		_outHit = new RaycastHit[2];
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(Physics.Raycast(_playerTransform.position, Vector3.down, out _outHit[0], 1.0f) && Physics.Raycast(_playerTransform.position + _playerTransform.forward * _offset, Vector3.down, out _outHit[1], 1.0f)){

			Debug.DrawRay(_outHit[0].point, Vector3.up, Color.blue);
			Debug.DrawRay(_outHit[1].point, Vector3.up, Color.red);
			Debug.DrawLine(_outHit[0].point + Vector3.up, _outHit[1].point + Vector3.up, Color.green);

			Vector3 _targetForward = _outHit[1].point - _outHit[0].point;

			Debug.Log(_targetForward.normalized);
			

		}
	}
}

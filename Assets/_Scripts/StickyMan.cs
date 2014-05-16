using UnityEngine;
using System.Collections;

public class StickyMan : MonoBehaviour {
		
	bool _OnPlatform;
	Collider _Thing;
	private Vector3 tempPlat = Vector3.zero;
	BoxCollider[] _boxyRay;

	public float _boxHeight = 3.0f;

	// Use this for initialization
	void Start () {
		_OnPlatform = false;
		gameObject.AddComponent<BoxCollider> ().isTrigger = true;
		_boxyRay = gameObject.GetComponents<BoxCollider>();
		_boxyRay [1].size = new Vector3 (1, 5, 1);
		_boxyRay [1].center = new Vector3(0, 3, 0);

	}
	
	// Update is called once per frame
	void Update () {

		if(_OnPlatform){
			_Thing.transform.position += (transform.position - tempPlat);


		}

		tempPlat = transform.position;

	}
	
	void OnTriggerEnter(Collider other) {
		_OnPlatform = true;
		_Thing = other;

	}

	void OnTriggerExit(Collider other) {
		_OnPlatform = false;

	}

}

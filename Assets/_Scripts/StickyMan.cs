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

		////If something is on the platform/trigger area, then move the thing with the same speed as the platform/trigger area.
		if(_OnPlatform){
			_Thing.transform.position += (transform.position - tempPlat);
		}

		////Sets a temp position of the current position of the platform/trigger area
		tempPlat = transform.position;

	}

	////When something is in the trigger area, set _OnPlatform to true and set _Thing as the object that's on.
	void OnTriggerEnter(Collider other) {
		_OnPlatform = true;
		_Thing = other;

	}

	void OnTriggerExit(Collider other) {
		_OnPlatform = false;

	}

}

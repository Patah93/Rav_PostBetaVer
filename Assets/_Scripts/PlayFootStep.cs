using UnityEngine;
using System.Collections;

public class PlayFootStep : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Footstep(){
		RaycastHit _rayHit;
		Physics.SphereCast (transform.position + new Vector3 (0, 1, 0), 0.3f, Vector3.down, out _rayHit, 1.1f);
		audio.PlayOneShot (GetComponent<FootSteps>().getRandomClip(0));
	}

}

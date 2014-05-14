using UnityEngine;
using System.Collections;

public class PlayFootStep : MonoBehaviour {

	public GameObject _audio;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Footstep(int i){
		Debug.Log ("Hello! Fot, fotsteg, fot fot, fotsteg steg: " + i);
		RaycastHit _rayHit;
		Physics.SphereCast (transform.position + new Vector3 (0, 1, 0), 0.3f, Vector3.down, out _rayHit, 1.1f);
		GameObject sauce = (GameObject)Instantiate (_audio, GameObject.Find ("L_foot_joint").transform.position, Quaternion.identity);
		
		sauce.audio.clip = GetComponent<FootSteps> ().getRandomClip (0);
		sauce.audio.Play ();
	}

}

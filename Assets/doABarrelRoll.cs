using UnityEngine;
using System.Collections;


public class doABarrelRoll : MonoBehaviour {
	public Rigidbody herp; 
	[Range(0,100)]
	public float x;

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		herp.AddForce (x, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class CheckingMan : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){

		Debug.Log(c.tag);

		if(c.CompareTag("Checkpoint")){



		}

	}

}

using UnityEngine;
using System.Collections;

public class testarfilm : MonoBehaviour {

	public MovieTexture _intro;

	// Use this for initialization
	void Start () {
	
		renderer.material.mainTexture = _intro;
		_intro.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

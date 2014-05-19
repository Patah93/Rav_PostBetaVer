using UnityEngine;
using System.Collections;

public class MoveFoxOnTrigger : TriggerAction {

	FoxAI _foxAI;

	// Use this for initialization
	void Start () {
		_foxAI = GetComponent<FoxAI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void onActive(){
		_foxAI.setBoyClose(true);
		
	}
	
	public override void onInactive(){
		_foxAI.setBoyClose(false);
	}
}

using UnityEngine;
using System.Collections;

public class InactivateScriptOnTrigger : TriggerAction {
	
	public MonoBehaviour _theScript;

	public bool _inactivateWhenActive = false;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void onActive(){
		_theScript.enabled = !_inactivateWhenActive;
	}
	
	public override void onInactive(){
		_theScript.enabled = _inactivateWhenActive;
	}
}

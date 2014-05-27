using UnityEngine;
using System.Collections;

public class InactivateScriptOnTrigger : TriggerAction {
	
	public MonoBehaviour _theScript;

	public bool _inactivateWhenActive = false;

	public bool _onceAndOnlyOnce = true;

	private bool _done = false;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void onActive(){
		if (_onceAndOnlyOnce && !_done) {
			_theScript.enabled = !_inactivateWhenActive;
			_done = true;
		}
		else if(!_onceAndOnlyOnce){
			_theScript.enabled = !_inactivateWhenActive;
		}
	}

	public override void onInactive(){
		if (_onceAndOnlyOnce && !_done) {
			_theScript.enabled = _inactivateWhenActive;
			_done = true;
		}
		else if(!_onceAndOnlyOnce){
			_theScript.enabled = _inactivateWhenActive;
		}
	}
}

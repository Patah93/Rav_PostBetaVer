using UnityEngine;
using System.Collections;

public class TriggerScriptGroup : TriggerAction {

	public TriggerAction[] theScripts;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void onActive(){
		for (int i = 0; i < theScripts.Length; i++) {
			if(theScripts[i].GetType() != typeof(TriggerScriptGroup)){
				theScripts[i].onActive();
			}
		}
	}
	
	public override void onInactive(){
		for (int i = 0; i < theScripts.Length; i++) {
			if(theScripts[i].GetType() != typeof(TriggerScriptGroup)){
				theScripts[i].onInactive();
			}
		}
	}
}

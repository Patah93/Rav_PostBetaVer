using UnityEngine;
using System.Collections;

public class TriggerVolumeButtonPress : MonoBehaviour {

	private bool _inVolume = false;
	private bool _active = false;

	public TriggerAction[] _triggerScripts;
	public TriggerAction[] _triggerScriptsInactive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_inVolume) {
			if(Input.GetButtonDown("Interact")){
				if(!_active){
					for(int i = 0; i < _triggerScripts.Length; i++){
						_triggerScripts[i].onActive();
					}
					for(int j = 0; j < _triggerScriptsInactive.Length; j++){
						_triggerScriptsInactive[j].onInactive();
					}
					_active = true;
				}
				else{
					for(int i = 0; i < _triggerScripts.Length; i++){
						_triggerScripts[i].onInactive();
					}
					for(int j = 0; j < _triggerScriptsInactive.Length; j++){
						_triggerScriptsInactive[j].onActive();
					}
					_active = false;
				}
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(this.enabled && other.tag == "Player"){
			_inVolume = true;

			for(int j = 0; j < _triggerScriptsInactive.Length; j++){
				_triggerScriptsInactive[j].onActive();
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(this.enabled && other.tag == "Player"){
			_inVolume = false;

			for(int i = 0; i < _triggerScripts.Length; i++){
				_triggerScripts[i].onInactive();
			}
			for(int j = 0; j < _triggerScriptsInactive.Length; j++){
				_triggerScriptsInactive[j].onInactive();
			}
			_active = false;
		}
	}
}

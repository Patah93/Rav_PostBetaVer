using UnityEngine;
using System.Collections;

public abstract class TriggerAction : MonoBehaviour{
	public abstract void onActive();
	public abstract void onInactive();
}

public class Trigger : MonoBehaviour {

	public GameObject[] _actionObj;

	public GameObject[] _triggerableObjects;

	TriggerAction[] _action;

	public bool _activatedByInteractables = true;

	public bool _activatedByThrowables = false;

	public int _scriptNumber = 0;

	int _numberOfThings = 0;

	public AudioClip _triggeredSound, _unTriggeredSound;

	AudioSource _audioSource;

	// Use this for initialization
	void Start () {
		_action = new TriggerAction[_actionObj.Length];
		for(int i = 0; i < _action.Length; i++){
			TriggerAction[] _tAction = _actionObj[i].GetComponents<TriggerAction>();

			_action[i] = _tAction[_scriptNumber];
		}

		_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(this.enabled){
			if(_activatedByInteractables && other.tag == "Interactive" || _activatedByThrowables && other.tag == "Throwable"){
				if(_numberOfThings == 0){
					for(int i = 0; i < _action.Length; i++){
						_action[i].onActive();
					}

					if(_triggeredSound != null && _audioSource != null){
						_audioSource.Stop();
						_audioSource.clip = _triggeredSound;
						_audioSource.Play();
					}
					//gameObject.renderer.material.color = Color.red;
				}
				_numberOfThings++;
				return;
			}

			for(int i = 0 ; i < _triggerableObjects.Length; i++){
				if(_triggerableObjects[i] == other.gameObject){
					if(_numberOfThings == 0){
						for(int j = 0; j < _action.Length; j++){
							_action[j].onActive();
						}
						//gameObject.renderer.material.color = Color.red;

						if(_triggeredSound != null && _audioSource != null){
							_audioSource.Stop();
							_audioSource.clip = _triggeredSound;
							_audioSource.Play();
						}
					}
					_numberOfThings++;
					return;
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(this.enabled){
			if(_activatedByInteractables && other.tag == "Interactive" || _activatedByThrowables && other.tag == "Throwable"){
				_numberOfThings--;
				if(_numberOfThings <= 0){
					for(int i = 0; i < _action.Length; i++){
						_action[i].onInactive();
					}

					if(_unTriggeredSound != null && _audioSource != null){
						_audioSource.Stop();
						_audioSource.clip = _unTriggeredSound;
						_audioSource.Play();
					}
					//gameObject.renderer.material.color = Color.green;
					return;
				}
			}

			for(int i = 0 ; i < _triggerableObjects.Length; i++){
				if(_triggerableObjects[i] == other.gameObject){
					_numberOfThings--;
					if(_numberOfThings <= 0){
						for(int j = 0; j < _action.Length; j++){
							_action[j].onInactive();
						}

						if(_unTriggeredSound != null && _audioSource != null){
							_audioSource.Stop();
							_audioSource.clip = _unTriggeredSound;
							_audioSource.Play();
						}
						//gameObject.renderer.material.color = Color.green;
						return;
					}
				}
			}
		}
	}
}

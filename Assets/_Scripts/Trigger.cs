using UnityEngine;
using System.Collections;

public abstract class TriggerAction : MonoBehaviour{
	public abstract void onActive();
	public abstract void onInactive();
}

public class Trigger : MonoBehaviour {

	//public GameObject[] _actionObj;

	public GameObject[] _triggerableObjects;

	public TriggerAction[] _scripts;

	public bool _activatedByInteractables = true;

	public bool _activatedByThrowables = false;

	int _numberOfThings = 0;

	public AudioClip _triggeredSound, _unTriggeredSound;

	AudioSource _audioSource;

	// Use this for initialization
	void Start () {
		_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(_activatedByInteractables && other.tag == "Interactive" || _activatedByThrowables && other.tag == "Throwable"){
			if(_numberOfThings == 0){
				for(int i = 0; i < _scripts.Length; i++){
					_scripts[i].onActive();
				}

				if(_triggeredSound != null){
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
					for(int j = 0; j < _scripts.Length; j++){
						_scripts[j].onActive();
					}
					//gameObject.renderer.material.color = Color.red;

					if(_triggeredSound != null){
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

	void OnTriggerExit(Collider other) {
		if(_activatedByInteractables && other.tag == "Interactive" || _activatedByThrowables && other.tag == "Throwable"){
			_numberOfThings--;
			if(_numberOfThings <= 0){
				for(int i = 0; i < _scripts.Length; i++){
					_scripts[i].onInactive();
				}

				if(_unTriggeredSound != null){
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
					for(int j = 0; j < _scripts.Length; j++){
						_scripts[j].onInactive();
					}

					if(_unTriggeredSound != null){
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

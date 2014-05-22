using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Button : MonoBehaviour {
	
	public GameObject[] _actionObj;

	GameObject _boy;
	
	TriggerAction[] _action;

	public float _PRESS_DISTANCE = 4;

	public float _PRESS_ANGLE = 45;

	public bool _pressed = false;

	public int _scriptNumber = 0;

	Animator _ani;

	ButtonMan _button;

	bool _checkevent = false;

	//AnimationMan _man;
	
	// Use this for initialization
	void Start () {
		//_man = gameObject.GetComponent<AnimationMan>();
		_button = GameObject.FindWithTag("Player").GetComponent<ButtonMan>();
		_ani = gameObject.GetComponent<Animator>();
		_action = new TriggerAction[_actionObj.Length];
		for(int i = 0; i < _action.Length; i++){
			TriggerAction[] _tAction = _actionObj[i].GetComponents<TriggerAction>();

			_action[i] = _tAction[_scriptNumber];
		}

		if(_pressed){
			for(int i = 0; i < _action.Length; i++){
				_action[i].onActive();
			}
		}

		_boy = GameObject.FindGameObjectWithTag("Player");
		_ani = _boy.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Interact")){
			if((_boy.transform.position - gameObject.transform.position).sqrMagnitude <= _PRESS_DISTANCE){
				if(Vector2.Angle((new Vector2(gameObject.transform.position.x, gameObject.transform.position.z) - new Vector2(_boy.transform.position.x, _boy.transform.position.z)).normalized, new Vector2(_boy.transform.forward.x, _boy.transform.forward.z))  <= _PRESS_ANGLE){
					_ani.SetBool("PushButton",true);
				//	_ani.applyRootMotion = false;
					_checkevent = true;

					//Color temp = gameObject.renderer.materials[1].color;
					//gameObject.renderer.materials[1].color = gameObject.renderer.materials[0].color;
					//gameObject.renderer.materials[0].color = temp;
				}
			}
		}
		if(_checkevent){
			if(_button.isPressed()){
				Debug.Log ("Registered pressed button");
				transform.audio.Play();
				if(_pressed){
					for(int i = 0; i < _action.Length; i++){
						_action[i].onInactive();
					}
					_pressed = false;
				}else{
					for(int i = 0; i < _action.Length; i++){
						_action[i].onActive();
					}
					_pressed = true;
				}
				_button.setPressed(false);
				_ani.SetBool("PushButton",false);
			}
		}
		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
			//_ani.applyRootMotion = true;
		}
	}
	/*
	void OnTriggerEnter(Collider other) {
		if(_activatedByInteractables && other.tag == "Interactive"){
			_action.onActive();
			_numberOfThings++;
			return;
		}
		
		for(int i = 0 ; i < _triggerableObjects.Length; i++){
			if(_triggerableObjects[i] == other.gameObject){
				_action.onActive();
				_numberOfThings++;
				return;
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		
		if(_activatedByInteractables && other.tag == "Interactive"){
			_numberOfThings--;
			if(_numberOfThings <= 0){
				_action.onInactive();
				return;
			}
		}
		
		for(int i = 0 ; i < _triggerableObjects.Length; i++){
			if(_triggerableObjects[i] == other.gameObject){
				_numberOfThings--;
				if(_numberOfThings <= 0){
					_action.onInactive();
					return;
				}
			}
		}
	}
	*/
}

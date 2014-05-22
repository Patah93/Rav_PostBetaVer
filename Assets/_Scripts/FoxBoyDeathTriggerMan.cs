using UnityEngine;
using System.Collections;

public class FoxBoyDeathTriggerMan : TriggerAction {

	//public bool _boyDead = false;
	private bool _tick = false;
	public float _deathClock = 0.0f;
	public float _maxTime = 10;
	JumpingMan _mrJump;


	void Start () {

		_mrJump = GetComponent<JumpingMan>();
	}
	
	// Update is called once per frame
	void Update () {

		if(_tick){
			Debug.Log("INBOYKILLERCOUNTER");
			_deathClock += Time.deltaTime;
			if(_deathClock >= _maxTime){
				_mrJump.setDead(true);
			}
		}
	}
	
	public override void onActive(){
		Debug.Log("WENT IN");
		_tick = false;
		_deathClock = 0.0f;
	}
	
	public override void onInactive(){
		Debug.Log("WENT OUT");
		_tick = true;
	}
}

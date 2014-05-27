using UnityEngine;
using System.Collections;

public class ParticleSwitch : TriggerAction {
	
	public float _countDown;
	public bool _counterStart;
	public bool _triggered;

	public GameObject _from;
	public GameObject _to;

	// Use this for initialization
	void Start () {
		//_countDown = 20;
		_counterStart = false;
		_triggered = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("HEJ HEJ PARTIKELUPDATE");

		if(_counterStart == true){
			_countDown -= Time.deltaTime;
			if(_countDown <= 0){
				Destroy(_to);
			}
		}
	}

	public override void onActive(){



		if (_to && _triggered == false) {

			_triggered = true;
			_to.particleSystem.Play ();
			_counterStart = true;

		}

		if(_from && _triggered == false) {
			//Debug.Log ("NEJ NEJ NU HÄNDER SAKER");
			_triggered = true;
			//_from.particleSystem.loop = false;
			_counterStart = true;

		}
	}

	public override void onInactive(){

	}

}

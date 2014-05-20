using UnityEngine;
using System.Collections;

public class FoxSoundZone : MonoBehaviour {

	public GameObject[] _music;

	FadeSound[] _fade;

	bool[] _prevActivated;

	void Start(){

		_prevActivated = new bool[_music.Length];
		_fade = new FadeSound[_music.Length];
		for(int i = 0; i < _music.Length; i++){

			_fade[i] = _music[i].GetComponent<FadeSound>();

		}

	}

	void OnTriggerEnter(Collider c){

		if(c.CompareTag("Player")){

			for(int i = 0; i < _music.Length; i++){

				_prevActivated[i] = _fade[i].GetBool();
				_fade[i].ChangeState(false);

			}

		}

	}

	void OnTriggerExit(Collider c){

		if(c.CompareTag("Player")){

			for(int i = 0; i < _music.Length; i++){

				_fade[i].ChangeState(_prevActivated[i]);

			}

		}

	}
}

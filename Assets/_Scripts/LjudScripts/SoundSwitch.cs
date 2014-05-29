using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]

public class SoundSwitch : MonoBehaviour {

	public GameObject[] _audioSourcesBlue, _audioSourcesBlueBack, _audioSourcesRed, _audioSourcesRedBack;

	public bool _destroyAfterSwitch;

	void OnTriggerExit(Collider c){

		Debug.Log(c.tag);


		if(c.CompareTag("Player")){

			float angleForward = Vector3.Angle(transform.forward, c.transform.forward);
			float angleRight = Vector3.Angle(transform.right,c.transform.forward);

			if(angleForward < 45){

				for(int i = 0; i < _audioSourcesBlueBack.Length; i++)
					_audioSourcesBlueBack[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesRedBack.Length; i++)
					_audioSourcesRedBack[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesRed.Length; i++)
					_audioSourcesRed[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesBlue.Length; i++)
					_audioSourcesBlue[i].GetComponent<FadeSound>().ChangeState(true);
				
			} else if(angleForward > 45 && angleForward < 135){

				if(angleRight < 90){
					for(int i = 0; i < _audioSourcesBlue.Length; i++)
						_audioSourcesBlue[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesBlueBack.Length; i++)
						_audioSourcesBlueBack[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesRedBack.Length; i++)
						_audioSourcesRedBack[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesRed.Length; i++)
						_audioSourcesRed[i].GetComponent<FadeSound>().ChangeState(true);
				} else {
					for(int i = 0; i < _audioSourcesBlue.Length; i++)
						_audioSourcesBlue[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesBlueBack.Length; i++)
						_audioSourcesBlueBack[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesRed.Length; i++)
						_audioSourcesRed[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesRedBack.Length; i++)
						_audioSourcesRedBack[i].GetComponent<FadeSound>().ChangeState(true);
				}
				
			} else if(angleForward > 135){

				for(int i = 0; i < _audioSourcesBlue.Length; i++)
					_audioSourcesBlue[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesRedBack.Length; i++)
					_audioSourcesRedBack[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesRed.Length; i++)
					_audioSourcesRed[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesBlueBack.Length; i++)
					_audioSourcesBlueBack[i].GetComponent<FadeSound>().ChangeState(true);

			}
			if(_destroyAfterSwitch)
				Destroy(gameObject);

		}

	}

}

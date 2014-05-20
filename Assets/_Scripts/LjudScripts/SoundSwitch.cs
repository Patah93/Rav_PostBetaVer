using UnityEngine;
using System.Collections;

public class SoundSwitch : MonoBehaviour {

	public GameObject[] _audioSourcesForward, _audioSourcesBackward, _audioSourcesLeft, _audioSourcesRight;

	void OnTriggerExit(Collider c){

		Debug.Log(c.tag);


		if(c.CompareTag("Player")){

			float angleForward = Vector3.Angle(transform.forward, c.transform.forward);
			float angleRight = Vector3.Angle(transform.right,c.transform.forward);

			if(angleForward < 45){
				
				for(int i = 0; i < _audioSourcesForward.Length; i++)
					_audioSourcesForward[i].GetComponent<FadeSound>().ChangeState(true);
				for(int i = 0; i < _audioSourcesBackward.Length; i++)
					_audioSourcesBackward[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesLeft.Length; i++)
					_audioSourcesLeft[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesRight.Length; i++)
					_audioSourcesRight[i].GetComponent<FadeSound>().ChangeState(false);
				
			} else if(angleForward > 45 && angleForward < 135){

				if(angleRight < 90){
					for(int i = 0; i < _audioSourcesForward.Length; i++)
						_audioSourcesForward[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesBackward.Length; i++)
						_audioSourcesBackward[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesLeft.Length; i++)
						_audioSourcesLeft[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesRight.Length; i++)
						_audioSourcesRight[i].GetComponent<FadeSound>().ChangeState(true);
				} else {
					for(int i = 0; i < _audioSourcesForward.Length; i++)
						_audioSourcesForward[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesBackward.Length; i++)
						_audioSourcesBackward[i].GetComponent<FadeSound>().ChangeState(false);
					for(int i = 0; i < _audioSourcesLeft.Length; i++)
						_audioSourcesLeft[i].GetComponent<FadeSound>().ChangeState(true);
					for(int i = 0; i < _audioSourcesRight.Length; i++)
						_audioSourcesRight[i].GetComponent<FadeSound>().ChangeState(false);
				}
				
			} else if(angleForward > 135){

				for(int i = 0; i < _audioSourcesForward.Length; i++)
					_audioSourcesForward[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesBackward.Length; i++)
					_audioSourcesBackward[i].GetComponent<FadeSound>().ChangeState(true);
				for(int i = 0; i < _audioSourcesLeft.Length; i++)
					_audioSourcesLeft[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesRight.Length; i++)
					_audioSourcesRight[i].GetComponent<FadeSound>().ChangeState(false);

			}

		}

	}

}

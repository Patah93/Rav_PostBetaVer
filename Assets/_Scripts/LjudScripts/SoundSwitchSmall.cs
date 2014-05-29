using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]

public class SoundSwitchSmall : MonoBehaviour {

	public GameObject[] _audioSourcesBlue, _audioSourcesBlueBack;

	public bool _destroyAfterSwitch;

	void OnTriggerExit(Collider c){

		Debug.Log(c.tag);


		if(c.CompareTag("Player")){

			float angleForward = Vector3.Angle(transform.forward, c.transform.forward);
			float angleRight = Vector3.Angle(transform.right,c.transform.forward);

			if(angleForward < 90){

				for(int i = 0; i < _audioSourcesBlueBack.Length; i++)
					_audioSourcesBlueBack[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesBlue.Length; i++)
					_audioSourcesBlue[i].GetComponent<FadeSound>().ChangeState(true);

			} else if(angleForward > 90){

				for(int i = 0; i < _audioSourcesBlue.Length; i++)
					_audioSourcesBlue[i].GetComponent<FadeSound>().ChangeState(false);
				for(int i = 0; i < _audioSourcesBlueBack.Length; i++)
					_audioSourcesBlueBack[i].GetComponent<FadeSound>().ChangeState(true);

			}
			if(_destroyAfterSwitch)
				Destroy(gameObject);

		}

	}

}

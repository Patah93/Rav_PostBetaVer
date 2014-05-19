using UnityEngine;
using System.Collections;

public class SoundSwitch : MonoBehaviour {

	public GameObject[] _windSources;

	void OnTriggerEnter(Collider c){
		if(c.CompareTag("Player")){
			//_activateWindSource1.GetComponent<FadeSound>().ChangeState();
			//_activateWindSource2.GetComponent<FadeSound>().ChangeState();
			for(int i = 0; i < _windSources.Length; i++)
				_windSources[i].GetComponent<FadeSound>().ChangeState();
		}
	}

}

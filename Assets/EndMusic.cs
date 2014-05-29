using UnityEngine;
using System.Collections;

public class EndMusic : MonoBehaviour {

	public FadeSound[] _audio;

	public void Deactivate(){
		
		for(int i = 0; i < _audio.Length; i++)
			_audio[i].ChangeState(false);
	}

}

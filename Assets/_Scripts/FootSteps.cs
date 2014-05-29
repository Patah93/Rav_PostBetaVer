using UnityEngine;
using System.Collections;

public class FootSteps : MonoBehaviour {

	public GameObject _audioSauce;

	public Transform _spawnPos;

	public AudioClip[][] _audios;
	
	public AudioClip[] _audioClips1, _audioClips2, _audioClips3, _audioClips4, _audioClips5, _audioClips6, _audioClips7, _audioClips8, _audioClips9;

	[Range(0,1)]
	public float _boyVolumeStone, _boyVolumeSand, _boyVolumeMetal,  _foxVolumeStone, _foxVolumeSand, _foxVolumeMetal;

	int _previousFoot;
	
	void Awake () {
		int count = 0;
		if (_audioClips1.Length > 0) {
			count++;
			if (_audioClips2.Length > 0) {
				count++;
				if (_audioClips3.Length > 0) {
					count++;
					if (_audioClips4.Length > 0) {
						count++;
						if (_audioClips5.Length > 0) {
							count++;
							if (_audioClips6.Length > 0) {
								count++;	
								if (_audioClips7.Length > 0) {
									count++;	
									if (_audioClips8.Length > 0) {
										count++;	
										if (_audioClips9.Length > 0) {
											count++;	
										}
									}
								}
							}
						}
					}
				}
			}
		}

		if (count > 0) {
			_audios = new AudioClip[count][];

			if(count > 0){
				_audios[0] = _audioClips1;
				if(count > 1){
					_audios[1] = _audioClips2;
					if(count > 2){
						_audios[2] = _audioClips3;
						if(count > 3){
							_audios[3] = _audioClips4;
							if(count > 4){
								_audios[4] = _audioClips5;
								if(count > 5){
									_audios[5] = _audioClips6;
									if(count > 6){
										_audios[6] = _audioClips7;
										if(count > 7){
											_audios[7] = _audioClips8;
											if(count > 8){
												_audios[8] = _audioClips9;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public AudioClip getRandomClip(int index){
		if(index <= _audios.Length)
			return _audios[index][Random.Range(0,_audios[index].Length)];
		return null;
	}

	void Footstep(int i){
		int a = i;
		if(tag == "Player"){
			GameObject sauce = (GameObject)Instantiate (_audioSauce, _spawnPos.position, Quaternion.identity);
			RaycastHit outHit;
			if(Physics.Raycast(transform.position + transform.up * 0.5f, Vector3.down, out outHit, 2f)){
				GroundType ground = outHit.transform.GetComponent<GroundType>();
				if(ground != null)
					a += (ground.GetType());
			}
			sauce.audio.clip = getRandomClip (a);
			//sauce.audio.volume = _boyVolume;
			switch(a){
			case 0:
				sauce.audio.volume = _boyVolumeStone;
				break;
			case 1:
				sauce.audio.volume = _boyVolumeSand;
				break;
			case 2:
				sauce.audio.volume = _boyVolumeMetal;
				break;
			}
			sauce.audio.Play ();
		}
		
		else if(tag == "Fox" && i != _previousFoot){

			//Debug.Log ("FOX STEPS");
			RaycastHit outHit;
			if(Physics.Raycast(transform.position + transform.up * 0.5f, Vector3.down, out outHit, 2f)){
				GroundType ground = outHit.transform.GetComponent<GroundType>();
				if(ground != null)
					a += (ground.GetType());
				GameObject sauce = (GameObject)Instantiate (_audioSauce, _spawnPos.position, Quaternion.identity);
				sauce.audio.clip = getRandomClip (a);
				//sauce.audio.volume = _boyVolume;
				switch(a){
				case 0:
					sauce.audio.volume = _foxVolumeStone;
					break;
				case 1:
					sauce.audio.volume = _foxVolumeSand;
					break;
				case 2:
					sauce.audio.volume = _foxVolumeMetal;
					break;
				}
				sauce.audio.Play ();
			}
			_previousFoot = i;
		}
	}
}

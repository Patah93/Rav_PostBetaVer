using UnityEngine;
using System.Collections;

public class FootSteps : MonoBehaviour {

	public AudioClip[][] _audios;
	
	public AudioClip[] _audioClips1, _audioClips2, _audioClips3, _audioClips4, _audioClips5;
	
	void Awake () {
		int count = 0;
		int maxLength = 0;
		if (_audioClips1.Length > 0) {
			count++;
			maxLength = _audioClips1.Length;
			if (_audioClips2.Length > 0) {
				count++;
				if(_audioClips2.Length > maxLength){
					maxLength = _audioClips2.Length;
				}
				if (_audioClips3.Length > 0) {
					count++;
					if(_audioClips3.Length > maxLength){
						maxLength = _audioClips3.Length;
					}
					if (_audioClips4.Length > 0) {
						count++;
						if(_audioClips4.Length > maxLength){
							maxLength = _audioClips4.Length;
						}
						if (_audioClips5.Length > 0) {
							count++;	
							if(_audioClips5.Length > maxLength){
								maxLength = _audioClips5.Length;
							}
						}
					}
				}
			}
		}

		if (count > 0 && maxLength > 0) {
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
							}
						}
					}
				}
			}
		}
	}
	// Use this for initialization
	void Start () {
		Debug.Log (_audios);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public AudioClip getRandomClip(int index){
		if(index+1 > _audios.Length)
			return _audios[index][Random.Range(0,_audios[index].Length)];
		return null;
	}
}

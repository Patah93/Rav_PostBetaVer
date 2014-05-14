using UnityEngine;
using System.Collections;

public class FootSteps : MonoBehaviour {

	public GameObject _audioSauce;
	public AudioClip[][] _audios;
	
	public AudioClip[] _audioClips1, _audioClips2, _audioClips3, _audioClips4, _audioClips5;
	
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
							}
						}
					}
				}
			}
		}
	}
	// Use this for initialization
	void Start () {
		Debug.Log (_audios[0].Length);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public AudioClip getRandomClip(int index){
		if(index <= _audios.Length)
			return _audios[index][Random.Range(0,_audios[index].Length)];
		return null;
	}

	void Footstep(int i){
		Debug.Log ("Hello! Fot, fotsteg, fot fot, fotsteg steg: " + i);
		RaycastHit _rayHit;
		Physics.SphereCast (transform.position + new Vector3 (0, 1, 0), 0.3f, Vector3.down, out _rayHit, 1.1f);
		GameObject sauce = (GameObject)Instantiate (_audioSauce, GameObject.Find ("L_foot_joint").transform.position, Quaternion.identity);
		
		sauce.audio.clip = GetComponent<FootSteps> ().getRandomClip (0);
		sauce.audio.Play ();
	}
}

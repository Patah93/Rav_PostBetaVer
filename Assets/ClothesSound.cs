using UnityEngine;
using System.Collections;

public class ClothesSound : MonoBehaviour {

	Animator _ani;

	AudioSource _audio, _audioLoop;

	public AudioClip _walkSound;

	public AudioClip _runSound;

	public AudioClip[] _jumpSounds;

	public AudioClip[] _pickUpSounds;

	public AudioClip[] _throwSounds;

	[Range (0.0f, 1.0f)]
	public float _WALK_TO_RUN_SPEED_LIMIT = 0.7f;
	[Range (0.0f, 1.0f)]
	public float _WALK_VOLUME = 0.446f;
	[Range (0.0f, 1.0f)]
	public float _RUN_VOLUME = 0.451f;
	[Range (0.0f, 1.0f)]
	public float _JUMP_VOLUME = 0.068f;
	[Range (0.0f, 1.0f)]
	public float _PICKUP_VOLUME = 0.223f;
	[Range (0.0f, 1.0f)]
	public float _THROW_VOLUME = 0.338f;

	private enum SoundType{ WALK, RUN, JUMP, PICK, THROW, NONE};

	private SoundType _soundType = SoundType.NONE;
	/*
	[Range(0.0f, 1.0f)]
	public float _minVolume = 0.0f;
	[Range(0.0f, 1.0f)]
	public float _maxVolume = 1.0f;
	*/


	// Use this for initialization
	void Start () {
		_ani = GetComponent<Animator>();
		AudioSource[] sources = GetComponents<AudioSource>();
		_audio = sources [0];
		_audioLoop = sources [1];
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo aniState = _ani.GetCurrentAnimatorStateInfo(0);
		if(aniState.IsName("Run")){
			if(_ani.GetFloat("Speed") < _WALK_TO_RUN_SPEED_LIMIT){
				if(_soundType != SoundType.WALK){
					/* TODO fade into runsound */
					_audio.Stop();
					_audioLoop.Stop();
					_audioLoop.volume = _WALK_VOLUME;
					_audioLoop.clip = _walkSound;
					_audioLoop.Play();
					_soundType = SoundType.WALK;
				}
			}
			else{
				if(_soundType != SoundType.RUN){
					/* TODO fade into runsound */
					_audio.Stop();
					_audioLoop.Stop();
					_audioLoop.volume = _RUN_VOLUME;
					_audioLoop.clip = _runSound;
					_audioLoop.Play();
					_soundType = SoundType.RUN;
				}
			}
		}else if(aniState.IsName("Push") || aniState.IsName("Pull")){
			if(_soundType != SoundType.WALK){
				/* TODO fade into runsound */
				_audio.Stop();
				_audioLoop.Stop();
				_audioLoop.volume = _WALK_VOLUME;
				_audioLoop.clip = _walkSound;
				_audioLoop.Play();
				_soundType = SoundType.WALK;
			}
		}else if(aniState.IsName("Jump") || aniState.IsName("Fallin'") || aniState.IsName("Land")){
			if(_soundType != SoundType.JUMP){
				/* TODO fade into runsound */
				_audio.Stop();
				_audioLoop.Stop();
				_audio.volume = _JUMP_VOLUME;
				_audio.clip = _jumpSounds[Random.Range(0, _jumpSounds.Length)];
				_audio.Play();
				_soundType = SoundType.JUMP;
			}
		}else if(aniState.IsName("Throw Prepare")){
			if(_soundType != SoundType.PICK){
				/* TODO fade into runsound */
				_audio.Stop();
				_audioLoop.Stop();
				_audio.volume = _PICKUP_VOLUME;
				_audio.clip = _pickUpSounds[Random.Range(0, _pickUpSounds.Length)];
				_audio.Play();
				_soundType = SoundType.PICK;
			}
		}else if(aniState.IsName("Throw")){
			if(_soundType != SoundType.THROW){
				/* TODO fade into runsound */
				_audio.Stop();
				_audioLoop.Stop();
				_audio.volume = _THROW_VOLUME;
				_audio.clip = _throwSounds[Random.Range(0, _throwSounds.Length)];
				_audio.Play();
				_soundType = SoundType.THROW;
			}
		}else{
			/* TODO fade out sound completely */
			_audio.Pause();
			_audioLoop.Pause();
		}
	}
}

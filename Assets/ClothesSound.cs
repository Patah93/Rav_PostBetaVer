using UnityEngine;
using System.Collections;

public class ClothesSound : MonoBehaviour {

	Animator _ani;

	AudioSource _audio, _audioVoice;

	public AudioClip _walkSound;

	public AudioClip _runSound;

	public AudioClip _breatheSound;

	public AudioClip[] _jumpSoundsClothes;

	public AudioClip[] _jumpSoundsVoice;

	public AudioClip[] _pickUpSounds;

	public AudioClip[] _throwSounds;

	[Range (0.0f, 1.0f)]
	public float _WALK_TO_RUN_SPEED_LIMIT = 0.7f;
	[Range (0.0f, 1.0f)]
	public float _WALK_VOLUME = 0.446f;
	[Range (0.0f, 1.0f)]
	public float _RUN_VOLUME = 0.451f;
	[Range (0.0f, 1.0f)]
	public float _JUMP_CLOTHES_VOLUME = 0.068f;
	[Range (0.0f, 1.0f)]
	public float _JUMP_VOICE_VOLUME = 0.342f;
	[Range (0.0f, 1.0f)]
	public float _PICKUP_VOLUME = 0.223f;
	[Range (0.0f, 1.0f)]
	public float _THROW_VOLUME = 0.338f;
	[Range (0.0f, 1.0f)]
	public float _BREATHE_VOLUME = 0.493f;

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
		_audioVoice = sources [1];
		_audioVoice.volume = _BREATHE_VOLUME;
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo aniState = _ani.GetCurrentAnimatorStateInfo(0);
		if(aniState.IsName("Run")){
			if(_ani.GetFloat("Speed") < _WALK_TO_RUN_SPEED_LIMIT){
				if(_soundType != SoundType.WALK){
					/* TODO fade into runsound */
					_audio.Stop();
					_audioVoice.Stop();
					_audio.loop = true;
					_audio.volume = _WALK_VOLUME;
					_audio.clip = _walkSound;
					_audio.Play();
					_soundType = SoundType.WALK;
				}
			}
			else{
				if(_soundType != SoundType.RUN){
					/* TODO fade into runsound */
					_audio.Stop();
					_audioVoice.Stop();
					_audio.loop = true;
					_audioVoice.loop = true;
					_audio.volume = _RUN_VOLUME;
					_audioVoice.volume = _BREATHE_VOLUME;
					_audio.clip = _runSound;
					_audioVoice.clip = _breatheSound;
					_audio.Play();
					_audioVoice.Play();
					_soundType = SoundType.RUN;
				}
			}
		}else if(aniState.IsName("Push") || aniState.IsName("Pull")){
			if(_soundType != SoundType.WALK){
				/* TODO fade into runsound */
				_audio.Stop();
				_audioVoice.Stop();
				_audio.loop = true;
				_audio.volume = _WALK_VOLUME;
				_audio.clip = _walkSound;
				_audio.Play();
				_soundType = SoundType.WALK;
			}
		}else if(aniState.IsName("Jump") || aniState.IsName("Idle Jump")){
			if(_soundType != SoundType.JUMP){
				/* TODO fade into runsound */
				_audio.Stop();
				_audioVoice.Stop();
				_audio.loop = false;
				_audioVoice.loop = false;
				_audio.volume = _JUMP_CLOTHES_VOLUME;
				_audioVoice.volume = _JUMP_VOICE_VOLUME;
				_audio.clip = _jumpSoundsClothes[Random.Range(0, _jumpSoundsClothes.Length)];
				_audioVoice.clip = _jumpSoundsVoice[Random.Range(0, _jumpSoundsVoice.Length)];
				_audio.Play();
				_audioVoice.Play();
				_soundType = SoundType.JUMP;
			}
		}else if(aniState.IsName("Throw Prepare")){
			if(_soundType != SoundType.PICK){
				/* TODO fade into runsound */
				_audio.Stop();
				_audioVoice.Stop();
				_audio.loop = false;
				_audio.volume = _PICKUP_VOLUME;
				_audio.clip = _pickUpSounds[Random.Range(0, _pickUpSounds.Length)];
				_audio.Play();
				_soundType = SoundType.PICK;
			}
		}else if(aniState.IsName("Throw")){
			if(_soundType != SoundType.THROW){
				/* TODO fade into runsound */
				_audio.Stop();
				_audioVoice.Stop();
				_audio.loop = false;
				_audio.volume = _THROW_VOLUME;
				_audio.clip = _throwSounds[Random.Range(0, _throwSounds.Length)];
				_audio.Play();
				_soundType = SoundType.THROW;
			}
		}else if(aniState.IsName("Idle")){
			/* TODO fade out sound completely */
			_soundType = SoundType.NONE;
			_audio.Pause();
			_audioVoice.Pause();
		}else{
			_audio.Pause();
			_audioVoice.Pause();
		}
	}
}

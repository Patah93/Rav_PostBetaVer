using UnityEngine;
using System.Collections;

public class EpicCameraMovementOnTriggerMan : TriggerAction {

	public GameObject _targetLookAtObj;

	private Vector3 _targetPosition;
	private Vector3 _targetLookAt;

	public float _moveSpeed;
	public float _lookAtSpeed;

	public float _moveSpeed_back;
	public float _lookAtSpeed_back;

	public float _waitTime;

	public bool _canIHasMoveTheBoy = false;
	public bool _useSlerpInsteadOfLerp = false;
	public bool _onlyOnce = true;

	private bool _BEGIN = false;
	private bool _WAITSTATE = false;
	private bool _GOBACK = false;

	private Vector3 _lerpStart;
	private Vector3 _lookAtStart;

	private float _clock;

	private GameObject _camera;
	private ThirdPersonCamera _cameraMan;

	private AnimationMan _animMan;
	private JumpingMan _jumpMan;
	private BoyStateManager _boyState;
	private Throw _throw;

	// Use this for initialization
	void Start () {
		_camera = GameObject.FindGameObjectWithTag ("MainCamera");
		_cameraMan = _camera.GetComponent<ThirdPersonCamera> ();

		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		_animMan = player.GetComponent<AnimationMan>();
		_jumpMan = player.GetComponent<JumpingMan>();
		_boyState = player.GetComponent<BoyStateManager>();
		_throw = player.GetComponent<Throw>();

		_targetPosition = transform.position;
		_targetLookAt = _targetLookAtObj.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(_BEGIN){
			if(_useSlerpInsteadOfLerp){
				_camera.transform.position = Vector3.Slerp(_lerpStart, _targetPosition, (Time.time - _clock) * _moveSpeed);
				_camera.transform.LookAt(Vector3.Slerp(_lookAtStart, _targetLookAt, (Time.time - _clock) * _lookAtSpeed));
			}
			else{
				_camera.transform.position = Vector3.Lerp(_lerpStart, _targetPosition, (Time.time - _clock) * _moveSpeed);
				_camera.transform.LookAt(Vector3.Lerp(_lookAtStart, _targetLookAt, (Time.time - _clock) * _lookAtSpeed));
			}

			GameObject temp = new GameObject();
			temp.transform.position = _camera.transform.position;
			temp.transform.LookAt(_targetLookAt);

			if((_camera.transform.position - _targetPosition).sqrMagnitude < 0.05f && Vector3.Angle (temp.transform.forward, _camera.transform.forward) < 1.0f){
				_camera.transform.position = _targetPosition;
				_camera.transform.LookAt(_targetLookAt);

				//Debug.Log ("FINISHED MOVING CAMERA! =D");

				_BEGIN = false;
				_WAITSTATE = true;

				_clock = Time.time;
			}
			GameObject.Destroy(temp);
		}

		if (_WAITSTATE) {

			//Debug.Log ("WAITING...");

			if(Time.time - _clock > _waitTime){

				//Debug.Log ("FINISHED WAITING =D!");

				_WAITSTATE = false;
				_GOBACK = true;

				_clock = Time.time;
			}
		}

		if(_GOBACK){
			if(_useSlerpInsteadOfLerp){
				_camera.transform.position = Vector3.Slerp(_targetPosition, _lerpStart, (Time.time - _clock) * _moveSpeed_back);
				_camera.transform.LookAt(Vector3.Slerp(_targetLookAt, _lookAtStart, (Time.time - _clock) * _lookAtSpeed_back));
			}
			else{
				_camera.transform.position = Vector3.Lerp(_targetPosition, _lerpStart, (Time.time - _clock) * _moveSpeed_back);
				_camera.transform.LookAt(Vector3.Lerp(_targetLookAt, _lookAtStart, (Time.time - _clock) * _lookAtSpeed_back));
			}
			
			GameObject temp = new GameObject();
			temp.transform.position = _camera.transform.position;
			temp.transform.LookAt(_lookAtStart);
			
			if((_camera.transform.position - _lerpStart).sqrMagnitude < 0.05f && Vector3.Angle (temp.transform.forward, _camera.transform.forward) < 1.0f){
				_camera.transform.position = _lerpStart;
				_camera.transform.LookAt(_lookAtStart);

				if(!_canIHasMoveTheBoy){
					_animMan.enabled = true;
					_jumpMan.enabled = true;
					_boyState.enabled = true;
					_throw.enabled = true;
				}

				_cameraMan.enabled = true;
				_GOBACK = false;

				if(_onlyOnce){
					GameObject.Destroy(temp);
					this.enabled = false;
				}
			}
			GameObject.Destroy(temp);
		}
	}
	
	public override void onActive(){
		if(this.enabled){
			if(!_canIHasMoveTheBoy){
				_animMan.enabled = false;
				_jumpMan.enabled = false;
				_boyState.enabled = false;
				_throw.enabled = false;

				GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetFloat("Speed", 0);
				GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().applyRootMotion = false;
			}
			_cameraMan.enabled = false;
			_BEGIN = true;
			_lerpStart = _camera.transform.position;
			_lookAtStart = GameObject.FindGameObjectWithTag ("CameraFollow").transform.position;

			_clock = Time.time;
		}
	}
	
	public override void onInactive(){

	}
}
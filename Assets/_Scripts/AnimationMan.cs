using UnityEngine;
using System.Collections;

public class AnimationMan : MonoBehaviour {

	private Animator _animator;
	Vector2 _cameraRotationForward;
	Vector2 _cameraRotationRight;
	Vector2 _targetRotation;
	Vector2 _characterRotation;
	public float _length;
	float _angle;
	float _clock;

	bool _active = true;
	bool _spawn;
	Vector3 _stopPos;


	public Animator Animator {get{return this._animator;} }
	private AnimatorStateInfo stateInfo;
	private ThirdPersonCamera camera;

	//Animation hashes
	int m_LocomotionId = 0;
	[Range (1f,5f)]
	public float _lerpTime = 5f;
	[Range (1f,2f)]
	public float _lerpThrowTime = 2f;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		m_LocomotionId = Animator.StringToHash ("Base Layer.Run");
		camera = Camera.main.GetComponent<ThirdPersonCamera> ();
		_spawn = true;

		//if (camera == null)
						//Debug.Log ("no camera detected");

		//if (_animator == null)
			//Debug.LogError ("No animator!");
	}
	
	// Update is called once per frame
	void Update () {
       // RaycastHit bajs;
        //Physics.Raycast(new Ray(transform.position, Vector3.down), out bajs);
        //gameObject.transform.up = bajs.normal;
            
			stateInfo = _animator.GetCurrentAnimatorStateInfo (0);
			updateCameraRotation();
			joystickConvert ();
			updateCharacterRotation();
			
		////Debug.Log (gameObject.GetComponent<CharacterController>().velocity);
			
			float lerpit = _lerpTime;

			if(!_animator.GetBool("ThrowMode") && (camera.camState != ThirdPersonCamera.CamStates.Focus)){
				_length = Mathf.Sqrt(Mathf.Pow (Mathf.Abs(Input.GetAxis("Horizontal")),2) + Mathf.Pow (Mathf.Abs(Input.GetAxis("Vertical")),2));	
			} else{
				lerpit = _lerpThrowTime;
				_length = Mathf.Lerp(_length, 0, _lerpTime);
			}
			
			
		if((Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0) && (camera.camState != ThirdPersonCamera.CamStates.Focus)){
				if(!_animator.GetBool("Falling") && !_animator.GetBool("Jump") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Land") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Push Button")){

							_angle = Vector2.Angle (_cameraRotationForward, _targetRotation) * Mathf.Sign(Input.GetAxis ("Horizontal"));

							Quaternion targetRot = Quaternion.Slerp (transform.rotation, Camera.main.transform.rotation * Quaternion.Euler(0, _angle, 0), Time.deltaTime * lerpit);
							transform.rotation = new Quaternion(transform.rotation.x, targetRot.y, transform.rotation.z, targetRot.w);
					}
				}
				else
				{
					_length = Mathf.Lerp(_length, 0, _lerpTime);
					//_stop = true;

				}
				

			_length = (_length > 1) ? 1 : _length;
			_animator.SetFloat("Speed", _length);
			_animator.speed = (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) ? ((_length == 0) ? 1 : _length) : Mathf.Lerp( _animator.speed, 1f, 0.5f);
			if((_length == 0 && !_animator.GetBool("Falling")) || _animator.GetBool("Falling")){
				_animator.applyRootMotion = false;
			}
			else{
				_animator.applyRootMotion = true;
			}
		/*	if(_length == 0 && !_spawn){
				transform.position = new Vector3(_stopPos.x,transform.position.y,_stopPos.z);
			}

		_stopPos = transform.position;
		_spawn = false;
		*/

	}

	void OnCollisionEnter(){

	} 

	private void updateCameraRotation(){
		_cameraRotationForward = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z).normalized;
		_cameraRotationRight = new Vector2(Camera.main.transform.right.x, Camera.main.transform.right.z).normalized;
	}

	private void joystickConvert(){

		if(_animator.GetBool("ThrowMode"))
			_targetRotation = (Mathf.Clamp(Input.GetAxis("Vertical"),0.1f,1f) * _cameraRotationForward) + (Input.GetAxis("Horizontal") * _cameraRotationRight);
		else
			_targetRotation = (Input.GetAxis("Vertical") * _cameraRotationForward) + (Input.GetAxis("Horizontal") * _cameraRotationRight);

	}

	private void updateCharacterRotation(){
		_characterRotation = new Vector2(transform.forward.x, transform.forward.z);
	}

	private bool leftStickMoved(){
		return Mathf.Abs (Input.GetAxis ("Horizontal")) > 0 || Mathf.Abs (Input.GetAxis ("Vertical")) > 0;
	}

	public bool IsInLocomotion() {
		return stateInfo.GetHashCode() == m_LocomotionId;
	}
}

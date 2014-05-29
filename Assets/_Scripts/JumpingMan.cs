using UnityEngine;
using System.Collections;

public class JumpingMan : MonoBehaviour {

	private Animator _animator;
	private CharacterController _charCon;
	private Vector3 _jumpingMove = Vector3.zero;
	private float _clock;
	public float _maxTime = 1f;
	private bool _jump = false;
	private float _stickLength;
	private float _mrHigh = 0;

	public bool _dead = false;
	//public Vector3 _jumpLength;
	public float _maxFallHigh = 5;
	//public float _jumpForce = 300;
	//public float _offsetX = 0;
	//public float _offsetY = 0;
	//public float _offsetZ = 0;
	public float _gravity = 20.0f;
	public float _jumpHeight = 10.0f;
	[Range(0.01f, 2.0f)]
	public float _speedScale = 1.0f;
	//public float _maxDeadTime = 10.0f;
	//public float _deadTimer = 0;
	public float _rayLength = 1.0f;
	[Range(0, 25)]
	public float _jumpOffsetDistanceFuckers = 13;


	//float _startPosition;

	RaycastHit _rayHit;
	//AnimationMan _animan;

	public float _slidingAngle = 45;
	public float _fallingAngle = 70;
	[Range(0.0f, 1.0f)]
	public float _slidingFactor = 0.625f;


	private GameObject _camera;
	private Vector3 _airVelocity = Vector3.zero;
	public float _MAX_AIRSPEED = 4.8f;
	

	//FUNGERAR HELT OKEJ MEN BEHÖVER KASTA RAYS FRÅN KANTERNA PÅ GUBBEN

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_charCon = GetComponent<CharacterController> ();
		_camera = GameObject.FindGameObjectWithTag("MainCamera");
	//	_animan = GetComponent<AnimationMan>();
		//_startPosition = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

		_stickLength = (Mathf.Abs (Input.GetAxis ("Horizontal")) + Mathf.Abs (Input.GetAxis ("Vertical")));

		if(Camera.main.GetComponent<ThirdPersonCamera>().camState != ThirdPersonCamera.CamStates.Focus) {
        if (Input.GetButtonDown("Jump") && (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
        {	//Aktiverar hoppet
			////Debug.Log("you pressed jump)");
			if(!_jump){


				if(_stickLength != 0){
					_jumpingMove.x = transform.forward.x * _MAX_AIRSPEED;
					_jumpingMove.z = transform.forward.z * _MAX_AIRSPEED;
					_jumpingMove *= Mathf.Clamp(_stickLength,0, 1);
					_animator.SetBool("Jump", true);
				}else{
					_jumpingMove = Vector3.zero;
					_animator.SetBool("Jump", true);
				}
					

				_jumpingMove.y = _jumpHeight;
				_animator.applyRootMotion = false;
				_animator.SetBool("Jump", true);
				_clock = Time.time;


				_airVelocity = Vector3.zero;

			}

		}
		}
	

		//_animator.SetBool("Jump", false);
	/*	if(_jump && transform.position.y - _startPosition > _jumpHeight){	//Hoppat så högt den klarar, kör en cooldown
			//////Debug.Log("cooldown");
			_cooldown = true;
		}
		
*/
		if(!_jump && Physics.SphereCast(transform.position + new Vector3(0,1,0), 0.3f ,Vector3.down,out _rayHit,2.5f)){
			if(Physics.SphereCast(transform.position + new Vector3(0,1,0), 0.3f /*+ _offsetY*/ ,Vector3.down,out _rayHit, _rayLength)){
				if(Vector3.Angle(Vector3.up, _rayHit.normal) < _slidingAngle){
					_animator.SetBool("Falling", false);
					_mrHigh = transform.position.y;

					if(Time.time - _clock > _maxTime){
						_jumpingMove = Vector3.zero;
					}
				}else{
					Vector3 slide_vec = Vector3.RotateTowards(_rayHit.normal, Vector3.down, Mathf.PI/2.0f, 0).normalized;
					slide_vec *= _gravity*-slide_vec.y*Time.deltaTime*_slidingFactor;
					_charCon.Move(slide_vec);
				}
			}else{
				//_jumpingMove = _charCon.velocity*_speedScale;
				
				_jumpingMove.y -= _gravity*Time.deltaTime;
				_charCon.Move(_jumpingMove*Time.deltaTime);
				//_animan.enabled = false;
			}
			//_animan.enabled = true;
		} else{
			//_jumpingMove = _charCon.velocity*_speedScale;
			_animator.SetBool("Falling", true);

			_jumpingMove.y -= _gravity*Time.deltaTime;
			_charCon.Move(_jumpingMove*Time.deltaTime);
			//_animan.enabled = false;
		}

		if(_mrHigh - transform.position.y >= _maxFallHigh){
			_dead = true;
			_mrHigh = transform.position.y;
		}

		if(_jump){

			if(Vector3.Angle(Vector3.up, _rayHit.normal) < _slidingAngle){

				/*
				Vector3 latjo1 = (transform.forward * _jumpOffsetDistanceFuckers * Input.GetAxis ("Vertical"));
				latjo1 += (transform.right * _jumpOffsetDistanceFuckers * Input.GetAxis ("Horizontal"));

				if(Mathf.Abs(_jumpingMove.x) < _jumpOffsetDistanceFuckers){
					_jumpingMove.x = latjo1.x;
					//Debug.Log("JUMPING FORWARD");
				}
				else if(Mathf.Abs(_jumpingMove.z) < _jumpOffsetDistanceFuckers){
					_jumpingMove.z = latjo1.z;
					//Debug.Log("JUMPING SIDEWAYS");
				}
				*/

				Vector3 jumpForwardVec = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z).normalized * _jumpOffsetDistanceFuckers * Input.GetAxis ("Vertical") * Time.deltaTime;
				Vector3 jumpRightVec = new Vector3(_camera.transform.right.x, 0, _camera.transform.right.z).normalized * _jumpOffsetDistanceFuckers * Input.GetAxis ("Horizontal") * Time.deltaTime;

				_airVelocity = jumpRightVec + jumpForwardVec;

				//if(Mathf.Abs(_jumpingMove.x) < _jumpOffsetDistanceFuckers){
				_jumpingMove.x += _airVelocity.x;
					////Debug.Log("JUMPING FORWARD");
				//}
				//if(Mathf.Abs(_jumpingMove.z) < _jumpOffsetDistanceFuckers){
				_jumpingMove.z += _airVelocity.z;
					////Debug.Log("JUMPING SIDEWAYS");
				//}

				Vector2 _currentAirSpeed = new Vector2(_jumpingMove.x, _jumpingMove.z);

				if(_currentAirSpeed.sqrMagnitude > _MAX_AIRSPEED *_MAX_AIRSPEED){
					_currentAirSpeed.Normalize();
					_currentAirSpeed *= _MAX_AIRSPEED;

					_jumpingMove.x = _currentAirSpeed.x;
					_jumpingMove.z = _currentAirSpeed.y;
				}

				//_jumpingMove.y -= _gravity*Time.deltaTime;
				//_charCon.Move(_jumpingMove*Time.deltaTime);
			}/*else{
				/* TODO SLIDE LIKE A MAESTRO *
				Vector3 slide_vec = Vector3.RotateTowards(_rayHit.normal, Vector3.down, Mathf.PI/2.0f, 0).normalized;
				slide_vec *= _gravity*-slide_vec.y*Time.deltaTime*_slidingFactor;
				_charCon.Move(slide_vec);
			}*/

			//Vector3 temp = new Vector3(_offsetX,_offsetY,_offsetZ);
			if(Time.time - _clock > _maxTime){
				//Debug.Log("NO LONGER FALLING");
				if(Physics.SphereCast(transform.position + new Vector3(0,1,0), 0.3f /*+ temp.y*/ ,Vector3.down,out _rayHit, _rayLength)){	//Nuddat marken och kan hoppa igen
					//Debug.DrawRay(transform.position + temp,Vector3.down,Color.blue,1 + temp.y,true);
					//Debug.DrawRay(transform.position, _rayHit.transform.position);
					//_animator.SetBool("Falling", false);

					////Debug.Log("Collided with "+ _rayHit.collider.name);
					if(!_rayHit.collider.name.Equals(this.name)){
						//transform.rigidbody.constraints &= ~ RigidbodyConstraints.FreezeRotationX|~RigidbodyConstraints.FreezeRotationZ;
						////Debug.Log ("hit something"); 
						//_startPosition = transform.position.y;
						_animator.SetBool("Jump", false);
						if(Vector3.Angle(Vector3.up, _rayHit.normal) < _slidingAngle){

							_animator.SetBool("Falling", false);
							_jumpingMove = Vector3.zero;
							_jump = false;

							//_animator.SetBool("Jump", false);
							_animator.applyRootMotion = true;

							//rigidbody.constraints = ; 
							//_animan.enabled = true;


							/* TODO Plz ta bort desa två superdåliga rader kod, my bad */
							//gameObject.GetComponent<CharacterController>().enabled = true;
							//gameObject.GetComponent<CapsuleCollider>().enabled = false;
						}else{

							if(Vector3.Angle(Vector3.up, _rayHit.normal) < _fallingAngle){
								_animator.SetBool("Falling", false);
								_jumpingMove = Vector3.zero;
							}

							_animator.SetBool("Jump", false);
							
							Vector3 slide_vec = Vector3.RotateTowards(_rayHit.normal, Vector3.down, Mathf.PI/2.0f, 0).normalized;
							slide_vec *=  _gravity*-slide_vec.y*Time.deltaTime*_slidingFactor;
							_charCon.Move(slide_vec);

						}
					}
				}
			}

		}
		//////Debug.Log("cooldown is" + _cooldown);
		 

	}


	public bool isJumping(){
		return _jump;
	}

	public void Jumpy(){
		_jump = true;

	}

	public void setDead(bool status){
		_dead = status;
	}

}

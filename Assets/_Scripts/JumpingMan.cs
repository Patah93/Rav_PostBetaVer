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

	public bool _dead = false;
	public Vector3 _jumpLength;
	public float _jumpForce = 300;
	public float _offsetX = 0;
	public float _offsetY = 0;
	public float _offsetZ = 0;
	public float _gravity = 20.0f;
	public float _jumpHeight = 10.0f;
	[Range(0.01f, 2.0f)]
	public float _speedScale = 1.0f;
	public float _maxDeadTime = 10.0f;
	public float _deadTimer = 0;
	public float _rayLength = 1.0f;


	//float _startPosition;

	RaycastHit _rayHit;
	//AnimationMan _animan;





	

	//FUNGERAR HELT OKEJ MEN BEHÖVER KASTA RAYS FRÅN KANTERNA PÅ GUBBEN

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_charCon = GetComponent<CharacterController> ();
	//	_animan = GetComponent<AnimationMan>();
		//_startPosition = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

		_stickLength = (Mathf.Abs (Input.GetAxis ("Horizontal")) + Mathf.Abs (Input.GetAxis ("Vertical")));

        if (Input.GetButtonDown("Jump") && (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
        {	//Aktiverar hoppet
			//Debug.Log("you pressed jump)");
			if(!_jump){


				if(_stickLength != 0){
					_jumpingMove.x = transform.forward.x * _jumpLength.x;
					_jumpingMove.z = transform.forward.z * _jumpLength.z;
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




			}

		}
	

		//_animator.SetBool("Jump", false);
	/*	if(_jump && transform.position.y - _startPosition > _jumpHeight){	//Hoppat så högt den klarar, kör en cooldown
			////Debug.Log("cooldown");
			_cooldown = true;
		}
		
*/
		if(!_jump && Physics.SphereCast(transform.position + new Vector3(0,1,0), 0.3f ,Vector3.down,out _rayHit,2.5f)){
			_animator.SetBool("Falling", false);
			_deadTimer = 0.0f;
			//_animan.enabled = true;
		} else{
			//_jumpingMove = _charCon.velocity*_speedScale;
			_animator.SetBool("Falling", true);
			_deadTimer += Time.deltaTime;
			//_animan.enabled = false;
		}

		if(_deadTimer >= _maxDeadTime){
			_dead = true;
		}

		if(_jump){

			if(Vector3.Angle(Vector3.up, _rayHit.normal) < 45){
				_jumpingMove.y -= _gravity*Time.deltaTime;
				_charCon.Move(_jumpingMove*Time.deltaTime);
			}

			//Debug.Log("Forward: " + transform.forward);
			//Debug.Log("JumpMove: " +_jumpingMove);

			Vector3 temp = new Vector3(_offsetX,_offsetY,_offsetZ);
			if(Time.time - _clock > _maxTime){
				Debug.Log("NO LONGER FALLING");
				if(Physics.SphereCast(transform.position + new Vector3(0,1,0), 0.3f + temp.y ,Vector3.down,out _rayHit, _rayLength)){	//Nuddat marken och kan hoppa igen
					Debug.DrawRay(transform.position + temp,Vector3.down,Color.blue,1 + temp.y,true);
					Debug.DrawRay(transform.position, _rayHit.transform.position);


			
		

					_animator.SetBool("Falling", false);

					//Debug.Log("Collided with "+ _rayHit.collider.name);
					if(!_rayHit.collider.name.Equals(this.name)){
						//transform.rigidbody.constraints &= ~ RigidbodyConstraints.FreezeRotationX|~RigidbodyConstraints.FreezeRotationZ;
						//Debug.Log ("hit something"); 
						//_startPosition = transform.position.y;
						_animator.SetBool("Jump", false);
						if(Vector3.Angle(Vector3.up, _rayHit.normal) < 45){
							_jump = false;
						}
						//_animator.SetBool("Jump", false);
						_animator.applyRootMotion = true;

						//rigidbody.constraints = ; 
						//_animan.enabled = true;


						/* TODO Plz ta bort desa två superdåliga rader kod, my bad */
						//gameObject.GetComponent<CharacterController>().enabled = true;
						//gameObject.GetComponent<CapsuleCollider>().enabled = false;

					}
				}
			}

		}
		////Debug.Log("cooldown is" + _cooldown);
		 

	}


	public bool isJumping(){
		return _jump;
	}

	public void Jumpy(){
		_jump = true;

	}

}

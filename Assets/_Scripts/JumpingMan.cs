using UnityEngine;
using System.Collections;

public class JumpingMan : MonoBehaviour {

	private Animator _animator;
	private CharacterController _charCon;
	public float _jumpForce = 300;
	public float _offsetX = 0;
	public float _offsetY = 0;
	public float _offsetZ = 0;
	float _clock;
	float _maxTime = 1f;
	//float _startPosition;
	bool _jump = false;
	RaycastHit _rayHit;
	//AnimationMan _animan;
	private Vector3 _jumpingMove = Vector3.zero;
	public float _gravity = 20.0f;
	public float _jumpSpeed = 8.0f;
	[Range(0.01f, 2.0f)]
	public float _speedScale = 0.6f;

	

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
	
		Debug.Log (_charCon.velocity);

		if(_animator.applyRootMotion == false){
			Debug.Log("NO MOTION FOR ME");
		}

		if(Input.GetButtonDown("Jump")){	//Aktiverar hoppet
			//Debug.Log("you pressed jump)");
			if(!_jump){
				//_animan.enabled = false;
				//transform.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ; //hindrar den från att snörra cp

				/* TODO Plz ta bort desa två superdåliga rader kod, my bad */
				//gameObject.GetComponent<CharacterController>().enabled = false;
				//gameObject.GetComponent<CapsuleCollider>().enabled = true;


				//Vector3 temp = rigidbody.velocity;
				//transform.rigidbody.velocity = new Vector3(temp.x,0,temp.z);
				//transform.rigidbody.AddForce(Vector3.up*_jumpForce);
				//Debug.Log("jumping");
				_jump = true;

				//gameObject.GetComponent<CapsuleCollider>().enabled = true;

				_jumpingMove = _charCon.velocity*_speedScale;
				_jumpingMove.y = _jumpSpeed;
				_animator.applyRootMotion = false;
				_animator.SetBool("Jump", true);
				_clock = Time.time;

			}

		}
	

		_animator.SetBool("Jump", false);
	/*	if(_jump && transform.position.y - _startPosition > _jumpHeight){	//Hoppat så högt den klarar, kör en cooldown
			////Debug.Log("cooldown");
			_cooldown = true;
		}
		
*/
		if(Physics.SphereCast(transform.position + new Vector3(0,1,0), 0.3f ,Vector3.down,out _rayHit,1.1f)){
			_animator.SetBool("Falling", false);
			//_animan.enabled = true;
		} else{
			_animator.SetBool("Falling", true);
			//_animan.enabled = false;
		}

		if(_jump){

			_jumpingMove.y -= _gravity*Time.deltaTime;
			_charCon.Move(_jumpingMove*Time.deltaTime);


			Vector3 temp = new Vector3(_offsetX,_offsetY,_offsetZ);
			if(Time.time - _clock > _maxTime){

				if(Physics.SphereCast(transform.position + new Vector3(0,1,0), 0.3f + temp.y ,Vector3.down,out _rayHit,1.1f)){	//Nuddat marken och kan hoppa igen
					Debug.DrawRay(transform.position + temp,Vector3.down,Color.blue,1 + temp.y,true);
					Debug.DrawRay(transform.position, _rayHit.transform.position);
					//Debug.Log("Collided with "+ _rayHit.collider.name);
					if(!_rayHit.collider.name.Equals(this.name)){
						//transform.rigidbody.constraints &= ~ RigidbodyConstraints.FreezeRotationX|~RigidbodyConstraints.FreezeRotationZ;
						//Debug.Log ("hit something"); 
						//_startPosition = transform.position.y;
						_jump = false;
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


}

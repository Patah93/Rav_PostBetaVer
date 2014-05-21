using UnityEngine;
using System.Collections;

public class FoxAI : MonoBehaviour {

	public FoxNode _targetNode;
	FoxNode _currentNode;

	ShadowDetection _shadowDetect;

	Animator _ani;

	GameObject _derp;

	bool _pathSafe;

	bool _testing = false;

	bool _fleeing = false;

	public bool _refuseMoveIfLight = false;

	public bool _controlled = false;

	public bool _moveWhenBoyIsClose = true;

	bool _boyClose = false;

	public float CHECK_LIGHT_INTERVAL = 0.625f;

	int _updateTick = 0;

	public int _sleep_updates_shadowDet = 8;

	Vector3 _direction;

	//Quaternion _desiredRotation;

	//BoxCollider _box;

	public float _fallAcc = 0.01f;

	private Vector3 _fallVec;

	private float _distanceToTurnPoint;
	
	private Vector2 _turnPoint;

	private bool _turning = false;

	private float _animationDirection = 0;

	[Range(2.0f, 8.0f)]
	public float _moveSpeed = 4.0f;

	[Range(1.0f, 3.0f)]
	public float _turnspeedFactor = 1.1f;

	[Range(1.0f, 30.0f)]
	public float  _defaultRotationSpeed = 7.0f;

	private float _rotationSpeed;

	[Range(0.25f, 3.0f)]
	public float _ANIMATION_CHANGE_SPEED = 1.0f;

	// Use this for initialization
	void Start () {

		_derp = new GameObject ();
		_derp.AddComponent<FoxNode> ();

		_currentNode = _derp.GetComponent<FoxNode> ();
		_currentNode._nextNode = _targetNode;

		_targetNode.setPrevNode(_currentNode);

		_currentNode.transform.position = gameObject.transform.position;
		_targetNode = null;

		_pathSafe = false;

		_shadowDetect = GetComponent<ShadowDetection>();

		//_desiredRotation = transform.rotation;

		//_box = GetComponent<BoxCollider>();

		_fallVec = Vector3.down * _fallAcc;

		_ani = GetComponent<Animator>();

		_rotationSpeed = _defaultRotationSpeed;
	}
	
	// Update is called once per frame
	void Update () {

		////Debug.DrawLine(transform.position + transform.forward * 0.22f + Vector3.up * 0.5f, transform.position + transform.forward * 0.22f + Vector3.up * 0.5f + Vector3.down * 1, Color.red);
		////Debug.DrawLine(transform.position - transform.forward * 0.22f + Vector3.up * 0.5f, transform.position - transform.forward * 0.22f + Vector3.up * 0.5f + Vector3.down * 1, Color.red);

		if (_targetNode != null) {
			if (reachedTarget()) {
				_turning = false;
				transform.position = new Vector3(_targetNode.transform.position.x, transform.position.y, _targetNode.transform.position.z);
				if(_targetNode._isWayPointNode && !_fleeing){
					if(_targetNode._nextNode != _currentNode){
						_currentNode = _targetNode;
						_targetNode = _currentNode._nextNode;
					}
					else{
						_currentNode = _targetNode;
						_targetNode = _currentNode._prevNode;
					}
				}else{
					if(_fleeing){
						transform.forward *= -1;
					}
					_fleeing = false;
					_currentNode = _targetNode;
					_targetNode = null;
					_pathSafe = false;

					_ani.SetBool("Walking", false);
				}
			} else {
				if(!_pathSafe && _refuseMoveIfLight){
					checkPathForShadows();
					//Debug.Log ("Checking path for shadows!");
				}
				if(_pathSafe || !_refuseMoveIfLight){
					move ();
					//Debug.Log ("Moving!");
				}
				else if(!_pathSafe){
					/* TODO Hantera "Oh no mr Boy, me no can walk!" */
					_targetNode = null;
				}
			}
		} else {
			if(_controlled){
				if (Input.GetButtonDown ("FoxForward") || Input.GetAxis("FoxCall") > 0){
					if (_currentNode._nextNode != null && !_currentNode._isWaitingForAction) {
						_targetNode = _currentNode._nextNode;
						if(_currentNode._turnSpeed == 0.0f){
							_rotationSpeed = _defaultRotationSpeed;
						}else{
							_rotationSpeed = _currentNode._turnSpeed;
						}
						_ani.SetBool("Walking", true);
					}
				}
				if (Input.GetButtonDown ("FoxBackward") || Input.GetAxis("FoxCall") < 0) {
					if (_currentNode != null) {
						_targetNode = _currentNode._prevNode;
						if(_targetNode._turnSpeed == 0.0f){
							_rotationSpeed = _defaultRotationSpeed;
						}else{
							_rotationSpeed = _targetNode._turnSpeed;
						}
						_ani.SetBool("Walking", true);
					}
				}
			}

			/* Fall-kod */

			RaycastHit rayInfoFront, rayInfoBack;

			Vector3 frontFeetRayPoint = transform.position + transform.forward * 0.22f + Vector3.up * 0.5f;
			Vector3 backFeetRayPoint = transform.position - transform.forward * 0.22f + Vector3.up * 0.5f;
			
			bool frontCast = Physics.Raycast(frontFeetRayPoint, Vector3.down, out rayInfoFront, 1.0f);
			bool backCast = Physics.Raycast(backFeetRayPoint, Vector3.down, out rayInfoBack, 1.0f);
			
			if(!(frontCast || backCast)){
				/* TODO HANDLE BOTH FEET IN DAT AIR! */
				//////Debug.Log("I'M FLYYING :D");
				_fallVec += Vector3.down * _fallAcc;
				transform.position += _fallVec;
			}else{
				_fallVec = Vector3.down * _fallAcc;
			}

			if(Physics.Raycast(transform.position + transform.forward * 0.22f + Vector3.up * 0.5f, Vector3.down, out rayInfoFront, 1.0f)){
				Vector3 yOffset = new Vector3(0, -(rayInfoFront.distance - 0.5f), 0);
				transform.position += yOffset;
			}
			else if(Physics.Raycast(transform.position - transform.forward * 0.22f + Vector3.up * 0.5f, Vector3.down, out rayInfoBack, 1.0f)){
				Vector3 yOffset = new Vector3(0, -(rayInfoBack.distance - 0.5f), 0);
				transform.position += yOffset;
			}
		}
	}

	void FixedUpdate(){

		/* TODO other points of interest in ShadowDet-Script
			 * 	kolla olika punkter varje updateTick, basera på %
			 *	istället för _updateTick == 0 bajset... 
			 */
		if(_updateTick == 0 && !_testing){

			if(_shadowDetect.isObjectInLightMorePoints()){
				////Debug.Log("DIEDIEDIEDIE POOR FOXIE! >='[");

				if(!_fleeing){
					if(_targetNode == null && _currentNode._prevNode != null){
						_targetNode = _currentNode._prevNode;
						_pathSafe = true;
						if(_targetNode._turnSpeed == 0.0f){
							_rotationSpeed = _defaultRotationSpeed;
						}else{
							_rotationSpeed = _targetNode._turnSpeed;
						}
						_ani.SetBool("Walking", true);
					}else{
						_targetNode = _currentNode;
					}
					_fleeing = true;

					/* TODO Ta bort när det finns en fin snabb rotation */
					transform.forward *= -1;
				}
			}

			if(_targetNode == null && !_currentNode._isWaitingForAction && !_controlled && (!_moveWhenBoyIsClose || _boyClose)){
				_targetNode = _currentNode._nextNode;
				if(_currentNode._turnSpeed == 0.0f){
					_rotationSpeed = _defaultRotationSpeed;
				}else{
					_rotationSpeed = _currentNode._turnSpeed;
				}
				if(_targetNode != null && !_refuseMoveIfLight){
					_ani.SetBool("Walking", true);
				}
			}
		}

		_updateTick++;
		_updateTick %= _sleep_updates_shadowDet;
	}

	bool reachedTarget(){
		/* Magi */
		return (new Vector2(transform.position.x, transform.position.z) - new Vector2(_targetNode.transform.position.x, _targetNode.transform.position.z)).sqrMagnitude < 0.1f;
	}

	void checkPathForShadows(){
		_testing = true;

		Vector3 originalPos = transform.position;
		Quaternion originalRotation = transform.rotation;
		Vector3 lastCheckPos = transform.position - new Vector3(100, 100, 100);
		Vector3 prevPos = originalPos;

		//int count = 0;
		while(!reachedTarget()){
			move ();
			if((transform.position - lastCheckPos).sqrMagnitude > CHECK_LIGHT_INTERVAL){
				if(_shadowDetect.isObjectInLight()){
					_pathSafe = false;
					transform.position = originalPos;
					transform.rotation = originalRotation;
					//_desiredRotation = originalRotation;
					_testing = false;
					return;
				}
				lastCheckPos = transform.position;
				//count++;
			}
			else if((transform.position - prevPos).sqrMagnitude < float.Epsilon){ /* BILLIGARE ÄN == 0 ?? */
				_pathSafe = false;
				transform.position = originalPos;
				transform.rotation = originalRotation;
				//_desiredRotation = originalRotation;
				_testing = false;
				return;
			}
			prevPos = transform.position;
		}
		
		transform.position = originalPos;
		transform.rotation = originalRotation;
		//_desiredRotation = originalRotation;
		//////Debug.Log(count + ": lightchecks!"); 

		_pathSafe = true;
		_ani.SetBool("Walking", true);
		_testing = false;

		//Debug.Log ("TESTAR VÄGEN EFTER SKUGGOR");
	}

	void move(){

		//////Debug.Log("HERP");

		if ((_currentNode._isTeleportNode && !_fleeing) || (_fleeing && _targetNode._isTeleportNode)) {
			transform.position = _targetNode.transform.position;

			if(!_fleeing){
				transform.forward = (_targetNode._nextNode.transform.position - _targetNode.transform.position).normalized;
			}
			else{
				transform.forward = (_targetNode._prevNode.transform.position - _targetNode.transform.position).normalized;
			}
			transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
			return;
		}

		RaycastHit rayInfoFront, rayInfoBack;

		if (!_turning && ((_currentNode._isTurningNode && _targetNode == _currentNode._nextNode) || (_targetNode._isTurningNode && _currentNode._prevNode == _targetNode))) {

			_turning = true;

			Vector2 toNextNode = new Vector2(_targetNode.transform.position.x, _targetNode.transform.position.z) - new Vector2(_currentNode.transform.position.x, _currentNode.transform.position.z);

			float angle = Vector2.Angle (new Vector2(transform.right.x, transform.right.z).normalized, toNextNode.normalized);

			Vector2 turnDir = new Vector2(transform.right.x, transform.right.z).normalized;

			if(angle > 90){
				turnDir *= -1;
				angle = Vector2.Angle (turnDir, toNextNode.normalized);
			}

			//Debug.DrawLine (transform.position, transform.position + new Vector3(turnDir.x, transform.position.y, turnDir.y), Color.red, 20.0f);

			//Debug.DrawLine (transform.position, transform.position + new Vector3(toNextNode.x, transform.position.y, toNextNode.y), Color.cyan, 20.0f);

			_distanceToTurnPoint = toNextNode.magnitude/(Mathf.Cos((angle * Mathf.PI)/180.0f))/2.0f;

			_turnPoint = new Vector2(_currentNode.transform.position.x, _currentNode.transform.position.z) + turnDir * _distanceToTurnPoint;

			//Debug.Log (_turnPoint + ": is turnpoint" );
		} 

		if (!_turning) {
			/*
			_direction = new Vector3 ((_targetNode.transform.position - transform.position).x, 0, (_targetNode.transform.position - transform.position).z);
			_direction.Normalize ();

			transform.rotation = Quaternion.LookRotation (_direction);
			*/

			Vector2 targetDirection = new Vector2 ((_targetNode.transform.position - transform.position).x, (_targetNode.transform.position - transform.position).z).normalized;

			float angle = Vector2.Angle (new Vector2(transform.right.x, transform.right.z).normalized, targetDirection);
			if(angle < 89){

				transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + _rotationSpeed, transform.localEulerAngles.z);
				setAnimationDirection(1);

				if(Vector2.Angle (new Vector2(transform.right.x, transform.right.z).normalized, targetDirection) >= 88){
					transform.rotation = Quaternion.LookRotation (new Vector3(targetDirection.x, 0, targetDirection.y).normalized);
					setAnimationDirection(0);
				}
				_direction = transform.forward;

			}else if(angle > 91){
				transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y - _rotationSpeed, transform.localEulerAngles.z);
				setAnimationDirection(-1);

				if(Vector2.Angle (new Vector2(transform.right.x, transform.right.z).normalized, targetDirection) <= 90){
					transform.rotation = Quaternion.LookRotation (new Vector3(targetDirection.x, 0, targetDirection.y).normalized);
					setAnimationDirection(0);
				}
				_direction = transform.forward;
			}
			else{
				_direction = new Vector3(targetDirection.x, 0, targetDirection.y).normalized;
				transform.rotation = Quaternion.LookRotation (_direction);

				setAnimationDirection(0);
			}
		}

		Vector3 frontFeetRayPoint = transform.position + transform.forward * 0.22f + Vector3.up * 0.5f;
		Vector3 backFeetRayPoint = transform.position - transform.forward * 0.22f + Vector3.up * 0.5f;

		bool frontCast = Physics.Raycast(frontFeetRayPoint, Vector3.down, out rayInfoFront, 1.0f);
		bool backCast = Physics.Raycast(backFeetRayPoint, Vector3.down, out rayInfoBack, 1.0f);

		if(!(frontCast || backCast)){
				/* TODO HANDLE BOTH FEET IN DAT AIR! */
				//////Debug.Log("I'M FLYYING :D");

			_fallVec += Vector3.down * _fallAcc;
			transform.position += _fallVec;
			transform.position += transform.forward * 2.0f * Time.deltaTime;
		}
		else{

			_fallVec = Vector3.down * _fallAcc;

			//////Debug.Log("I'M GROUNDED D:");

			if(frontCast && backCast){

				
				if(_turning){
					transform.position += transform.forward * Time.deltaTime * _turnspeedFactor;

					Vector2 turnPointToPosition = (new Vector2(transform.position.x, transform.position.z) - _turnPoint).normalized;
					transform.position = new Vector3(_turnPoint.x, transform.position.y, _turnPoint.y) + (new Vector3(turnPointToPosition.x, 0, turnPointToPosition.y) * _distanceToTurnPoint);
					
					Vector2 rightAngleVec = new Vector2(-1 * turnPointToPosition.y, turnPointToPosition.x);

					if(Vector2.Angle (rightAngleVec, new Vector2(transform.forward.x, transform.forward.z).normalized) > 90){
						rightAngleVec *= -1;
						setAnimationDirection(1);
					}else{
						setAnimationDirection(-1);
					}
					
					_direction = new Vector3(rightAngleVec.x, 0, rightAngleVec.y).normalized;

					transform.rotation = Quaternion.LookRotation (_direction);

					frontFeetRayPoint = transform.position + transform.forward * 0.22f + Vector3.up * 0.5f;
					backFeetRayPoint = transform.position - transform.forward * 0.22f + Vector3.up * 0.5f;
					
					frontCast = Physics.Raycast(frontFeetRayPoint, Vector3.down, out rayInfoFront, 1.0f);
					backCast = Physics.Raycast(backFeetRayPoint, Vector3.down, out rayInfoBack, 1.0f);
				}

				float angle = Vector3.Angle(_direction, (rayInfoFront.point - rayInfoBack.point).normalized);

				if(rayInfoFront.distance > rayInfoBack.distance){
					transform.rotation = Quaternion.Euler(transform.localEulerAngles.x + angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
					//////Debug.Log("LUTNING! =D");
				}
				else if(rayInfoFront.distance < rayInfoBack.distance){
					transform.rotation = Quaternion.Euler(transform.localEulerAngles.x - angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
					//////Debug.Log("LUTNING! =D");
				}else{
					transform.rotation = Quaternion.Euler(transform.localEulerAngles.x + angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
				}
			}
			/*
			Physics.Raycast(frontFeetRayPoint, Vector3.down, out rayInfoFront, 1.0f);

			Vector3 yOffset = new Vector3(0, -(rayInfoFront.distance - 0.5f), 0);

			transform.position += yOffset + transform.forward * 4 * Time.deltaTime;
			*/
			transform.position += transform.forward * _moveSpeed * Time.deltaTime;

		}	
		
		if(Physics.Raycast(transform.position + transform.forward * 0.22f + Vector3.up * 0.5f, Vector3.down, out rayInfoFront, 1.0f)){
			Vector3 yOffset = new Vector3(0, -(rayInfoFront.distance - 0.5f), 0);
			transform.position += yOffset;
		}
		else if(Physics.Raycast(transform.position - transform.forward * 0.22f + Vector3.up * 0.5f, Vector3.down, out rayInfoBack, 1.0f)){
			Vector3 yOffset = new Vector3(0, -(rayInfoBack.distance - 0.5f), 0);
			transform.position += yOffset;
		}
	}

	public void setBoyClose(bool boyClose){
		_boyClose = boyClose;
	}

	private void setAnimationDirection(float direction){
		if(_animationDirection > direction){
			_animationDirection -= _ANIMATION_CHANGE_SPEED * Time.deltaTime;
			if(_animationDirection < direction){
				_animationDirection = direction;
			}
		}
		else if(_animationDirection < direction){
			_animationDirection += _ANIMATION_CHANGE_SPEED * Time.deltaTime;
			if(_animationDirection > direction){
				_animationDirection = direction;
			}
		}

		_ani.SetFloat("Direction", _animationDirection);
	}
}

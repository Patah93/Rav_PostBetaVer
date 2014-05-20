using UnityEngine;
using System.Collections;

/*
 * Made by Tobias Tevemark
 * 2014-04-29
 * 
 * There’s a certain amount of craftsmanship involved in making things move around algorithmically.  
 * Getting a camera to slide smoothly into position, gliding on the gossamer wings of math, can be a perilous undertaking.
 * One wrong wobble, one tiny pop, and every pixel on the screen becomes wrong, the needle slides off the record, and the magic spell is broken.
 * - Jeff Farris - Epic Games
 */

public class ThirdPersonCamPatrikFailEdition : MonoBehaviour {
	#region Public variables
	
	//Camera posistion and look at variables
	public Vector3 Offset = Vector3.zero;
	public Transform LookAtNormal;
	public Transform LookAtInside;
	[Range(0.0f, 5.0f)]
	public float CameraUp = 1.0f;
	[Range(0.0f, 5.0f)]
	public float CameraAway = 3.0f;
	[Range(0.0f, 5.0f)]
	public float ThorowCameraUp = 0.0f;
	[Range(0.0f, 5.0f)]
	public float ThrowCameraAway = 1.5f;
	[Range(0.0f, 5.0f)]
	public float ThrowCameraShoulderOffset = 1.0f;
	
	[Range(0.0f, 5.0f)]
	public float insideCameraUp = .0f;
	[Range(0.0f, 5.0f)]
	public float insideCameraAway = 1.5f;
	
	//Camera max movement delta (Low value to create a moothing effect)
	[Range(0.0f, 2.0f)]
	public float camSmoothDampTme = 0.2f;
	
	//Controller Deadzone for rotating the camera. We only want to rotate camera when we move the stick far enough.
	//values from 0 to 1
	[Range(0.0f, 1.0f)]
	public float deadZoneX = 0.3f;
	[Range(0.0f, 1.0f)]
	public float deadZoneY = 0.3f;
	
	
	//Controller variables for rotation speed etc
	[Range(1.0f, 200.0f)]
	public float RotationSpeedX = 100.0f;
	[Range(1.0f, 200.0f)]
	public float RotationSpeedY = 100.0f;
	public bool InvertedX = true;
	public bool InvertedY = false;
	
	//Clamping values for Y axis so we can't go to far up or down.
	public Vector2 cameraClampingY = new Vector2(-40.0f, 60.0f);
	
	//Camera State
	public CamStates camState = CamStates.Behind;
	
	//Starting moving behind after set time.
	[Range(0.0f, 2.0f)]
	public float moveBehind = 0.5f;
	//What smoothing time do you want for that movement.
	[Range(20.0f, 100.0f)]
	public float autoMoveSmooth = 80.0f;
	
	//Camera compenstation values
	[Range(0.0f, 1.0f)]
	public float ScalingNormalCompenstation = 1.0f;
	[Range(0.0f, 5.0f)]
	public float ScalingComenstationUpMovement = 1.0f;


	[Range(0.0f, 2.0f)]
	public float _wallDistance = 0.35f;
	[Range(1.0f, 5.0f)]
	public float _pushDistanceFactorY = 3.0f;
	[Range(1.0f, 3.0f)]
	public float _pushDistanceFactorXZ = 1.5f;
	#endregion
	
	#region Private variables
	
	//Reference to the throw scripts to get the higest point in the aiming arc
	private Throw referenceToThrow;
	
	//what is our current look direction
	Vector3 currentLookDirection;
	
	//Target posistion
	private Vector3 targetPosistion;
	
	//TODO : Change to corresponds to correct throw posistion later
	//The first Person Camera Posistion
	private CameraPosistion FirstPersonCameraPosistion;
	
	//The behind Person Camera Posistion
	private CameraPosistion BehindPersonCameraPosistion;
	
	//Reference to the characters transform
	private Transform PlayerXform;
	
	//TODO : add other controller input values
	//The controller input values
	private float rightX = 0.0f;
	private float rightY = 0.0f;
	private float leftX = 0.0f;
	private float leftY = 0.0f;
	
	//private containers holding the amount of rotation around the character that have been applied from the controller input
	private float rotationAmountY = 0.0f;
	private float rotationAmountX = 0.0f;
	
	//time since last input
	private float deltaLastInput = 0.0f;
	//do we need to start moving?
	private bool startMoving = false;
	
	//what was our previus camState?
	private CamStates prevCamstate = CamStates.Behind;
	public CamStates PrevCamstate { get { return this.prevCamstate; } set { this.prevCamstate = value; } }
	
	Vector3 followerVelocity;

	private Transform _obj = null;
	private Vector3 _pushDir;
	private bool _exitPushMode = false;
	#endregion
	
	#region Structs
	struct CameraPosistion
	{
		//Posistion
		private Vector3 posistion;
		//Transform of object
		private Transform xForm;
		
		//getters and setters
		public Vector3 Posistion { get { return posistion; } set { posistion = value; } }
		public Transform XForm { get { return xForm; } set { xForm = value; } }
		
		//Init
		public void Init(string camName, Vector3 pos, Transform transform, Transform parent)
		{
			posistion = pos;
			xForm = transform;
			xForm.name = camName;
			xForm.parent = parent;
			xForm.localPosition = Vector3.zero;
			xForm.localPosition = posistion;
		}
	}
	#endregion
	
	#region Enums
	public enum CamStates
	{
		Behind,
		FirstPerston,
		Target,
		Free,
		Throw,
		Inside,
		Push
	}
	#endregion
	
	#region Inits
	//Called even if script component is not enabled
	//best used for references between scripts and Inits
	void Awake()
	{
		//grabbing the transform from the character.
		PlayerXform = GameObject.FindWithTag("Player").transform;
		
		//Init out look direction to correspond where the character is looking at i.e it's forward vector.
		currentLookDirection = PlayerXform.forward;
		
		
		//grabbing the reference to the throw component.
		referenceToThrow = GameObject.FindWithTag("Player").GetComponent<Throw>();
	}
	
	//Called if script component is enabled
	void Start()
	{
		
	}
	#endregion
	
	#region Update funtions
	void Update()
	{
		
		//We need to update the players transform so we always have the correct values.
		PlayerXform = GameObject.FindWithTag("Player").transform;
		
		//TODO : add more controller input grabs
		//We need to grab the controller input values
		rightX = Input.GetAxis("RightStickHorizontal");
		rightY = Input.GetAxis("RightStickVertical");
		leftX = Input.GetAxis("Horizontal");
		leftY = Input.GetAxis("Vertical");
		
		//check for inputs so the camera does not auto move if we've not used the right stick
		//TODO Needs to check for all inputs!
		if (Mathf.Abs(leftX) >= 0.1 && Mathf.Abs(rightX) == 0.0 && Mathf.Abs(rightY) == 0.0)
		{
			deltaLastInput += Time.deltaTime;
		}
		else
			deltaLastInput = 0;
		
		if (deltaLastInput >= moveBehind)
			startMoving = true;
		else
			startMoving = false;
	}
	void LateUpdate()
	{
		
		//Here we check what camera  state we are actually in.
		switch (camState)
		{
			//If we are in the default camera state
		case CamStates.Behind:
			
			//saving the rotation amount
			if (Mathf.Abs(rightX) > deadZoneX)
				rotationAmountX += rightX * Time.deltaTime * RotationSpeedX * ((InvertedX == true) ? -1 : 1);
			if (Mathf.Abs(rightY) > deadZoneY)
				rotationAmountY += rightY * Time.deltaTime * RotationSpeedY * ((InvertedY == true) ? -1 : 1);
			
			//clamping Y rotation
			rotationAmountY = Mathf.Clamp(rotationAmountY, cameraClampingY.x, cameraClampingY.y);
			//clamping X rotation
			if (Mathf.Abs(rotationAmountX) > 360.0f)
				rotationAmountX = 0.0f;
			
			//addding the rotations
			currentLookDirection = Quaternion.Euler(rotationAmountY, rotationAmountX, 0.0f) * Vector3.forward;
			
			//now if we are not directly in front of the character we want to slowly move behind the character.
			//if the conditon for start moving is met.
			if (!(Vector3.Dot(PlayerXform.forward, this.transform.forward) <= -0.8f))
			{
				if (startMoving)
				{
					rotationAmountX += (Vector3.Angle(-PlayerXform.right, this.transform.forward) > 90 ? -1.0f : 1.0f) * Time.deltaTime * autoMoveSmooth;
				}
			}
			if ((Vector3.Dot(PlayerXform.forward, this.transform.forward) >= 1.0f))
			{
				startMoving = false;
				deltaLastInput = 0;
			}
			
			targetPosistion =
				//moving target pos up according to CameraUp variable 
				(LookAtNormal.position + (Vector3.Normalize(PlayerXform.up) * CameraUp)) -
					//move the target a bit back according to the CameraAway variable
					(Vector3.Normalize(currentLookDirection) * CameraAway);
			
			
			CompenstaForWalls(LookAtNormal.position, ref targetPosistion);

			//Vector3 pos = transform.position;
			//CompenstaForWalls(LookAtNormal.position, ref pos);
			//transform.position = pos;

			smoothPosistion(this.transform.position, targetPosistion);
			
			transform.LookAt(LookAtNormal);
			break;
		case CamStates.Inside:
			
			//saving the rotation amount
			if (Mathf.Abs(rightX) > deadZoneX)
				rotationAmountX += rightX * Time.deltaTime * RotationSpeedX * ((InvertedX == true) ? -1 : 1);
			if (Mathf.Abs(rightY) > deadZoneY)
				rotationAmountY += rightY * Time.deltaTime * RotationSpeedY * ((InvertedY == true) ? -1 : 1);
			
			//clamping Y rotation
			rotationAmountY = Mathf.Clamp(rotationAmountY, cameraClampingY.x, cameraClampingY.y);
			//clamping X rotation
			if (Mathf.Abs(rotationAmountX) > 360.0f)
				rotationAmountX = 0.0f;
			
			//addding the rotations
			currentLookDirection = Quaternion.Euler(rotationAmountY, rotationAmountX, 0.0f) * Vector3.forward;
			
			//now if we are not directly in front of the character we want to slowly move behind the character.
			//if the conditon for start moving is met.
			if (!(Vector3.Dot(PlayerXform.forward, this.transform.forward) <= -0.8f))
			{
				if (startMoving)
				{
					rotationAmountX += (Vector3.Angle(-PlayerXform.right, this.transform.forward) > 90 ? -1.0f : 1.0f) * Time.deltaTime * autoMoveSmooth;
				}
			}
			if ((Vector3.Dot(PlayerXform.forward, this.transform.forward) >= 1.0f))
			{
				startMoving = false;
				deltaLastInput = 0;
			}
			
			targetPosistion =
				//moving target pos up according to CameraUp variable 
				(LookAtInside.position + (Vector3.Normalize(PlayerXform.up) * insideCameraUp)) -
					//move the target a bit back according to the CameraAway variable
					(Vector3.Normalize(currentLookDirection) * insideCameraAway);
			
			
			CompenstaForWalls(LookAtInside.position, ref targetPosistion);
			smoothPosistion(this.transform.position, targetPosistion);
			
			transform.LookAt(LookAtInside.position);
			break;
		case CamStates.Throw:
			//find the lookAt posistion
			float pitch = 0.0f;
			//find the top point in throw arc and save location
			
			
			//grabbing the higest pos form the throw arc
			Vector3 higestpos = referenceToThrow.highestPos;
			//Vector3 higestpos = Vector3.zero;
			//Debug.Log(higestpos);
			
			//angle between the camera to the target
			pitch = Vector3.Dot(Vector3.Normalize(higestpos - this.transform.position), PlayerXform.transform.forward);
			pitch = Mathf.Acos(pitch) * Mathf.Rad2Deg;
			//Debug.Log(pitch);
			
			//set camerea to right pos
			targetPosistion =
				//set it to be at the appropiate pos for over the shoulder.
				LookAtNormal.position + PlayerXform.up * ThorowCameraUp -
					PlayerXform.forward * ThrowCameraAway - PlayerXform.right * ThrowCameraShoulderOffset;
			
			//compensate for ze walls
			CompenstaForWalls(LookAtNormal.position, ref targetPosistion);
			//set the target to be smoothed
			smoothPosistion(this.transform.position, targetPosistion);
			//set the new lookAt
			transform.LookAt(higestpos);
			break;

		case CamStates.Push:
			
			//saving the rotation amount
			if (Mathf.Abs(rightX) > deadZoneX)
				rotationAmountX += rightX * Time.deltaTime * RotationSpeedX * ((InvertedX == true) ? -1 : 1);
			if (Mathf.Abs(rightY) > deadZoneY)
				rotationAmountY += rightY * Time.deltaTime * RotationSpeedY * ((InvertedY == true) ? -1 : 1);
			
			//clamping Y rotation
			rotationAmountY = Mathf.Clamp(rotationAmountY, cameraClampingY.x, cameraClampingY.y);
			//clamping X rotation
			if (Mathf.Abs(rotationAmountX) > 360.0f)
				rotationAmountX = 0.0f;
			
			//addding the rotations
			currentLookDirection = Quaternion.Euler(rotationAmountY, rotationAmountX, 0.0f) * Vector3.forward;

			Vector3 lookPos = _obj.transform.position + (PlayerXform.position - _obj.transform.position)/2.0f + _obj.collider.bounds.extents.y*0.985f*Vector3.up; 
			
			targetPosistion =
				//moving target pos up according to CameraUp variable 
				(lookPos + (Vector3.Normalize(PlayerXform.up) * CameraUp * _pushDistanceFactorY)) -
					//move the target a bit back according to the CameraAway variable
					(Vector3.Normalize(currentLookDirection) * CameraAway * _pushDistanceFactorXZ);
			
			
			CompenstaForWalls(lookPos, ref targetPosistion);
			
			smoothPosistion(this.transform.position, targetPosistion);
			
			transform.LookAt(lookPos);

			if (Input.GetButtonDown("Interact")){
				if(_exitPushMode){
					camState = prevCamstate;
					_exitPushMode = false;
				}else{
					_exitPushMode = true;
				}

			}

			break;
		}
		
		
	}
	void FixedUpdate()
	{
		
	}
	#endregion
	
	#region Private functions
	private void smoothPosistion(Vector3 fromPos, Vector3 toPos) {
		this.transform.position = Vector3.SmoothDamp (fromPos, toPos, ref followerVelocity, camSmoothDampTme);
		//this.transform.position = Vector3.Lerp(fromPos,toPos,10f * Time.renderedFrameCount);
	}
	
	Vector3 SuperSmoothLerp( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float time, float speed ) {
		Vector3 f = pastPosition - pastTargetPosition + (targetPosition - pastTargetPosition) / (speed * time);
		return targetPosition - (targetPosition - pastTargetPosition) / (speed*time) + f * Mathf.Exp(-speed*time);
	}
	private void CompenstaForWalls(Vector3 fromObject, ref Vector3 toTarget) {
		//Debug.DrawLine (toTarget,Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,Camera.main.nearClipPlane)));
		//Debug.DrawLine (toTarget, Camera.main.ViewportToWorldPoint (new Vector3 (1.0f, 0.5f, Camera.main.nearClipPlane)),Color.red);
		RaycastHit wallHit = new RaycastHit();
		
		//Vector3 offset = Vector3.zero;

		float _wallDistance = 0.25f;

		if (Physics.Linecast(fromObject, toTarget, out wallHit)) {
			//if(wallHit.distance >= 0.5f){
				Vector3 toPlayer = (wallHit.point - toTarget).normalized; 
				float angle = 90.0f - Vector3.Angle(toPlayer, wallHit.normal);
				if(angle < 0.1f){
					toTarget = wallHit.point + toPlayer * _wallDistance;
				}else{
					toTarget = wallHit.point + toPlayer * (_wallDistance/Mathf.Sin((angle * Mathf.PI)/180.0f));
				}
			//}else{
				//toTarget = transform.position;
			//}
		}

		/*
		//left
		if(Physics.Linecast(this.transform.position,Camera.main.ViewportToWorldPoint(new Vector3(0.0f,0.5f,Camera.main.nearClipPlane)),out wallHit)) {
			offset += wallHit.normal;
			//Debug.Log("Left");
		}
		//right
		if(Physics.Linecast(this.transform.position,Camera.main.ViewportToWorldPoint(new Vector3(1.0f,0.5f,Camera.main.nearClipPlane)),out wallHit)) {
			offset += wallHit.normal;
			//Debug.Log("Right");
		}
		//down
		if(Physics.Linecast(this.transform.position,Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.0f,Camera.main.nearClipPlane)),out wallHit)) {
			offset += wallHit.normal;
			//Debug.Log("down");
		}
		//up
		if(Physics.Linecast(this.transform.position,Camera.main.ViewportToWorldPoint(new Vector3(0.5f,1.0f,Camera.main.nearClipPlane)),out wallHit)) {
			offset += wallHit.normal;
			//Debug.Log("Up");
		}
		*/
		
		//toTarget = toTarget + offset.normalized * ScalingNormalCompenstation;
	}
	#endregion
	
	#region Public functions

	public void setPushMode(ref Transform obj){
		camState = CamStates.Push;
		_obj = obj;
		_pushDir = PlayerXform.forward;
	}

	public void setCameraState(string s){
		if(s.Equals("Throw"))
			camState = (camState != CamStates.Throw) ? CamStates.Throw : prevCamstate;
	}

	#endregion
}


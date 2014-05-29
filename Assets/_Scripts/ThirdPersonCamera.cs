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

public class ThirdPersonCamera : MonoBehaviour {
	#region Public variables
	
	//Camera posistion and look at variables
	public Transform LookAt;
	[Range(0.0f, 5.0f)]
	public float CameraUp = 1.0f;
	[Range(0.0f, 5.0f)]
	public float CameraAway = 3.0f;
	[Range(0.0f, 5.0f)]
	public float ThrowCameraUp = 0.0f;
	[Range(0.0f, 5.0f)]
	public float ThrowCameraAway = 1.5f;
	[Range(0.0f, 5.0f)]
	public float ThrowCameraShoulderOffset = 1.0f;
	[Range(0.0f, 5.0f)]
	public float FocusCameraAway = 3.0f;

	//Camera max movement delta (Low value to create a moothing effect)
	[Range(1.0f, 20.0f)]
	public float camSmoothDampTme = 10.0f;
	
	//Controller Deadzone for rotating the camera. We only want to rotate camera when we move the stick far enough.
	//values from 0 to 1
	[Range(0.0f, 1.0f)]
	public float deadZoneX = 0.3f;
	[Range(0.0f, 1.0f)]
	public float deadZoneY = 0.3f;
	
	
	//Controller variables for rotation speed etc
	[Range(1.0f, 200.0f)]
	public float RotationSpeedX = 150.0f;
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
	[Range(0.0f, 0.3f)]
	public float ScalingNormalCompenstation = 0.1f;
	[Range(0.0f, 1.0f)]
	public float ScalingComenstationUpMovement = 0.0f;
	
	//Push mode variables
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
	
	//Push mode variables
	private Transform _obj = null;
	private Vector3 _pushDir;
	private bool _exitPushMode = false;

	//bug fix for "clone" player objects
	GameObject[] playerObjects;

	private Transform focusTarget;
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
		Throw,
		Inside,
		Push,
		Focus
	}
	#endregion
	
	#region Inits
	//Called even if script component is not enabled
	//best used for references between scripts and Inits
	void Awake()
	{
		playerObjects = GameObject.FindGameObjectsWithTag("Player");
		//grabbing the transform from the character.
		PlayerXform = GameObject.FindWithTag("Player").transform;
		
		//Init out look direction to correspond where the character is looking at i.e it's forward vector.
		currentLookDirection = PlayerXform.forward;
		
		
		//grabbing the reference to the throw component.
		referenceToThrow = GameObject.FindWithTag("Player").GetComponent<Throw>();
		
		//setting near clip plane.
		Camera.main.nearClipPlane = 0.1f;
	}
	
	//Called if script component is enabled
	void Start()
	{
		
	}
	#endregion
	
	#region Update funtions
	void Update()
	{
		//if (Input.GetButtonDown("Aim"))
			//camState = (camState != CamStates.Throw) ? CamStates.Throw : prevCamstate;
		
		//TODO : add more controller input grabs
		//We need to grab the controller input values
		rightX = Mathf.Clamp(Input.GetAxis("RightStickHorizontal"), -1, 1);
		rightY = Mathf.Clamp(Input.GetAxis("RightStickVertical"), -1, 1);
		leftX = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
		leftY = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);
		
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

			//player clone bug fix
			if(playerObjects.Length > 1)
			for(int i = 0; i < playerObjects.Length; i++)
				if(playerObjects[i].name == "0 HajPojken(Clone)")
					Destroy(playerObjects[i]);

			if(startMoving)
			if(Vector3.Dot(playerObjects[0].transform.forward,this.transform.forward) < .8f)
				if(Vector3.Angle(playerObjects[0].transform.right, this.transform.forward) < 90)
						rotationAmountX -= Time.deltaTime * autoMoveSmooth;
				else if(Vector3.Angle(playerObjects[0].transform.right, this.transform.forward) > 90)
						rotationAmountX += Time.deltaTime * autoMoveSmooth;

			if ((Vector3.Dot(playerObjects[0].transform.forward, this.transform.forward) >= .8f))
			{
				startMoving = false;
				deltaLastInput = 0;
			}

			
			targetPosistion =
				//moving target pos up according to CameraUp variable 
				(LookAt.position + (Vector3.Normalize(PlayerXform.up) * CameraUp)) -
					//move the target a bit back according to the CameraAway variable
					(Vector3.Normalize(currentLookDirection) * CameraAway);
			
			
			CompenstaForWalls(LookAt.position, ref targetPosistion);
			smoothPosistion(this.transform.position, targetPosistion);
			
			transform.LookAt(LookAt);
			break;
		case CamStates.Throw:
			//find the lookAt posistion
			float pitch = 0.0f;
			//find the top point in throw arc and save location
			
			
			//grabbing the higest pos form the throw arc
			Vector3 higestpos = referenceToThrow.highestPos;			

			//angle between the camera to the target
			pitch = Vector3.Dot(Vector3.Normalize(higestpos - this.transform.position), PlayerXform.transform.forward);
			pitch = Mathf.Acos(pitch) * Mathf.Rad2Deg;
			
			//set camerea to right pos
			targetPosistion =
				//set it to be at the appropiate pos for over the shoulder.
				LookAt.position + PlayerXform.up * ThrowCameraUp -
					PlayerXform.forward * ThrowCameraAway - PlayerXform.right * ThrowCameraShoulderOffset;
			
			//compensate for ze walls
			CompenstaForWalls(LookAt.position, ref targetPosistion);
			//set the target to be smoothed
			smoothPosistion(this.transform.position, targetPosistion);
			//set the new lookAt
			higestpos = new Vector3(higestpos.x, higestpos.y - (higestpos.y - PlayerXform.position.y)/2.0f, higestpos.z);
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
			
			if (PlayerXform.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") || PlayerXform.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Run")){
				if(_exitPushMode){
					camState = prevCamstate;
					_exitPushMode = false;
				}else{
					_exitPushMode = true;
				}			
			}
			break;
		case CamStates.Focus:

			currentLookDirection = Quaternion.Euler(0.0f, 0.0f, 0.0f) * Vector3.forward;

			targetPosistion =
				(focusTarget.position) + 
				//move the target a bit back according to the CameraAway variable
					(focusTarget.transform.forward * FocusCameraAway);

			CompenstaForWalls(focusTarget.position, ref targetPosistion);
			smoothPosistion(this.transform.position, targetPosistion);
			transform.LookAt(focusTarget);		
			break;
		}
		
		
	}
	void FixedUpdate()
	{
		
	}
	#endregion
	
	#region Private functions
	private void smoothPosistion(Vector3 fromPos, Vector3 toPos) {
		this.transform.position = Vector3.SmoothDamp (fromPos, toPos, ref followerVelocity, Time.deltaTime * camSmoothDampTme);
	}
	private void CompenstaForWalls(Vector3 fromObject, ref Vector3 toTarget) {
		RaycastHit wallHit = new RaycastHit();
		
		Vector3 offset = Vector3.zero;
		
		if (Physics.Linecast(fromObject, toTarget, out wallHit)) {
			toTarget = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z) + Vector3.up * ScalingComenstationUpMovement + wallHit.normal * ScalingNormalCompenstation;
			offset += wallHit.normal;
		}
	}
	

	
	#endregion
	
	#region Public functions

	public void setPushMode(ref Transform obj){
		camState = CamStates.Push;		
		_obj = obj;
		_pushDir = PlayerXform.forward;	
	}

    public void setCameraState(string s,Transform o)
    {
        if (s.Equals("Throw")) {
			if(camState != CamStates.Throw) {
				prevCamstate = camState;
				camState = CamStates.Throw;
			}else {
				camState = prevCamstate;
			}
		}else if(s.Equals("Focus")) {
			if(camState != CamStates.Focus) {
			prevCamstate = camState;
			camState = CamStates.Focus;
			focusTarget = o;
			}else {
				camState = prevCamstate;
			}
		}
	}

	#endregion
}


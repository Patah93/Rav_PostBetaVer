﻿using UnityEngine;
using System.Collections;

public class ThirdPersonCamera222 : MonoBehaviour {

    #region Public variables

    //Camera posistion and look at variables
    public Vector3 Offset = Vector3.zero;
    public Transform LookAt;
    public float CameraUp = 1.0f;
    public float CameraAway = 3.0f;

    //Camera Smoothing variables
    public float CameraSmoothing = 1.0f;  
    public float LookDirDampTime = 0.1f;
    public float camSmoothDampTime = 0.1f;

    //Controller Deadzone for rotating the camera. We only want to rotate camera when we move the stick far enough.
    //values from 0 to 1
    public float deadZoneX = 0.3f;
    public float deadZoneY = 0.3f;


    //Controller variables for rotation speed etc
    public float RotationSpeedX = 5.0f;
    public float RotationSpeedY = 2.0f;

    //Clamping values for Y axis so we can't go to far up or down.
    public Vector2 cameraClampingY = new Vector2(-40.0f, 20.0f);

    //Camera State
    public CamStates camState = CamStates.Behind;

    //Camera offsets for the different default camera positions
    //We want this to be close to the eyes for example
    //made private for now
    private Vector3 FirstPersonCameraOffset = new Vector3(0.0f, 1.6f, 0.0f);

    //We want this to be a bit up from the character and a bit back for example
    //made private for now
    private Vector3 BehindPersonCameraOffset = new Vector3(0.0f, 1.0f, -1.0f);

    #endregion

    #region Private variables
    
    //what is our current look direction
    Vector3 currentLookDirection;
    
    //where we want to be looking in the end
    Vector3 lookDirection;

    //Target posistion
    private Vector3 targetPosistion;

    //TODO : Change to corresponds to correct throw posistion later
    //The first Person Camera Posistion
    private CameraPosistion FirstPersonCameraPosistion;

    //The behind Person Camera Posistion
    private CameraPosistion BehindPersonCameraPosistion;

    //Reference to the characters transform
    private Transform PlayerXform;
    
    //Reference to out controller scripts on the character.
    private AnimationMan referenceToController;

    //TODO : add other controller input values
    //The controller input values
    private float rightX = 0.0f;
    private float rightY = 0.0f;
    private float leftX  = 0.0f;
    private float leftY  = 0.0f;

    //private containers holding the velocity for the camera smoothing
    private Vector3 velocityLookDir = Vector3.zero;
    private Vector3 velocityCamSmooth = Vector3.zero;


    //private containers holding the amount of rotation around the character that have been applied from the controller input
    private float rotationAmountX = 0.0f;
    private float rotationAmountY = 0.0f;
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
        public void Init(string camName, Vector3 pos, Transform transform, Transform parent) {
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
        Free
    }
    #endregion

    #region Inits
    //Called even if script component is not enabled
    //best used for references between scripts and Inits
    void Awake() {
        //grabbing the transform from the character.
        PlayerXform = GameObject.FindWithTag("Player").transform;

        //Init out look direction to correspond where the character is looking at i.e it's forward vector.
        currentLookDirection = PlayerXform.forward;
        lookDirection = PlayerXform.forward;
        
        //Grabbing the reference to our character controller.
        referenceToController = GameObject.FindWithTag("Player").GetComponent<AnimationMan>();

        //Setting up our Camera Posistion so we can reference to them later
        //First Person Camera
        FirstPersonCameraPosistion = new CameraPosistion();
        FirstPersonCameraPosistion.Init(
            "First Person Camera",
            FirstPersonCameraOffset,
            new GameObject().transform,
            PlayerXform);

        //Behind Camera
       BehindPersonCameraPosistion.Init(
            "Behind Person Camera",
            new Vector3(0.0f, CameraUp, -CameraAway),
            new GameObject().transform,
            PlayerXform);
    }
    
    //Called if script component is enabled
    void Start () {
	    
	}
    #endregion

    #region Update funtions
    //every frame (1)
	void Update () {
	    //We need to update the players transform so we always have the correct values.
        PlayerXform = GameObject.FindWithTag("Player").transform;

        //TODO : add more controller input grabs
        //We need to grab the controller input values
        rightX = Input.GetAxis ("RightStickHorizontal");
        rightY = Input.GetAxis ("RightStickVertical");
        leftX  = Input.GetAxis ("Horizontal");
        leftY  = Input.GetAxis ("Vertical");
	}

    //after Update every frame (2)
    void LateUpdate() {
        
        //Here we check what camera  state we are actually in.
        switch (camState) {
            //If we are in the default camera state
            case CamStates.Behind:

                //adding rotation
                if(Mathf.Abs(rightX) > deadZoneX)
                    currentLookDirection = Vector3.RotateTowards(currentLookDirection, this.transform.right, rightX * Time.deltaTime * RotationSpeedX, 0.0f);

                
                
            //angle between current posistion and look at.
                float angle = Vector3.Angle(
                    Vector3.Normalize(
                    LookAt.position - this.transform.position),
                    Vector3.Normalize(
                        new Vector3(LookAt.position.x - this.transform.position.x, LookAt.position.y, LookAt.position.z - this.transform.position.z)));
               
            //we need to clamp this value so we don't go over the character.
               if (Mathf.Abs(rightY) >= deadZoneY){
                    rotationAmountY += Mathf.Rad2Deg * rightY * Time.deltaTime * RotationSpeedY;
                    if (rotationAmountY < cameraClampingY.x){
                        rotationAmountY = cameraClampingY.x;
                    }else if (rotationAmountY > cameraClampingY.y) {
                        rotationAmountY = cameraClampingY.y;
                    }

               if (rotationAmountY > cameraClampingY.x && rotationAmountY < cameraClampingY.y) 
                    currentLookDirection = Vector3.RotateTowards(currentLookDirection, this.transform.up, rightY * Time.deltaTime * RotationSpeedY, 0.0f);
               }
                
                targetPosistion =
                    //moving target pos up according to CameraUp variable 
                    (LookAt.position + (Vector3.Normalize(PlayerXform.up) * CameraUp)) -
                    //move the target a bit back according to the CameraAway variable
                    (Vector3.Normalize(currentLookDirection) * CameraAway);                

                //Debug.Log("RightX: " + rightX + " " + "RightY: " + rightY + " " + "Distance: " + Vector3.Distance(this.transform.position, LookAt.position));
                break;
        }

        CompenstaForWalls(PlayerXform.position, ref targetPosistion);
        smoothPosistion(this.transform.position, targetPosistion);
        //this.transform.position = targetPosistion;
        transform.LookAt(LookAt);

    }
    //physics updates (3) does not happen every frame
    void FixedUpdate() {

    }
    #endregion

    #region Private functions
    private void smoothPosistion(Vector3 fromPos, Vector3 toPos) {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    Vector3 RotateAroundPoint1(Vector3 point,Vector3 pivot,Quaternion angle) {
        return angle * (point - pivot) + pivot;
    }
    float ClampAngleX(float angle, float min, float max) {
        float minMaxDelta = Mathf.DeltaAngle(min, max);
        if (minMaxDelta < 0f)
            minMaxDelta += 360f;
        float minDeltaAngle = Mathf.DeltaAngle(angle, min);
        if (minDeltaAngle > 360f - minMaxDelta) {
            minDeltaAngle -= 360f;
        }

        if (minDeltaAngle < -minMaxDelta) {
            angle = max;
        }
        else if (minDeltaAngle > 0f) {
            float minMaxHalfDiff = Mathf.DeltaAngle(min, max) / 2f;
            if (minMaxHalfDiff < 0f)
                minMaxHalfDiff *= -1;
            if (minDeltaAngle < minMaxHalfDiff)
                angle = min;
            else if (minDeltaAngle > minMaxHalfDiff)
                angle = max;
        }
        return angle;
    }

    private void CompenstaForWalls(Vector3 fromObject, ref Vector3 toTarget) {
        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out wallHit)) {
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }
    #endregion

    #region Public functions
    #endregion
}

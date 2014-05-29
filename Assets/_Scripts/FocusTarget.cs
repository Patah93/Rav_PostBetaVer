using UnityEngine;
using System.Collections;

public class FocusTarget : MonoBehaviour {
    #region Public variables
	public string FocusTag = "Focus";
	public float rayCastLength = 1.0f;
    #endregion

    #region Private variables
	Transform focusObject;
	ThirdPersonCamera refToCamera;
	Transform PlayerXForm;
	CharacterController charController;
	int layerMask;
    #endregion

    #region Structs 
    #endregion

    #region Enums
    #endregion

    #region Inits
    //Called even if script component is not enabled
    //best used for references between scripts and inits
    void Awake() {
		//grabbing reference to camera
		refToCamera = Camera.main.GetComponent<ThirdPersonCamera>();
		//grabbing a reference to the players character controller.
		charController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
		//ignore all but what we want to hit
		layerMask = (1<<LayerMask.NameToLayer("Focus"));

    }
    
    //Called if script component is enabled
    void Start () {
	    
	}
    #endregion

    #region Update funtions
    //every frame (1)
	void Update () {
		//grabbing the new transform pos from the player
		PlayerXForm = GameObject.FindWithTag("CameraFollow").transform;


	}

    //after Update every frame (2)
    void LateUpdate() {
		//ray casting to check for focus targets
		RaycastHit objectHit = new RaycastHit();
		Vector3 p1 = PlayerXForm.position;
		//we only want to hit object with the tag corrent layer mask.
		layerMask = 1<<LayerMask.NameToLayer("Focus");
		//raycasting several ray around there character to check for a focus target
		if(Physics.SphereCast(p1,charController.height/2.0f,PlayerXForm.forward,out objectHit,rayCastLength,layerMask) || 
		Physics.SphereCast(p1,charController.height/2.0f,-PlayerXForm.forward,out objectHit,rayCastLength,layerMask) ||
		Physics.SphereCast(p1,charController.height/2.0f,PlayerXForm.right,out objectHit,rayCastLength,layerMask) ||
		Physics.SphereCast(p1,charController.height/2.0f,-PlayerXForm.right,out objectHit,rayCastLength,layerMask) ||
		Physics.SphereCast(p1,charController.height/2.0f,(PlayerXForm.forward+PlayerXForm.right).normalized,out objectHit,rayCastLength,layerMask) ||
		Physics.SphereCast(p1,charController.height/2.0f,((-1*PlayerXForm.forward)+(-1*PlayerXForm.right).normalized ),out objectHit,rayCastLength,layerMask)) {

			if(objectHit.transform.tag == FocusTag && Input.GetButtonDown("Interact")) {
					refToCamera.setCameraState("Focus",objectHit.transform);
			}
			if(objectHit.transform.tag == FocusTag && Input.GetButtonUp("Interact")) {
				refToCamera.setCameraState("Focus",objectHit.transform);
				}
			}

    }

    //physics updates (3) does not happen every frame
    void FixedUpdate() {

    }
    #endregion

    #region Private functions
    #endregion

    #region Public functions
    #endregion
}

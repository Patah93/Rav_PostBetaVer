using UnityEngine;
using System.Collections;

public class FocusTarget : MonoBehaviour {
    #region Public variables
	string FocusTag = "Focus";
	float rayCastLength = 1.0f;
    #endregion

    #region Private variables
	Transform focusObject;
	ThirdPersonCamera refToCamera;
	Transform PlayerXForm;
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

			if(Physics.Linecast(PlayerXForm.position,PlayerXForm.position + PlayerXForm.forward * rayCastLength,out objectHit)) {
			Debug.Log(objectHit.transform.tag);
				if(objectHit.transform.tag == FocusTag && Input.GetKeyDown(KeyCode.Z)) {
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

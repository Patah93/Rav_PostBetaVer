using UnityEngine;
using System.Collections;

public class FocusTarget : MonoBehaviour {
    #region Public variables
	public string FocusTag = "Focus";
	public float rayCastLength = 1.0f;
    #endregion

    #region Private variables
	private Transform focusObject;
	private	ThirdPersonCamera refToCamera;
	private Transform PlayerXForm;
	private CharacterController charController;
	private int layerMask;
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
		//raycasting several ray around there character to check for a focus target
		//ray casting to check for focus targets
		Collider[] col = Physics.OverlapSphere(PlayerXForm.position,rayCastLength,layerMask);
		Debug.Log(col.Length);
		if(col.Length == 1) {
			if(col[0].gameObject.tag == FocusTag && Input.GetButtonDown("Interact")) {
				refToCamera.setCameraState("Focus",col[0].transform);
			}
			if(col[0].gameObject.tag == FocusTag && Input.GetButtonUp("Interact")) {
				refToCamera.setCameraState("Focus",col[0].transform);
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

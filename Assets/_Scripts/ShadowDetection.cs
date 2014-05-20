using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowDetection : MonoBehaviour {

	private Vector3 _sunDirection; //Direction TOWARDS the sun

	private Vector3 _lastPosition;

	private Vector3[] _pointsOfInterest;

	private Vector3[] _moreSpreadPoints;

	private Vector3[] _localPointsOfInterest;

	private Vector3[] _localMoreSpreadPoints;

	//private bool temp_isLighted;

	//private int _numberLightedVertices;

	GameObject[] lamps;

	GameObject[] spotLights;

	GameObject[] _shadowCasters;

	// Use this for initialization
	void Start () {
		_sunDirection = GameObject.FindGameObjectWithTag("Sun").transform.forward * -1;

		//temp_isLighted = false;

		//_numberLightedVertices = 0;

		_lastPosition = transform.position - new Vector3(100, 100, 100);

		lamps = GameObject.FindGameObjectsWithTag("Lamp");

		spotLights = GameObject.FindGameObjectsWithTag("SpotLight");

		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		initiateShadowCasters(ref allObjects);

		initiateLocalPointsOfInterest();
		updatePointsOfInterest ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		updatePointsOfInterest ();

		/* TODO If fox (fox-ghost) moved OR shadows have changed *
			if (isObjectInLight ()) {
				/* Handle ... *
					// - Stop the actual fox
					// - Kill the fox
					// - etc... (Whatever we decide to do...)

				//if(!temp_isLighted){
					//Debug.Log("OH MY GOD THE LIGHT! IT IS SO BRIGHT! D:\n" + _numberLightedVertices + " vertices in the sun! D=");
					temp_isLighted = true;
				//}
			}

		else{
		/* END *

		if(temp_isLighted){
			//Debug.Log("Mmm vad SKÖÖN SKUGGA MUMS");
			temp_isLighted = false;
		}
		}
		*/
		//gameObject.collider.bounds.IntersectRay(new Ray(gameObject.collider.bounds., _sunDirection
	}

	void initiateLocalPointsOfInterest(){
		BoxCollider boxColl = GetComponent<BoxCollider>();
		
		float minX = boxColl.center.x - boxColl.size.x/2.0f;
		float maxX = boxColl.center.x + boxColl.size.x/2.0f;
		float minY = boxColl.center.y - boxColl.size.y/2.0f;
		float maxY = boxColl.center.y + boxColl.size.y/2.0f;

		_localPointsOfInterest = new Vector3[21];
		_pointsOfInterest = new Vector3[21];
		
		_localPointsOfInterest[0] = new Vector3(minX, minY, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[1] = new Vector3(minX, maxY, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[2] = new Vector3(maxX, minY, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[3] = new Vector3(maxX, maxY, boxColl.center.z + boxColl.size.z/2.0f);
		
		_localPointsOfInterest[4] = new Vector3(minX/2.0f, maxY, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[5] = new Vector3(maxX/2.0f, maxY, boxColl.center.z + boxColl.size.z/2.0f);

		_localPointsOfInterest[6] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/2.0f);

		_localPointsOfInterest[7] = new Vector3(maxX, maxY/3.0f, boxColl.center.z + boxColl.size.z/2.0f);
		
		_localPointsOfInterest[8] = new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/2.0f);

		_localPointsOfInterest[9] = new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/2.0f);
		
		_localPointsOfInterest[10] = new Vector3(minX, boxColl.center.y, boxColl.center.z + boxColl.size.z/2.0f);

		_localPointsOfInterest[11] = new Vector3(minX, minY, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[12] = new Vector3(minX, maxY, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[13] = new Vector3(maxX, minY, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[14] = new Vector3(maxX, maxY, boxColl.center.z - boxColl.size.z/2.0f);
		
		_localPointsOfInterest[15] = new Vector3(boxColl.center.x, maxY, boxColl.center.z - boxColl.size.z/2.0f);

		_localPointsOfInterest[16] = new Vector3(minX, maxY/3.0f, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[17] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/2.0f);

		_localPointsOfInterest[18] = new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[19] = new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/2.0f);

		_localPointsOfInterest[20] = new Vector3(maxX, boxColl.center.y, boxColl.center.z - boxColl.size.z/2.0f);

		/* ------------------------------------------------------------------------------------------------ */

		_localMoreSpreadPoints = new Vector3[119];
		_moreSpreadPoints = new Vector3[119];
		
		_localMoreSpreadPoints[0] = new Vector3(minX, minY, boxColl.center.z + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[1] = new Vector3(minX, maxY, boxColl.center.z + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[2] = new Vector3(maxX, minY, boxColl.center.z + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[3] = new Vector3(maxX, maxY, boxColl.center.z + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[4] = new Vector3(minX/2.0f, maxY, boxColl.center.z + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[5] = new Vector3(maxX/2.0f, maxY, boxColl.center.z + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[6] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[7] = new Vector3(maxX, maxY/3.0f, boxColl.center.z + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[8] = new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[9] = new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[10] = new Vector3(minX, boxColl.center.y, boxColl.center.z + boxColl.size.z/8.0f);

		_localMoreSpreadPoints[11] = new Vector3(boxColl.center.x, maxY, boxColl.center.z + boxColl.size.z/8.0f);

		_localMoreSpreadPoints[12] = new Vector3(minX, maxY/3.0f, boxColl.center.z + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[13] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[14] = new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[15] = new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[16] = new Vector3(maxX, boxColl.center.y, boxColl.center.z + boxColl.size.z/8.0f);

		/* ------------ */
		
		_localMoreSpreadPoints[17] = new Vector3(minX, minY, boxColl.center.z - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[18] = new Vector3(minX, maxY, boxColl.center.z - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[19] = new Vector3(maxX, minY, boxColl.center.z - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[20] = new Vector3(maxX, maxY, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[21] = new Vector3(minX/2.0f, maxY, boxColl.center.z - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[22] = new Vector3(maxX/2.0f, maxY, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[23] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[24] = new Vector3(maxX, maxY/3.0f, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[25] = new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[26] = new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[27] = new Vector3(minX, boxColl.center.y, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[28] = new Vector3(boxColl.center.x, maxY, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[29] = new Vector3(minX, maxY/3.0f, boxColl.center.z - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[30] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[31] = new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[32] = new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[33] = new Vector3(maxX, boxColl.center.y, boxColl.center.z - boxColl.size.z/8.0f);
		
		/* ------------ */

		_localMoreSpreadPoints[34] = new Vector3(minX, minY, boxColl.center.z + boxColl.size.z/4.0f);
		_localMoreSpreadPoints[35] = new Vector3(minX, maxY, boxColl.center.z + boxColl.size.z/4.0f);
		_localMoreSpreadPoints[36] = new Vector3(maxX, minY, boxColl.center.z + boxColl.size.z/4.0f);
		_localMoreSpreadPoints[37] = new Vector3(maxX, maxY, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[38] = new Vector3(minX/2.0f, maxY, boxColl.center.z + boxColl.size.z/4.0f);
		_localMoreSpreadPoints[39] = new Vector3(maxX/2.0f, maxY, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[40] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[41] = new Vector3(maxX, maxY/3.0f, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[42] = new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[43] = new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[44] = new Vector3(minX, boxColl.center.y, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[45] = new Vector3(boxColl.center.x, maxY, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[46] = new Vector3(minX, maxY/3.0f, boxColl.center.z + boxColl.size.z/4.0f);
		_localMoreSpreadPoints[47] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[48] = new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/4.0f);
		_localMoreSpreadPoints[49] = new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[50] = new Vector3(maxX, boxColl.center.y, boxColl.center.z + boxColl.size.z/4.0f);
		
		/* ------------ */

		_localMoreSpreadPoints[51] = new Vector3(minX, minY, boxColl.center.z - boxColl.size.z/4.0f);
		_localMoreSpreadPoints[52] = new Vector3(minX, maxY, boxColl.center.z - boxColl.size.z/4.0f);
		_localMoreSpreadPoints[53] = new Vector3(maxX, minY, boxColl.center.z - boxColl.size.z/4.0f);
		_localMoreSpreadPoints[54] = new Vector3(maxX, maxY, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[55] = new Vector3(minX/2.0f, maxY, boxColl.center.z - boxColl.size.z/4.0f);
		_localMoreSpreadPoints[56] = new Vector3(maxX/2.0f, maxY, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[57] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[58] = new Vector3(maxX, maxY/3.0f, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[59] = new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[60] = new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[61] = new Vector3(minX, boxColl.center.y, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[62] = new Vector3(boxColl.center.x, maxY, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[63] = new Vector3(minX, maxY/3.0f, boxColl.center.z - boxColl.size.z/4.0f);
		_localMoreSpreadPoints[64] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[65] = new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/4.0f);
		_localMoreSpreadPoints[66] = new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/4.0f);
		
		_localMoreSpreadPoints[67] = new Vector3(maxX, boxColl.center.y, boxColl.center.z - boxColl.size.z/4.0f);
		
		/* ------------ */

		float minZ = boxColl.center.z - boxColl.size.z/2.0f;
		float maxZ = boxColl.center.z + boxColl.size.z/2.0f;

		/* ------------ */

		_localMoreSpreadPoints[68] = new Vector3(minX, minY, minZ + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[69] = new Vector3(minX, maxY, minZ + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[70] = new Vector3(maxX, minY, minZ + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[71] = new Vector3(maxX, maxY, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[72] = new Vector3(minX/2.0f, maxY, minZ + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[73] = new Vector3(maxX/2.0f, maxY, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[74] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[75] = new Vector3(maxX, maxY/3.0f, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[76] = new Vector3(minX, maxY/3.0f/2.0f, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[77] = new Vector3(maxX, maxY/3.0f*2.0f, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[78] = new Vector3(minX, boxColl.center.y, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[79] = new Vector3(boxColl.center.x, maxY, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[80] = new Vector3(minX, maxY/3.0f, minZ + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[81] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[82] = new Vector3(minX, maxY/3.0f*2.0f, minZ + boxColl.size.z/8.0f);
		_localMoreSpreadPoints[83] = new Vector3(maxX, maxY/3.0f/2.0f, minZ + boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[84] = new Vector3(maxX, boxColl.center.y, minZ + boxColl.size.z/8.0f);
		
		/* ------------ */
		
		_localMoreSpreadPoints[85] = new Vector3(minX, minY, maxZ - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[86] = new Vector3(minX, maxY, maxZ - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[87] = new Vector3(maxX, minY, maxZ - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[88] = new Vector3(maxX, maxY, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[89] = new Vector3(minX/2.0f, maxY, maxZ - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[90] = new Vector3(maxX/2.0f, maxY, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[91] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[92] = new Vector3(maxX, maxY/3.0f, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[93] = new Vector3(minX, maxY/3.0f/2.0f, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[94] = new Vector3(maxX, maxY/3.0f*2.0f, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[95] = new Vector3(minX, boxColl.center.y, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[96] = new Vector3(boxColl.center.x, maxY, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[97] = new Vector3(minX, maxY/3.0f, maxZ - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[98] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[99] = new Vector3(minX, maxY/3.0f*2.0f, maxZ - boxColl.size.z/8.0f);
		_localMoreSpreadPoints[100] = new Vector3(maxX, maxY/3.0f/2.0f, maxZ - boxColl.size.z/8.0f);
		
		_localMoreSpreadPoints[101] = new Vector3(maxX, boxColl.center.y, maxZ - boxColl.size.z/8.0f);
		
		/* ------------ */
		
		_localMoreSpreadPoints[102] = new Vector3(minX, minY, boxColl.center.z);
		_localMoreSpreadPoints[103] = new Vector3(minX, maxY, boxColl.center.z);
		_localMoreSpreadPoints[104] = new Vector3(maxX, minY, boxColl.center.z);
		_localMoreSpreadPoints[105] = new Vector3(maxX, maxY, boxColl.center.z);
		
		_localMoreSpreadPoints[106] = new Vector3(minX/2.0f, maxY, boxColl.center.z);
		_localMoreSpreadPoints[107] = new Vector3(maxX/2.0f, maxY, boxColl.center.z);
		
		_localMoreSpreadPoints[108] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z);
		
		_localMoreSpreadPoints[109] = new Vector3(maxX, maxY/3.0f, boxColl.center.z);
		
		_localMoreSpreadPoints[110] = new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z);
		
		_localMoreSpreadPoints[111] = new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z);
		
		_localMoreSpreadPoints[112] = new Vector3(minX, boxColl.center.y, boxColl.center.z);
		
		_localMoreSpreadPoints[113] = new Vector3(boxColl.center.x, maxY, boxColl.center.z);
		
		_localMoreSpreadPoints[114] = new Vector3(minX, maxY/3.0f, boxColl.center.z);
		_localMoreSpreadPoints[115] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z);
		
		_localMoreSpreadPoints[116] = new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z);
		_localMoreSpreadPoints[117] = new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z);
		
		_localMoreSpreadPoints[118] = new Vector3(maxX, boxColl.center.y, boxColl.center.z);
	}

	bool updatePointsOfInterest(){
		if ((_lastPosition - transform.position).sqrMagnitude > 0.1f) {
			for (int i = 0; i < _pointsOfInterest.Length; i++) {
				_pointsOfInterest [i] = gameObject.transform.TransformPoint (_localPointsOfInterest [i]);
			}
			_lastPosition = transform.position;
		} else {
			return false;
		}
		/*
		for(int i = 0; i < _pointsOfInterest.Length-1; i++){
			for(int j = i+1; j < _pointsOfInterest.Length; j++){ 
				Debug.DrawLine(_pointsOfInterest[i], _pointsOfInterest[j]);
			}
		}
		*/
		return true;
	}

	bool updateMoreSpreadPointsOfInterest(){
		if ((_lastPosition - transform.position).sqrMagnitude > 0.1f) {
			for (int i = 0; i < _moreSpreadPoints.Length; i++) {
				_moreSpreadPoints [i] = gameObject.transform.TransformPoint (_localMoreSpreadPoints [i]);
			}
			_lastPosition = transform.position;
		} else {
			return false;
		}
		/*
		for(int i = 0; i < _moreSpreadPoints.Length-1; i++){
			for(int j = i+1; j < _moreSpreadPoints.Length; j++){ 
				Debug.DrawLine(_moreSpreadPoints[i], _moreSpreadPoints[j]);
			}
		}
		*/
		return true;
	}

	public bool isObjectInLight(){

		if (!updatePointsOfInterest ()) {
			return false;
		}

		GameObject[] shadowCasters = getPotentialShadowCasters ();

		//bool return_value = false;
		//_numberLightedVertices = 0;

		for (int i = 0; i < _pointsOfInterest.Length; i++) {

			/* Solen */
			if(isPointInLight(_pointsOfInterest[i], ref shadowCasters)){
				return true;
				//return_value = true;
				////Debug.Log("OMG SOLEN");
				//_numberLightedVertices++;
			}

			/* SpotLights */
			else if(isPointInSpotLight(/*getLamps()*/ spotLights, _pointsOfInterest[i], ref shadowCasters)){
				return true;
				//return_value = true;
				////Debug.Log("OMG SPOTLIGHT");
				//_numberLightedVertices++;
			}

			/* Lampor */
			else if(isPointInLampLight(/*getLamps()*/ lamps, _pointsOfInterest[i], ref shadowCasters)){
				return true;
				//return_value = true;
				////Debug.Log("OMG LAMPA");
				//_numberLightedVertices++;
			}

		}

		return false;
		//return return_value;
	}

	public bool isObjectInLightMorePoints(){
		
		if(!updateMoreSpreadPointsOfInterest ()){
			return false;
		}
		
		GameObject[] shadowCasters = getPotentialShadowCasters ();
		
		//bool return_value = false;
		//_numberLightedVertices = 0;
		
		for (int i = 0; i < _moreSpreadPoints.Length; i++) {
			
			/* Solen */
			if(isPointInLight(_moreSpreadPoints[i], ref shadowCasters)){
				return true;
				//return_value = true;
				////Debug.Log("OMG SOLEN");
				//_numberLightedVertices++;
			}
			
			/* SpotLights */
			else if(isPointInSpotLight(/*getLamps()*/ spotLights, _moreSpreadPoints[i], ref shadowCasters)){
				return true;
				//return_value = true;
				////Debug.Log("OMG SPOTLIGHT");
				//_numberLightedVertices++;
			}
			
			/* Lampor */
			else if(isPointInLampLight(/*getLamps()*/ lamps, _moreSpreadPoints[i], ref shadowCasters)){
				return true;
				//return_value = true;
				////Debug.Log("OMG LAMPA");
				//_numberLightedVertices++;
			}
			
		}
		
		return false;
		//return return_value;
	}

	bool isPointInLight(Vector3 point, ref GameObject[] shadowCasters){

		Ray theRay = new Ray(point, _sunDirection);

		//Debug.DrawRay (point, _sunDirection, Color.cyan, 100f);
		//Debug.DrawLine (point, point + _sunDirection * 100, Color.cyan, 1);
		/*
		for(int i = 0; i < shadowCasters.Length; i++){
			if(shadowCasters[i].collider.bounds.IntersectRay(theRay)){
				////Debug.Log ("Blocked by: " + shadowCasters[i].name);
				RaycastHit bajskorv;
				Physics.Raycast(theRay, out bajskorv);
				//Debug.DrawLine (point, shadowCasters[i].transform.position, Color.red, 1);
				Debug.DrawLine (point, bajskorv.point, Color.red, 1);
				return false;
			}
		}
		*/

		return !Physics.Raycast(theRay);

		//return true;
	}

	bool isPointInLampLight(GameObject[] lamps, Vector3 point, ref GameObject[] shadowCasters){
		Ray theRay = new Ray();

		if(lamps.Length <= 0){
			return false;
		}

		bool return_value;

		for(int i = 0; i < lamps.Length; i++){

			if(lamps[i] != null && lamps[i].light.enabled && (point - lamps[i].transform.position).sqrMagnitude <= lamps[i].light.range *lamps[i].light.range){
				theRay.origin = point;
				theRay.direction = (lamps[i].transform.position - point).normalized; 

				return_value = true;

				/*for(int j = 0; j < shadowCasters.Length; j++){
					float length;
					if(shadowCasters[j].collider.bounds.IntersectRay(theRay, out length)){
						if(length * length  <= (point - lamps[i].transform.position).sqrMagnitude){
							////Debug.Log (shadowCasters[j].name + ": är ivägen! D:");
							return_value = false;
						}
					}
				}
				*/

				return_value = !Physics.Raycast(theRay, (point - lamps[i].transform.position).magnitude);

				if(return_value){
					return true;
				}
			}
		}

		return false;
	}

	bool isPointInSpotLight(GameObject[] spotLights, Vector3 point, ref GameObject[] shadowCasters){
		Ray theRay = new Ray();
		
		if(spotLights.Length <= 0){
			return false;
		}

		bool return_value;
		
		for(int i = 0; i < spotLights.Length; i++){
			
			if(spotLights[i] != null && spotLights[i].light.enabled && (point - spotLights[i].transform.position).sqrMagnitude <= spotLights[i].light.range * spotLights[i].light.range){
				////Debug.Log ("In range...");
				theRay.origin = point;
				theRay.direction = (spotLights[i].transform.position - point).normalized;


				if(Vector3.Angle(spotLights[i].transform.forward, (theRay.direction*-1)) <= spotLights[i].light.spotAngle/2.0f){
					////Debug.Log ("In cone...");
					return_value = true;

					/*
					for(int j = 0; j < shadowCasters.Length; j++){
						float length;
						if(shadowCasters[j].collider.bounds.IntersectRay(theRay, out length)){
							if(length * length  <= (point - spotLights[i].transform.position).sqrMagnitude){
								////Debug.Log (shadowCasters[j].name + ": är ivägen! D:");
								return_value = false;
							}
						}
					}
					*/
					int layerMask = 1 << 11;
					layerMask = ~layerMask;
					if(Physics.Raycast(theRay, (point - spotLights[i].transform.position).magnitude, layerMask)){
						return_value = false;
					}
					//Debug.DrawLine(point, point + theRay.direction * (point - spotLights[i].transform.position).magnitude, Color.red, 1);
					
					if(return_value){
						return true;
					}

				}
			}
		}
		
		return false;
	}

	GameObject[] getLamps(){
		/* TODO find relevant lamps and yao 
		 * Maybe find relevant lamps using logical volumes in the map,
		 * could be the same as the music-areas
		 */ 

		//ArrayList nearbyLamps = new ArrayList();
		List<GameObject> nearbyLamps = new List<GameObject>();


		for(int i = 0; i < lamps.Length; i++){
			if((lamps[i].transform.position - gameObject.transform.position).sqrMagnitude < lamps[i].light.range * lamps[i].light.range){
				nearbyLamps.Add(lamps[i]);
				////Debug.Log("Distance to Lamp: " + (lamps[i].transform.position - gameObject.transform.position).sqrMagnitude
				         // + "\nMax Distance: " + lamps[i].light.range * lamps[i].light.range);
			}
		}

		//GameObject[] array = new GameObject[nearbyLamps.Count];
		//nearbyLamps.CopyTo ( array );

		//return array;

		return nearbyLamps.ToArray();

	}

	GameObject[] getPotentialShadowCasters(){
		/* TODO Use position difference
		 * and compare with direction of sun
		 * using dot product ?? BARA EN IDE LOL
		 * 
		 * Will minska antalet object att testa 
		 * hemsk dyr raycasting mot
		 */
		
		return _shadowCasters;
	}

	void initiateShadowCasters(ref GameObject[] allObjects){
		
		ArrayList shadowCasters = new ArrayList();

		for(int i = 0; i < allObjects.Length; i++){
			if(allObjects[i].GetComponent(typeof(Renderer)) != null && allObjects[i].renderer.castShadows && allObjects[i].collider != null && allObjects[i].activeInHierarchy){
				shadowCasters.Add (allObjects[i]);
			}
			/*else{
				Renderer[] renderers = allObjects[i].GetComponentsInChildren<Renderer>();
				for(int j = 0; j < renderers.Length; j++){
					if(renderers[j].castShadows){
						shadowCasters.Add (allObjects[i]);
						break;
					}
				}
			}
			*/
		}
		
		_shadowCasters = new GameObject[shadowCasters.Count];

		shadowCasters.CopyTo ( _shadowCasters );
	}

	/*
	GameObject[] FindGameObjectsWithLayer (int layer, GameObject[] objects) {
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < objects.Length; i++) {
			if (objects[i].layer == layer) {
				list.Add(objects[i]);
			}
		}
		if (list.Count == 0) {
			return null;
		}
		return list.ToArray();
	}
	*/
}

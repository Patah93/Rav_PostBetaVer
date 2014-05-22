using UnityEngine;
using System.Collections;

public class CheckpointSystem : MonoBehaviour {

	public GameObject[] _checkPoints;

	public GameObject CompareCheckpoints(GameObject cp1, GameObject cp2){

		int cpIndex1 = -1, cpIndex2 = -1;

		for(int i = 0; i < _checkPoints.Length; i++){

			if(_checkPoints[i] == cp1)
				cpIndex1 = i;
			if(_checkPoints[i] == cp2)
				cpIndex2 = i;
			if(cpIndex1 != -1 && cpIndex2 != -1)
				break;
		}

		return (cpIndex1 > cpIndex2) ? _checkPoints[cpIndex1] : _checkPoints[cpIndex2];
	
	}

}

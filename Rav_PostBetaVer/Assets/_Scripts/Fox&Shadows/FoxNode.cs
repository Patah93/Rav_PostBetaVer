using UnityEngine;
using System.Collections;

public class FoxNode : MonoBehaviour {

	public FoxNode _nextNode;
	public FoxNode _prevNode;

	// Use this for initialization
	void Start () {
		if(GetComponent<Renderer>() != null)
			GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setPrevNode (FoxNode prevNode){
		if(_prevNode == null){
			_prevNode = prevNode;
		}
		if (_nextNode != null && _nextNode._prevNode == null) {
			_nextNode.setPrevNode (this);
		}
	}
}

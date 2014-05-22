using UnityEngine;
using System.Collections;

public class GroundType : MonoBehaviour {

	public enum Ground {Default, Sand, Metal}

	public Ground _type = Ground.Default;

	public int GetType(){

		int a;

		if(_type == Ground.Default)
			a = 0;
		else if(_type == Ground.Sand)
			a = 1;
		else if(_type == Ground.Metal)
			a = 2;
		else 
			a = 0;

		return a;
	}

}

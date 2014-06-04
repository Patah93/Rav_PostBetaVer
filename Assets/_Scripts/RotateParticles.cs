using UnityEngine;
using System.Collections;

public class RotateParticles : MonoBehaviour {

	float _shake;
	bool _leftRot;
	Quaternion _shaking;

	public float _rotationSpeed;
	public float _shakeSpeed;
	public float _shakeDip;
	int _countdown = 0;

	// Use this for initialization
	void Start () {
		_shakeDip = 50f;
		_shakeSpeed = 1.0f;
		_shake = _shakeDip*-1;
		_leftRot = true;

	}
	
	// Update is called once per frame
	void Update () {
		////Checks a countdown, if it is 3 or bigger, then run code in if-statement.
		if (_countdown >= 3) {

				////Rotates object with deltatime times the rotationspeed. This one rotates around the z-axis.
				transform.Rotate (new Vector3 (0, 0, Time.deltaTime * _rotationSpeed));

				////Checks which way it's supposed to rotatate towards, as it's supposed to "shake". This one rotates around the y-axis.
				if (_leftRot == false) {
						transform.Rotate (new Vector3 (0, Time.deltaTime * (_shake % _shakeDip), 0));
						_shake += _shakeSpeed;
						if (_shake >= _shakeDip) {
								_leftRot = true;
						}
						//////Debug.Log (_shake);
				} else if (_leftRot == true) {
						transform.Rotate (new Vector3 (0, Time.deltaTime * (_shake % _shakeDip), 0));
						_shake -= _shakeSpeed;
						if (_shake <= (_shakeDip * -1)) {
								_leftRot = false;
						}
						//////Debug.Log (_shake);
				}
			_countdown = 0;

		}

		_countdown += 1;
	}



}

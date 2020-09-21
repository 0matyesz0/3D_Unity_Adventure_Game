using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

	public float _mouseSensitivity = 10;
	public Transform _origin;
	public float _dstFromOrigin = 2.5f;
	public Vector2 _verticalMinMax = new Vector2(-40, 85);
	
	// variables for camera smoothing 
	public float _rotationSmoothTime = .4f;
	Vector3 _rotationSmoothVelocity;
	Vector3 _currentRotation;

	float _horizontal;
	float _vertical;

	// this method will be called after all the other update methods.
	void LateUpdate () {
		_horizontal += Input.GetAxis("Mouse X") * _mouseSensitivity;
		_vertical -= Input.GetAxis("Mouse Y") * _mouseSensitivity;

		// clamping the vertical movement of our camera, so it won't be upside down by any chance.
		_vertical = Mathf.Clamp(_vertical, _verticalMinMax.x, _verticalMinMax.y);

		_currentRotation = Vector3.SmoothDamp(_currentRotation, new Vector3(_vertical, _horizontal), ref _rotationSmoothVelocity, _rotationSmoothTime);
		transform.eulerAngles = _currentRotation;

		// positioning the camera:
		transform.position = _origin.position - transform.forward * _dstFromOrigin;

        
	}
}

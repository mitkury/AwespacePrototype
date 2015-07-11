using UnityEngine;
using System.Collections;

public class MouseCameraControl : MonoBehaviour {

	public MouseLookSimple mouseLook;
	public Camera targetCamera;

	void Start () {
		mouseLook.Init(transform, targetCamera.transform);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update () {
		mouseLook.LookRotation(transform, targetCamera.transform);
	}
	
}

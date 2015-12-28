using UnityEngine;
using System.Collections;

public class Visitor : MonoBehaviour {

	public Transform vrCameraRig;
	public Transform vrCenterOfView;
	public Transform regularCameraRig;
	public Transform regularCenterOfView;

	Sight sight;
	
	void Start () {
		Application.targetFrameRate = 60;

		sight = GetComponent<Sight>();

		/*
		// VR mode.
		if (Visitor.isInVRMode) {
			vrCameraRig.gameObject.SetActive(true);
			regularCameraRig.gameObject.SetActive(false);
			sight.anchor = vrCenterOfView;
		} 
		// Regular mode.
		else {
			regularCameraRig.gameObject.SetActive(true);
			vrCameraRig.gameObject.SetActive(false);
			sight.anchor = regularCenterOfView;
		}
		*/

		sight.anchor = regularCenterOfView;
		
		if (UnityEngine.VR.VRSettings.enabled) {
			regularCameraRig.GetComponent<MouseCameraControl>().enabled = false;
		}
	}

	void OnEnable() {
		/*
		OVRTouchpad.Create();
		OVRTouchpad.TouchHandler += TouchOnTouchpadHandler;
		*/
	}

	void OnDisable() {
		//OVRTouchpad.TouchHandler -= TouchOnTouchpadHandler;
	}

	//#if UNITY_STANDALONE
	void Update() {
		#if UNITY_ANDROID
		if (Input.GetMouseButtonDown(0) && sight.target != null) {
			sight.target.SendMessage("OnClick", sight.hitInfo.point, SendMessageOptions.DontRequireReceiver);
		}
		#endif

		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Break();
		}
		#endif
	}
	//#endif

	void TouchOnTouchpadHandler (object sender, System.EventArgs e) {
		/*
		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs)e;
		if(touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap) {
			sight.target.SendMessage("OnClick", sight.hitInfo.point, SendMessageOptions.DontRequireReceiver);
		}
		*/
	}

}

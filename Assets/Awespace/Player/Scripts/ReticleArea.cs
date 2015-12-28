using UnityEngine;
using System.Collections;

public class ReticleArea : MonoBehaviour {

	public Collider collider;
	public Sight sight;
	public GameObject reticle;

	public float TimeSinceFocusedOnArea { get; set; }
	public float ElapsedTimeSinceLastFocus { 
		get { 
			return TimeSinceFocusedOnArea != 0f ? Time.timeSinceLevelLoad - TimeSinceFocusedOnArea : float.MaxValue;
		} 
	}
	public float TimeSinceMovedReticle { get; private set; }
	public float ElapsedTimeSinceMovedReticle { 
		get {
			return TimeSinceMovedReticle != 0f ? Time.timeSinceLevelLoad - TimeSinceMovedReticle : float.MaxValue;
		}
	}

	void OnEnable() {
		TimeSinceFocusedOnArea = Time.timeSinceLevelLoad;
		TimeSinceMovedReticle = Time.timeSinceLevelLoad;
	}
	
	void Update () {
		var relativePointAtDistance = sight.hitInfo.collider != null ? sight.hitInfo.point : collider.transform.position;
		var closestPositionToBounds = sight.anchor.position + sight.facingVector * Vector3.Distance(sight.anchor.position, relativePointAtDistance);
		var closestPointOnBounds = collider.ClosestPointOnBounds(closestPositionToBounds);

		if (Vector3.Distance(closestPointOnBounds, closestPositionToBounds) <= 0.01f) {
			TimeSinceFocusedOnArea = Time.timeSinceLevelLoad;
		}

		#if UNITY_ANDROID
		//reticle.transform.position = closestPointOnBounds;	
		reticle.transform.position = Vector3.Lerp(reticle.transform.position, closestPointOnBounds, 30f);
		#endif

		#if UNITY_STANDALONE

        // If VR isn't enabled and Alt key isn't pressed--don't move the cursor by the mouse.
        if (!UnityEngine.VR.VRSettings.enabled && !Input.GetKey(KeyCode.LeftAlt))
            return;		

		if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
			TimeSinceMovedReticle = Time.timeSinceLevelLoad;
		}

		if (Input.GetAxis("Mouse X") != 0) {
			reticle.transform.position = reticle.transform.position + reticle.transform.right * Input.GetAxis("Mouse X") * 0.01f;
		}
		if (Input.GetAxis("Mouse Y") != 0) {
			reticle.transform.position = reticle.transform.position + reticle.transform.forward * Input.GetAxis("Mouse Y") * 0.01f;
		}

		reticle.transform.position = collider.bounds.ClosestPoint(reticle.transform.position);

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hitInfo;
			Physics.Raycast(sight.anchor.position, reticle.transform.position - sight.anchor.position, out hitInfo, sight.layerMask);

			if (hitInfo.collider != null) {
				var target = hitInfo.rigidbody != null ? hitInfo.rigidbody.gameObject : hitInfo.collider.gameObject;

				if (target != null) {
					target.SendMessage("OnClick", hitInfo.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		#endif

	}
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sight : MonoBehaviour {
	
	[HideInInspector]
	public RaycastHit hitInfo;
	[HideInInspector]
	public Transform anchor;
	public Reticle reticle;
	public LayerMask layerMask = 1;
	public float focusTimeSec;
	public float focusOnTargetWithoutInterruptionSec { get; private set; }
	public Vector3 facingVector { get; private set; }
	public GameObject target;

	void Update() {
		UpdateHitInfo();
		UpdateReticle();
		UpdateTarget();
		UpdateFocus();
	}
	
	void OnEnable() {
		reticle.gameObject.SetActive(true);
		reticle.SetBody(0);
		
		//OVRTouchpad.TouchHandler += HandleTouchHandler;
	}
	
	void HandleTouchHandler (object sender, System.EventArgs e) {
		/*
		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs)e;
		if(touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap) {
			focusOnTargetWithoutInterruptionSec = 999f;
		}
		*/
	}
	
	void OnDisable() {
		reticle.gameObject.SetActive(false);
		hitInfo = new RaycastHit();
		focusOnTargetWithoutInterruptionSec = 0;
	}
	
	void UpdateReticle() {
		reticle.transform.LookAt(anchor.transform.position);
		var reticleDistance = hitInfo.collider != null ? hitInfo.distance : 500f;
		Vector3 targetPosition = hitInfo.collider != null ? hitInfo.point : facingVector * 500f;
		
		/*
		// A little bit of magic so reticle won't look smaller when observed up-close.
		if (reticleDistance < 10f) {
			reticleDistance *= 1 + 5*Mathf.Exp(-reticleDistance);
		}
		*/
		
		reticle.SetFocus(targetPosition, reticle.originalScale * reticleDistance);
	}
	
	void UpdateHitInfo() {
		facingVector = anchor.TransformDirection(Vector3.forward);
		Physics.Raycast(anchor.position, facingVector, out hitInfo, 500f, layerMask);
	}
	
	void UpdateTarget() {

		if (hitInfo.collider != null)
			target = hitInfo.rigidbody != null ? hitInfo.rigidbody.gameObject : hitInfo.collider.gameObject;
		else
			target = null;	
	}
	
	void UpdateFocus() {
		/*
		if (target == null || !target.isAbleToInteract) {
			focusOnTargetWithoutInterruptionSec = 0;
			return;
		}
		
		focusOnTargetWithoutInterruptionSec += Time.deltaTime;
		*/
	}
	
	Vector3 GetClosestPointOnBoundsToFocusPoint(Component target) {
		var anchorPosition = anchor.position;
		
		var relativePointAtDistance = hitInfo.collider != null ? hitInfo.point : target.transform.position;
		var closestPositionToThing = anchorPosition + facingVector * Vector3.Distance(anchorPosition, relativePointAtDistance);
		var closestPointOnBounds = Vector3.zero;
		
		//Debug.DrawLine(anchorPosition, closestPositionToThing, Color.green, 0.25f);
		
		if (target is Collider) {
			closestPointOnBounds = (target as Collider).ClosestPointOnBounds(closestPositionToThing);
		} else if (target is Rigidbody) {
			closestPointOnBounds = (target as Rigidbody).ClosestPointOnBounds(closestPositionToThing);
		} else {
			Debug.LogError(target+" couldn't used as it doesn't have bounds");
		}
		
		//Debug.DrawLine(anchorPosition, closestPointOnBounds, Color.red, 0.25f);
		
		return closestPointOnBounds;
	}
	
	float GetAngleBetweenFocusPointAndPosition(Vector3 position) {
		var anchorPosition = anchor.position;
		var targetDir = position - anchorPosition;
		return Vector3.Angle(targetDir, facingVector);
	}

	/*
	public void SetReticleVisibility(List<Component> components) {
		var smallestAngle = 180f;
		var minAngle = 8;
		
		if (target != null && target.isAbleToInteract && King.visitor.itemInHand == null) {
			smallestAngle = 0;
		} else {
			foreach (Component component in components) {
				if (component == null)
					continue;
				
				var targetPosition = component.transform.position;
				var targetRigidbody = component.GetComponent<Rigidbody>();
				Collider targetCollider;
				
				if (targetRigidbody != null) {
					targetPosition = GetClosestPointOnBoundsToFocusPoint(targetRigidbody);
				} else {
					targetCollider = component.GetComponent<Collider>();
					targetPosition = GetClosestPointOnBoundsToFocusPoint(targetCollider);
				}
				
				var angle = GetAngleBetweenFocusPointAndPosition(targetPosition);
				
				// Sort out a smallest angle.
				smallestAngle = angle < smallestAngle ? angle : smallestAngle;
			}
		}
		
		// Show a reticle only when a smallest angle to an object is less than x.
		if (smallestAngle < minAngle) {
			reticle.SetBodyScale(1);

		} else {
			reticle.SetBodyScale(0.4f);
		}
	}
	*/
	
	public void ResetTarget() {
		target = null;
		hitInfo = new RaycastHit();
		reticle.ResetTargetBodyScale();
		focusOnTargetWithoutInterruptionSec = 0;
	}
	
	public float focusOnTargetAlpha {
		get {
			return reticle.focus.completenes;
		}
		set {
			reticle.focus.completenes = value;
		}
	}
}

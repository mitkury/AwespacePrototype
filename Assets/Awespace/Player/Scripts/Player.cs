using UnityEngine;
using System.Collections;
//using WellFired;

namespace Awespace {
	public class Player : MonoBehaviour {

		public Sequence sequence;
		public PlayerTimeline timeline;
		public GameObject body;

		public float progress {
			get {
				return sequence.RunningTime / sequence.Duration;
			}
			set {
				var targetProgress = value < 0 ? 0 : value;
				targetProgress = targetProgress > 1 ? 1 : targetProgress;

				sequence.RunningTime = targetProgress * sequence.Duration;
			}
		}

		public bool isPlaying {
			get {
				return sequence.IsPlaying;
			}
		}

		public void Play() {
			sequence.Play();	
		}

		public void Pause() {
			sequence.Pause();
		}

		void Start () {
			sequence.Play();
		}
		
		void Update () {

			UpdateTimeline();
			
			/*
			if (IsVisible) {
				var relativePointAtDistance = sight.hitInfo.collider != null ? sight.hitInfo.point : collider.transform.position;
				var closestPositionToThing = sight.anchor.position + sight.facingVector * Vector3.Distance(sight.anchor.position, relativePointAtDistance);
				var closestPointOnBounds = collider.bounds.ClosestPoint(closestPositionToThing);
				
				//reticle.transform.position = closestPointOnBounds;	
				reticle.transform.position = Vector3.Lerp(reticle.transform.position, closestPointOnBounds, 30f);
				
				if (Vector3.Distance(closestPointOnBounds, closestPositionToThing) <= 0.01f) {
					timeSinceFocusedOnArea = Time.timeSinceLevelLoad;
				}
				
				float elapsedTimeSinceLastFocus = timeSinceFocusedOnArea != 0f ? Time.timeSinceLevelLoad - timeSinceFocusedOnArea : 0f;
				
				if (elapsedTimeSinceLastFocus >= delayBetweenHiding) { 
					Debug.Log("hide");
				}
			} else {
				
				if (Input.GetMouseButtonDown(0)) {
					Debug.Log("Show");
				}
				
			}
			*/
		}

		void UpdateTimeline() {
			if (sequence.RunningTime == 0)
				return;
			
			timeline.progress = sequence.RunningTime / sequence.Duration;
		}
	}
}
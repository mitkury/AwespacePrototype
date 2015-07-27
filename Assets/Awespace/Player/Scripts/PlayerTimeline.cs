using UnityEngine;
using System.Collections;

namespace Awespace {
	public class PlayerTimeline : MonoBehaviour {
		
		public Transform startPoint;
		public Transform endPoint;
		public Transform progressBar;
		
		float _progress;

		public void OnClick(Vector3 clickPoint) {
			var sequence = transform.parent.GetComponent<Player>().sequence;
			var sequenceWasComplete = sequence.IsComplete;
			var localClickPoint = transform.InverseTransformPoint(clickPoint);
			
			transform.parent.GetComponent<Player>().progress = localClickPoint.x + 0.5f;
			
			if (sequenceWasComplete)
				sequence.Play();

			if (!sequenceWasComplete && !sequence.IsPlaying) {
				sequence.Play();
				sequence.PauseAfterSec(0.0001f);

				//sequence.PlayForOneFrameAt(progress);
			}
		}

		public float progress {
			get {
				return _progress;
			}
			set {
				_progress = value < 0 ? 0 : value;
				_progress = _progress > 1 ? 1 : _progress;
				
				progressBar.localScale = new Vector3(_progress, 1, 1);
			}
		}
	}
}

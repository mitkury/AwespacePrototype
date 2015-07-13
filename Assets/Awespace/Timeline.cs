using UnityEngine;
using System.Collections;

namespace Awespace {

	[ExecuteInEditMode]
	public class Timeline : MonoBehaviour {

		public GameObject target;
		public Sequence sequence;
		public float startTime;
		
		protected bool _isPlaying;

		public bool IsPlaying {
			get {
				return _isPlaying;
			}
		}

		public virtual float Duration {
			get {
				return 0f;
			}
		}
		
		public virtual float EndTime {
			get {
				return startTime + Duration;
			}
		}
		
		public float RunningTime {
			get {
				return sequence.RunningTime - startTime;
			}
		}

		public void Play() {
			Play(0f);
		}

		public void PlayForOneFrameAt(float normalizedTime) {
			StartCoroutine(PlayForOneFrameAtCo(normalizedTime));
		}

		public virtual void Install(Sequence sequence, GameObject target) {
			this.sequence = sequence;
			this.target = target;
		}

		public virtual void Play(float localRunningTime) {
			_isPlaying = true;
		}

		public virtual void Pause() {
			_isPlaying = false;
		}

		protected virtual IEnumerator PlayForOneFrameAtCo(float normalizedTime) {
			Play (normalizedTime * Duration);
			yield return new WaitForEndOfFrame();
			Pause();
		}

		

	}

}

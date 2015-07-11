using UnityEngine;
using System.Collections;
//using WellFired;

namespace Awespace {
	public class Player : MonoBehaviour {
		public Sequence sequence;
		public PlayerTimeline timeline;
		
		void Start () {
			sequence.Play();
		}

		void Update () {
			if (sequence.RunningTime == 0)
				return;

			timeline.progress = sequence.RunningTime / sequence.Duration;
		}

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
	}
}
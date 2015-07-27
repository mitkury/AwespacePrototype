using UnityEngine;
using System.Collections;

namespace Awespace {

	public class TimelineAudio : Timeline {

		public AudioSource audioSource;
		public float turnOffAfterSec;

		public override float Duration {
			get {
				return audioSource.clip.length;
			}
		}

		public override float EndTime {
			get {
				float targetEndTime;
				
				if (audioSource.loop)
					targetEndTime = sequence.Duration;
				else
					targetEndTime = base.EndTime;
				
				if (turnOffAfterSec == 0 || turnOffAfterSec + startTime >= targetEndTime)
					return targetEndTime;
				else
					return turnOffAfterSec + startTime;
			}
		}

		public override void Install (Sequence sequence, GameObject target)
		{
			base.Install (sequence, target);
			audioSource = target.GetComponent<AudioSource>();
		}

		public override void Play (float localRunningTime) {
			base.Play(localRunningTime);

			if (Duration > localRunningTime && !audioSource)
				return;

			audioSource.time = localRunningTime < Duration ? localRunningTime : 0;

			if (RunningTime >= 0 && RunningTime <= EndTime) {
				audioSource.Play();
			}
		}

		public override void Pause ()
		{
			base.Pause ();

			audioSource.Pause();
		}

		protected override IEnumerator PlayForOneFrameAtCo (float normalizedTime)
		{
			// Mute the soundSource in order to not get an unpleasent noize when scrubbing through a paused timeline.
			audioSource.mute = true;
			yield return base.PlayForOneFrameAtCo (normalizedTime);
			audioSource.mute = false;
		}

		void Update() {
			if (!IsPlaying)
				return;
			
			if (turnOffAfterSec > 0) {
				if (RunningTime >= turnOffAfterSec) {
					audioSource.Pause();
					_isPlaying = false;
				}
			}
		}

	}

}

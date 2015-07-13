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

		public override void Play (float localRunningTime) {
			base.Play(localRunningTime);

			if (Duration > localRunningTime && !audioSource)
				return;

			audioSource.time = localRunningTime < Duration ? localRunningTime : 0;

			audioSource.Play();
		}

		public override void Pause ()
		{
			base.Pause ();

			audioSource.Pause();
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

using UnityEngine;
using System.Collections;

namespace Awespace {

	public class TimelineAudio : Timeline {

		public AudioSource audioSource;

		public override float Duration {
			get {
				return audioSource.clip.length;
			}
		}

		public override float EndTime {
			get {
				if (audioSource.loop)
					return sequence.Duration;
				else
					return base.EndTime;
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

	}

}

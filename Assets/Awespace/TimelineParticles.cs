using UnityEngine;
using System.Collections;

namespace Awespace {

	public class TimelineParticles : Timeline {

		public ParticleSystem particleSystem;
		public uint seed = 1;
		public float disableEmissionAfterSec;

		public override float Duration {
			get {
				return particleSystem.duration;
			}
		}

		public override float EndTime {
			get {
				float targetEndTime;

				if (particleSystem.loop)
					targetEndTime = sequence.Duration;
				else
					targetEndTime = base.EndTime;

				if (disableEmissionAfterSec == 0 || disableEmissionAfterSec + startTime >= targetEndTime)
					return targetEndTime;
				else
					return disableEmissionAfterSec + startTime;	
			}
		}

		public override void Install (Sequence sequence, GameObject target)
		{
			base.Install (sequence, target);
			particleSystem.randomSeed = seed;
		}

		public override void Play (float localRunningTime) {
			base.Play(localRunningTime);

			var time = localRunningTime < Duration || particleSystem.loop ? localRunningTime : 0;

			particleSystem.enableEmission = true;
			particleSystem.Simulate(time, true, true);
			particleSystem.Play();
		}
		
		public override void Pause ()
		{
			base.Pause ();
			
			particleSystem.Pause();
		}

		void Update() {
			if (!IsPlaying)
				return;

			if (disableEmissionAfterSec > 0) {
				if (RunningTime >= disableEmissionAfterSec) {
					particleSystem.enableEmission = false;
					_isPlaying = false;
				}
			}
		}

	}

}

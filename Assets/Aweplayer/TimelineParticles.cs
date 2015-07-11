using UnityEngine;
using System.Collections;

namespace Awespace {

	public class TimelineParticles : Timeline {

		public ParticleSystem particleSystem;
		public uint seed = 1;

		public override float Duration {
			get {
				return particleSystem.duration;
			}
		}

		public override float EndTime {
			get {
				if (particleSystem.loop)
					return sequence.Duration;
				else
					return base.EndTime;
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

			particleSystem.Simulate(time, true, true);
			particleSystem.Play();
		}
		
		public override void Pause ()
		{
			base.Pause ();
			
			particleSystem.Pause();
		}

	}

}

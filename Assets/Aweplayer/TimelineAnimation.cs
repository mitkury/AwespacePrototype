using UnityEngine;
using System.Collections;
using System.Linq;

namespace Awespace {

	public class TimelineAnimation : Timeline {

		public Animator animator;
		public int layer;
		public string stateName;
		public float animationClipLength = 0;

		public override float Duration {
			get {
				return animationClipLength;
			}
		}

		public override void Play (float localRunningTime) {
			base.Play(localRunningTime);

			var normalizedTime = localRunningTime / Duration;
			animator.Play(stateName, layer, normalizedTime);
			animator.speed = 1f;
		}

		public override void Pause ()
		{
			base.Pause();

			animator.speed = 0f;
		}
		
		AnimationClip GetAnimationClip() {
			#if UNITY_EDITOR
			if (animator.runtimeAnimatorController is UnityEditor.Animations.AnimatorController) {
				var animatorController = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
				var stateMachine = animatorController.layers[layer].stateMachine;
				
				var state = stateMachine.states.First(cs => cs.state.name == stateName).state;
				var clip = animatorController.animationClips.First(c => c.name == state.motion.name);
				
				return clip;
			}
			#endif
			
			return null;
		}

		#if UNITY_EDITOR
		void Update() {
			if (!Application.isPlaying) {
				animationClipLength = GetAnimationClip().length;
			}
		}
		#endif
	}

}

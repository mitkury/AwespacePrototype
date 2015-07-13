using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Awespace {

	[ExecuteInEditMode]
	public class TimelineContainer : MonoBehaviour {

		public GameObject target;
		public List<Timeline> timelines;

		public float EndTime {
			get {
				return timelines.OrderBy(t => t.EndTime).ToList().Last().EndTime;
			}
		}

		public void Install(Sequence sequence) {
			foreach (var timeline in timelines) {
				timeline.Install(sequence, target);
			}
		}

		#if UNITY_EDITOR
		void Update() {
			if (!Application.isPlaying) {
				timelines = GetComponentsInChildren<Timeline>().ToList();
			}
		}
		#endif

		/*
		public Animator animator;
		public int layer;
		public string stateName;
		public float startTime;
		public float animationClipLength = 0;

		bool _isPlaying;
		float _runningTime;

		AnimationClip GetMotionAnimationClip() {
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
				animationClipLength = GetMotionAnimationClip().length;
			}
		}
		#endif

		public void Play(float localRunningTime) {
			_isPlaying = true;
			_runningTime = localRunningTime;
			var normalizedTime = localRunningTime / Duration;

			animator.Play(stateName, layer, normalizedTime);
			animator.speed = 1f;
		}

		public void Play() {
			Play(0f);
		}

		public void Pause() {
			_isPlaying = false;

			animator.speed = 0f;
		}

		public bool IsPlaying {
			get {
				return _isPlaying;
			}
		}

		public float Duration {
			get {
				return animationClipLength;
			}
		}

		public float EndTime {
			get {
				return startTime + Duration;
			}
		}

		public float RunningTime {
			get {
				return _runningTime;
			}
		}
		*/

	}

}
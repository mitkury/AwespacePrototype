using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Awespace {

	[ExecuteInEditMode]
	public class Sequence : MonoBehaviour {

		public enum TimelineStatus {
			Future,
			Present,
			Past
		}

		[HideInInspector]
		public List<TimelineContainer> containers;
		//[HideInInspector]
		public float _duration;

		bool _isPlaying;
		bool _isInitialized;
		float _runningTime = 0;
		float runningTimeToStopAt;

		public bool IsPlaying {
			get; private set;
		}

		public bool IsComplete { 
			get {
				return _runningTime >= _duration;
			}
		}

		public float RunningTime {
			get {
				return _runningTime;
			}
			set {
				if (_runningTime == value)
					return;

				_runningTime = value;
		
				foreach (var container in containers) {
					foreach (var timeline in container.timelines) {
						timeline.Pause();

						// Check here if the timeline needs to be rewined to "before" or "past" states.

						switch (GetTimelineStatusRelatedToRunningTime(timeline)) {
						case TimelineStatus.Future:
							//Debug.Log("Future: "+timeline);
							timeline.PlayForOneFrameAt(0f);
							break;
						case TimelineStatus.Past:
							//Debug.Log("Past: "+timeline);
							timeline.PlayForOneFrameAt(1f);
							break;
						}
					}
				}
			}
		}

		public float Duration {
			get {
				return _duration;
			}
		}

		public void Initialize() {
			foreach (var container in containers) {
				container.Install(this);
			}
		}

		public void Play() {
			if (!_isInitialized)
				Initialize();

			IsPlaying = true;
		}
		
		public void Pause() {
			IsPlaying = false;
			
			foreach (var container in containers) {
				foreach (var timeline in container.timelines) {
					timeline.Pause();
				}
			}
		}
		
		public void PauseAfterSec(float seconds) {
			runningTimeToStopAt = _runningTime + seconds;
		}

		public TimelineStatus GetTimelineStatusRelatedToRunningTime(Timeline timeline) {
			if (_runningTime < timeline.startTime)
				return TimelineStatus.Future;
			
			if (_runningTime >= timeline.startTime && _runningTime <= timeline.EndTime)
				return TimelineStatus.Present;
			else
				return TimelineStatus.Past;
		}

		void Start() {
			Initialize();
		}

		void Update() {
			#if UNITY_EDITOR
			if (!Application.isPlaying) {
				containers = GetComponentsInChildren<TimelineContainer>().ToList();

				foreach (var container in containers) {
					if (container.timelines == null || container.timelines.Count == 0)
						continue;

					foreach (var timeline in container.timelines) {
						timeline.sequence = this;
					}
				}

				var minDuration = GetMinDuration();

				_duration = _duration < minDuration ? minDuration : _duration;
				
				return;
			}
			#endif
			
			if (IsComplete) {
				IsPlaying = false;
			}
			
			if (!IsPlaying)
				return;

			foreach (var container in containers) {
				foreach (var timeline in container.timelines) {
					if (!timeline.IsPlaying && GetTimelineStatusRelatedToRunningTime(timeline) == TimelineStatus.Present) {
						timeline.Play(_runningTime - timeline.startTime);
					}
				}
			}
			
			_runningTime += Time.deltaTime;
			
			if (runningTimeToStopAt > 0 && _runningTime >= runningTimeToStopAt) {
				Pause();
				
				runningTimeToStopAt = 0;
			}
		}
		
		float GetMinDuration() {
			if (containers.Count == 0)
				return 0;
			
			float duration = containers.OrderBy(t => t.EndTime).ToList().Last().EndTime;
			return duration;
		}

	}

}



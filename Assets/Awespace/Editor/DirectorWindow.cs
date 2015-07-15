using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Awespace {

	public class DirectorWindow : EditorWindow {

		public int timeScale = 30;

		Sequence sequence;
		List<GameObject> draggedInObjects = new List<GameObject>();
		bool dragIsPerformed;

		[MenuItem ("Window/Awespace Director")]
		static void Init () {
			DirectorWindow window = (DirectorWindow)EditorWindow.GetWindow (typeof (DirectorWindow));
			window.Show();
			window.titleContent = new GUIContent("Director");
		}

		public TimelineContainer CreateTimelineContainer(GameObject target) {
			var container = HComponent.Create<TimelineContainer>(target.name+" (Timelines)");

			container.transform.parent = sequence.transform;

			if (target.GetComponent<AudioSource>() != null) {
				var audioTimeline = HComponent.Create<TimelineAudio>("Audio");

				audioTimeline.transform.parent = container.transform;
			}

			container.Install(target, sequence);

			return container;
		}
		
		void OnGUI () {
			UpdateDraggedInObjects();
			UpdateTimelineGUI();

			if (dragIsPerformed && draggedInObjects.Count > 0) {
				foreach (var target in draggedInObjects) {
					var targetContainer = sequence.containers.Find(c => c.target == target);

					if (targetContainer == null) {
						Debug.Log("Create a container for "+target.name);
						targetContainer = CreateTimelineContainer(target);
					}

					Debug.Log(targetContainer);
				}
			}
		}

		void UpdateTimelineGUI() {
			if (sequence == null)
				sequence = GameObject.FindObjectOfType<Sequence>();
			
			if (sequence == null)
				return;
			
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, 20));
			
			timeScale = EditorGUILayout.IntField("Time Scale", timeScale, GUILayout.Width(200));
			
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, Screen.width, Screen.height - 20));
			
			/*
			 * GameObjects
			 */
			int timelineHeight = 20;
			int elementsAreaWidth = 200;
			
			GUILayout.BeginArea(new Rect(0, 0, elementsAreaWidth, 500));
			
			int pos = 0;
			
			foreach(var container in sequence.containers) {
				
				GUILayout.BeginArea(new Rect(0, timelineHeight * pos, elementsAreaWidth, timelineHeight));
				GUILayout.Label (container.name, EditorStyles.boldLabel);
				GUILayout.EndArea();
				
				pos += 1;
				
				foreach (var timeline in container.timelines) {
					GUILayout.BeginArea(new Rect(0, timelineHeight * pos, elementsAreaWidth, timelineHeight));
					GUILayout.Label (timeline.name, EditorStyles.miniLabel);
					GUILayout.EndArea();
					
					pos += 1;
				}
			}
			GUILayout.EndArea();
			
			/*
			 * Timelines
			 */
			pos = 0;
			
			int timelinesAreaWidth = (int)sequence.Duration * timeScale;
			
			GUILayout.BeginArea(new Rect(200, 0, timelinesAreaWidth, timelinesAreaWidth));
			
			foreach(var container in sequence.containers) {
				pos += 1;
				
				foreach (var timeline in container.timelines) {
					GUILayout.BeginArea(new Rect(0, timelineHeight * pos, timelinesAreaWidth, timelineHeight));
					
					int left = (int)timeline.startTime * timeScale;
					int width = (int)timeline.EndTime * timeScale - left;
					
					GUILayout.BeginArea(new Rect(left, 0, width, timelineHeight), EditorStyles.helpBox);
					//GUILayout.Box(EditorStyles.bo);
					GUILayout.EndArea();
					
					GUILayout.EndArea();
					
					pos += 1;
				}
			}

			// Draw running time line
			
			Handles.color = Color.red;
			Handles.DrawLine(new Vector3(sequence.RunningTime * timeScale, 0), new Vector3(sequence.RunningTime * timeScale, Screen.height));
			
			GUILayout.EndArea();
			
			GUILayout.EndArea();
		}

		void UpdateDraggedInObjects() {
			Event evt = Event.current;

			draggedInObjects.Clear();
			dragIsPerformed = false;
			
			switch (evt.type)  {
			case EventType.DragUpdated:
			case EventType.DragPerform:
				
				foreach (Object dO in DragAndDrop.objectReferences) {
					if (dO is GameObject)
					{
						draggedInObjects.Add(dO as GameObject);
					}
				}
				
				if (draggedInObjects.Count > 0) {
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					
					if (evt.type == EventType.DragPerform) {
						dragIsPerformed = true;
					}
				}
				
				break;
			}
		}

		void Update() {
			Repaint();
		}
	}

}
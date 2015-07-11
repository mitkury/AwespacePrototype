using UnityEngine;
using UnityEditor;

namespace Awespace {

	public class DirectorWindow : EditorWindow {

		public int timeScale = 30;

		Sequence sequence;

		[MenuItem ("Window/Awespace Director")]
		static void Init () {
			DirectorWindow window = (DirectorWindow)EditorWindow.GetWindow (typeof (DirectorWindow));
			window.Show();
			window.titleContent = new GUIContent("Director");
		}
		
		void OnGUI () {

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


					GUILayout.BeginArea(new Rect((int)timeline.startTime * timeScale, 0, timeline.Duration * timeScale, timelineHeight), EditorStyles.helpBox);
					//GUILayout.Box(EditorStyles.bo);
					GUILayout.EndArea();

					GUILayout.EndArea();

					pos += 1;
				}
			}

			GUILayout.EndArea();

			GUILayout.EndArea();
		}

		void Update() {
			Repaint();
		}
	}

}
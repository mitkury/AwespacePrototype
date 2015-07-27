using UnityEngine;
using System.Collections;

namespace Awespace {
	public class PlayButton : MonoBehaviour {

		public GameObject playIcon;
		public GameObject pauseIcon;

		Player player;

		public void OnClick(Vector3 pointClick) {
			if (player.sequence.IsPlaying)
				player.sequence.Pause();
			else {
				if (player.sequence.IsComplete) {
					player.sequence.RunningTime = 0;
				}

				player.sequence.Play();
			}
		}

		void Start() {
			player = transform.parent.GetComponent<Player>();
			playIcon.SetActive(false);
			pauseIcon.SetActive(false);
		}

		void Update() {
			if (player.sequence.IsPlaying) {
				playIcon.SetActive(false);
				pauseIcon.SetActive(true);
			}
			else {
				playIcon.SetActive(true);
				pauseIcon.SetActive(false);
			}
		}

	}
}

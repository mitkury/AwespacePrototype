using UnityEngine;
using System.Collections;

namespace Awespace {
	public class PlayButton : MonoBehaviour {

		public void OnClick(Vector3 pointClick) {
			var player = transform.parent.GetComponent<Player>();

			if (player.sequence.IsPlaying)
				player.sequence.Pause();
			else
				player.sequence.Play();
		}

	}
}

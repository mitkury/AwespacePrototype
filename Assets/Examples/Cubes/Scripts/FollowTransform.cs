using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {

	public Transform target;
	
	void Update () {
		transform.position = target.position;
	}
}

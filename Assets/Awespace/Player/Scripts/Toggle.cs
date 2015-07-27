using UnityEngine;
using System.Collections;

public class Toggle : MonoBehaviour {

	public GameObject body;
	public Transform hidingPoint;
	public float delayBetweenHiding = 2f;

	ReticleArea reticleArea;
	Vector3 showPosition;
	Vector3 hidePosition;
	Vector3 initScale;
	
	public bool IsVisible { get; private set; }

	public void SetVisibility(bool show) {
		IsVisible = show;

		LeanTween.cancel(body);

		Vector3 targetPosition = showPosition;
		Vector3 targetScale = initScale;
		LeanTweenType targetEase = LeanTweenType.easeInCubic;

		if (!show) {
			targetPosition = hidePosition;
			targetScale = Vector3.zero;
			//targetEase = LeanTweenType.easeInSine;
		} else { 
			reticleArea.TimeSinceFocusedOnArea = Time.timeSinceLevelLoad;
			body.SetActive(true);
		}

		LeanTween.move(body, targetPosition, 0.5f).setEase(targetEase).setOnComplete(delegate() {
			if (!show) { 
				body.SetActive(false);
			}
		});

		LeanTween.scale(body, targetScale, 0.5f).setEase(targetEase);
	}

	void Start () {
		showPosition = body.transform.position;
		hidePosition = hidingPoint.position;
		initScale = body.transform.localScale;

		body.transform.position = hidingPoint.position;
		body.SetActive(false);

		reticleArea = body.GetComponent<ReticleArea>();

		if (reticleArea == null) {
			Debug.LogError("Couldn't find a ReticleArea component on the body");
		}

		SetVisibility(true);
	}

	void Update () {
		UpdateVisibility();
	}

	void UpdateVisibility() {
		if (IsVisible) {
			if (reticleArea == null)
				return;

			// TODO: turn the toggle off after the delay even if the elapsed time is equal 0.
			//(reticleArea.elapsedTimeSinceLastFocus == 0 && Time.timeSinceLevelLoad - visibleSinceTime >= delayBetweenHiding)

			#if UNITY_ANDROID
			if (reticleArea.ElapsedTimeSinceLastFocus >= delayBetweenHiding) {
				SetVisibility(false);
			} else {

				// Hide when has clicked roughly outside of the reticleArea, i.e. elapsed time since it was in focus is greater than a time of a few frames.
				if (Input.GetMouseButtonDown(0) && reticleArea.ElapsedTimeSinceLastFocus > Time.deltaTime * 5f) {
					SetVisibility(false);
				}

			}
			#endif

			#if UNITY_STANDALONE
			if (reticleArea.ElapsedTimeSinceMovedReticle >= 2f) {
				SetVisibility(false);
			}
			#endif
		} else {
			if (Input.GetMouseButtonDown(0)) {
				SetVisibility(true);
			}
			#if UNITY_STANDALONE
				#if UNITY_EDITOR
				if (!Input.GetKey(KeyCode.LeftAlt))
					return;
				#endif

			if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
				SetVisibility(true);
			}
			#endif
		}
	}
}

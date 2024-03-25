using System;
using System.Collections;
using UnityEngine;

namespace ShaunGoh {
	public class ObjectFollower : MonoBehaviour {
		public Transform target;
		public bool position = true, rotation = true;
		public float transitionDuration = 0.5f;
		private bool inTransition;
		private Coroutine cache;
		private void Update() {
			if(inTransition || !target){ return; }
			if (position) {
				transform.position = target.position;
			}
			if (rotation) {
				transform.rotation = target.rotation;
			}
		}
		public void TransitionTo(Transform point, Action afterTranstion = null) {
			if(null != cache) { StopCoroutine(cache); }
			//inTransition = true;
			cache = StartCoroutine(MoveToPoint(point, afterTranstion));
		}
		private IEnumerator MoveToPoint(Transform point, Action afterTranstion) {
			inTransition = true;
			target = point;
			float curr = 0;
			Vector3 startpos = transform.position;
			while (curr < 1) {
				curr = transitionDuration <= 0 ? 1 : Mathf.MoveTowards(curr, 1, Time.deltaTime / transitionDuration);
				transform.position = Vector3.Lerp(startpos, target.position, curr);
				//transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, curr);
				yield return null;
			}
			inTransition = false;
			afterTranstion?.Invoke();
		}
	}
}
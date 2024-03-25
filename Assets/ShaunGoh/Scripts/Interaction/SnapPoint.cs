using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShaunGoh {
	public class SnapPoint : MonoBehaviour {
		public List<string> snapTags;
		public float transitionDuration = 0.2f;
		public Transform targetPoint;
		public List<SnapPoint> dependencies;
		private List<SnapPoint> dependents;
		public UnityEvent SnapFinished;

		private List<Collider> collidedObjects;
		private static List<SnapPoint> triggeredPoints;
		private I_Interactable heldObject;
		//private bool playingTransition;
		public static void CheckTriggeredPoints(I_Interactable interactable) {
			if (null == triggeredPoints) { return; }
			int tcount = triggeredPoints.Count;
			if (tcount <= 0) { return; }
			MonoBehaviour mono = interactable as MonoBehaviour;
			if (null == mono) { return; }
			Transform itrans = mono.transform;
			List<SnapPoint> points = new();
			for (int i = 0; i < tcount;i++) {
				SnapPoint point = triggeredPoints[i];
				if (!point.CheckDependencies()) { continue; }
				for (int c = 0, cmax = point.collidedObjects.Count; c < cmax; c++) {
					I_Interactable col = point.collidedObjects[c].GetComponent<I_Interactable>();
					if (null != interactable && interactable == col) {
						points.Add(point);
						break;
					}
				}
			}
			float closestDist = Mathf.Infinity;
			SnapPoint closestPoint = null;
			for (int i = 0, max = points.Count; i < max; i++) {
				SnapPoint point = points[i];
				float dist = point.CheckDist(itrans);
				if (dist < closestDist) { 
					closestDist = dist;
					closestPoint = point;
				}
			}
			if (closestPoint) {
				closestPoint.SnapInteractable(interactable, itrans);
			}
		}
		private bool CheckDependencies() {
			if (dependencies.Count <= 0) return true;
			int fulfilled = 0;
			for (int i = 0, max = dependencies.Count; i < max; i++) { 
				SnapPoint point = dependencies[i];
				if (null != point.heldObject) {
					if (!point.dependents.Contains(this)) { 
						point.dependents.Add(this);
					}
					fulfilled++;
				}
			}
			return fulfilled == dependencies.Count;
		}
		private float CheckDist(Transform target) {
			return Vector3.SqrMagnitude(transform.position - target.position);
		}
		private void SnapInteractable(I_Interactable interactable, Transform targetTrans) {
			if (null == interactable || null == targetTrans || null == snapTags || !snapTags.Contains(interactable.SnapTag)) { return; }
			heldObject = interactable;
			interactable.FreezeInteraction(null);
			interactable.OnStartInteract += InteractionStarted;
			StartCoroutine(WaitSnap(targetTrans));
		}
		private void InteractionStarted() {
			if (null == heldObject) { return; }
			heldObject.OnStartInteract -= InteractionStarted;
			heldObject.StartInteraction(null);
			heldObject = null;
			int dcount = dependents.Count;
			if (dcount > 0) {
				for (int i = 0; i < dcount; i++) { 
					SnapPoint dependent = dependents[i];
					dependent.InteractionStarted();
				}
			}
		}
		private IEnumerator WaitSnap(Transform target) {
			//playingTransition = true;
			float curr = 0;
			Vector3 startpos = target.position;
			Quaternion startrot = target.rotation;
			while (curr < 1) {
				curr = transitionDuration <= 0 ? 1 : Mathf.MoveTowards(curr, 1, Time.deltaTime / transitionDuration);
				target.position = Vector3.Lerp(startpos, targetPoint.position, curr);
				target.rotation = Quaternion.Lerp(startrot, targetPoint.rotation, curr);
				yield return null;
			}
			//playingTransition = false;
			SnapFinished.Invoke();
		}
		private void Awake() {
			collidedObjects = new();
			dependents = new();
			if (!targetPoint) { targetPoint = transform; }
		}
		private void FixedUpdate() {
			if (collidedObjects.Count > 0) {
				if (null == triggeredPoints) {
					triggeredPoints = new();
				}
				if(!triggeredPoints.Contains(this)) {
					triggeredPoints.Add(this);
				}
				collidedObjects.Clear();
			} else {
				if (null != triggeredPoints && triggeredPoints.Contains(this)) { 
					triggeredPoints.Remove(this);
				}
			}
		}
		private void OnTriggerStay(Collider other) {
			if (!CheckDependencies() || null != heldObject) { return; }
			if (collidedObjects.Contains(other)) { return; }
			collidedObjects.Add(other);
		}
	}
}
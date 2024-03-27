using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShaunGoh {
	public class InteractablePickup : MonoBehaviour, I_Interactable {
		public Rigidbody rb;
		public float minZoomDist;
		public InteractableType Itype{ get { return InteractableType.Pickable; } }
		public string snapTag;
		public string SnapTag { get { return snapTag; } }
		public UnityEvent OnInteractStart, OnInteractStop;
		public event Action OnStartInteract, OnStopInteract;
		private bool rotating;
		private List<Collider> colliders;
		private List<LayerMask> startingMasks;
		private LayerMask ignoreMask;
		private Transform originalParent;

		private void Awake() {
			ignoreMask = LayerMask.NameToLayer("Ignore Raycast");
			originalParent = transform.parent;
			Collider[] objectcol = this.GetComponents<Collider>();
			Collider[] childCol = this.GetComponentsInChildren<Collider>();
			colliders = new();
			colliders.AddRange(objectcol);
			colliders.AddRange(childCol);
			startingMasks = new();
			for (int i = 0, max = colliders.Count; i < max; i++) {
				startingMasks.Add(colliders[i].gameObject.layer);
			}
		}
		public void StartInteraction(Interactor interactor) {
			if (!interactor) { return; }
			rb.isKinematic = true;
			for(int i = 0, max = colliders.Count; i < max; i++) {
				colliders[i].gameObject.layer = ignoreMask;
				colliders[i].isTrigger = true;
			}
			interactor.holdpoint.rotation = interactor.transform.root.rotation;
			transform.SetParent(interactor.holdpoint);
			OnStartInteract?.Invoke();
			OnInteractStart.Invoke();
		}
		private bool StopCommon(Interactor interactor) {
			if (!interactor) { return false; }
			for (int i = 0, max = colliders.Count; i < max; i++) {
				colliders[i].gameObject.layer = startingMasks[i];
				colliders[i].isTrigger = false;
			}
			transform.SetParent(originalParent, true);
			SnapPoint.CheckTriggeredPoints(this);
			OnStopInteract?.Invoke();
			OnInteractStop.Invoke();
			return true;
		}
		public void StopInteraction(Interactor interactor) {
			if (!StopCommon(interactor)) { return; }
			rb.isKinematic = false;
			rb.AddForce(Vector3.up);
		}
		public void FreezeInteraction(Interactor interactor) {
		}
		private Bounds MaxBounds(List<Collider> cols) {
			Bounds b = new Bounds(transform.position, Vector3.zero);
			foreach (Collider col in cols) {
				b.Encapsulate(col.bounds);
			}
			return b;
		}
		public void ConstantInteraction(Interactor interactor) {
			if (!interactor) { return; }
			switch (ProjectUtils.playState) {
				case PlayerState.RotateObject:
				case PlayerState.Focused:

					float hori = Input.GetAxisRaw("Horizontal");
					float vert = Input.GetAxisRaw("Vertical");
					float roll = Input.GetAxisRaw("Roll");
					if (hori == 0 && vert == 0 && roll == 0) {
						if (rotating) {
							transform.SetParent(null);
							interactor.holdpoint.rotation = interactor.transform.root.rotation;
							transform.SetParent(interactor.holdpoint);
							rotating = false;
						}
						break;
					}
					float timepassed = Time.deltaTime;
					interactor.holdpoint.Rotate(Vector3.up, -hori * timepassed * ProjectUtils.pickupRotateSpeed);
					interactor.holdpoint.Rotate(Vector3.right, vert * timepassed * ProjectUtils.pickupRotateSpeed);
					interactor.holdpoint.Rotate(Vector3.forward, roll * timepassed * ProjectUtils.pickupRotateSpeed);
					rotating = true;
					break;
				default: break;
			}
			if (!rotating) {
				interactor.holdpoint.rotation = interactor.transform.root.rotation;
			}
			RaycastHit? hit = interactor.CastRay();
			if (null != hit) {
				Bounds bounds = MaxBounds(colliders);
				Vector3 temppos = hit.Value.point;
				temppos.x += bounds.size.x / 2 * hit.Value.normal.x;
				temppos.y += bounds.size.y / 2 * hit.Value.normal.y;
				temppos.z += bounds.size.z / 2 * hit.Value.normal.z;
				interactor.holdpoint.position = temppos;
				transform.position = temppos;
			}
		}
	}
}
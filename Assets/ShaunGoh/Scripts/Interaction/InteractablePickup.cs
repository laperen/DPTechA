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
		private Vector3 cacheOffset;
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
			MaxBounds(colliders);
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
			cacheOffset = b.size / 2;
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
					if (hori != 0) {
						interactor.holdpoint.Rotate(Vector3.up, -hori * timepassed * ProjectUtils.pickupRotateSpeed);
					}
					if (vert != 0) {
						interactor.holdpoint.Rotate(Vector3.right, vert * timepassed * ProjectUtils.pickupRotateSpeed);
					}
					if (roll != 0) {
						interactor.holdpoint.Rotate(Vector3.forward, roll * timepassed * ProjectUtils.pickupRotateSpeed);
					}
					rotating = true;
					break;
				default: break;
			}
			if (!rotating) {
				interactor.holdpoint.rotation = interactor.transform.root.rotation;
			}
			RaycastHit? hit = interactor.CastRay();
			if (null != hit) {
				Vector3 p = hit.Value.point;
				Vector3 n = hit.Value.normal;
				interactor.hitmark.position = p;
				interactor.hitmark.rotation = Quaternion.LookRotation(n);
				if (rotating) {
					MaxBounds(colliders);
				}
				Vector3 temppos = p;
				temppos.x += cacheOffset.x * n.x;
				temppos.y += cacheOffset.y * n.y;
				temppos.z += cacheOffset.z * n.z;
				interactor.holdpoint.position = temppos;
				transform.position = temppos;
			}
		}
	}
}
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
		private Vector3 cacheOffset;
		private float holdist;
		private void SetColliders(bool trigger) {
			for (int i = 0, max = colliders.Count; i < max; i++) {
				colliders[i].gameObject.layer = trigger ? ignoreMask : startingMasks[i];
				colliders[i].isTrigger = trigger;
			}

		}
		private void Awake() {
			ignoreMask = LayerMask.NameToLayer("Ignore Raycast");
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
			interactor.holdpoint.transform.rotation = interactor.transform.root.rotation;
			interactor.holdpoint.transform.position = rb.position;
			interactor.holdpoint.connectedBody = rb;
			MaxBounds(colliders);
			switch (ProjectUtils.grabState) {
				case GrabState.Placement:
					SetColliders(true);
					interactor.hitmark.gameObject.SetActive(true);
					break;
				case GrabState.Freehold:
					interactor.grabPoint.position = rb.position;
					break;
			}
			OnStartInteract?.Invoke();
			OnInteractStart.Invoke();
		}
		private bool StopCommon(Interactor interactor) {
			if (!interactor) { return false; }
			SetColliders(false);
			interactor.holdpoint.connectedBody = null;
			interactor.hitmark.gameObject.SetActive(false);
			SnapPoint.CheckTriggeredPoints(this);
			OnStopInteract?.Invoke();
			OnInteractStop.Invoke();
			return true;
		}
		public void StopInteraction(Interactor interactor) {
			if (!StopCommon(interactor)) { return; }
			rb.AddForce(Vector3.up);
		}
		public void SwitchInteraction(Interactor interactor) {
			switch (ProjectUtils.grabState) {
				case GrabState.Placement:
					interactor.grabPoint.position = interactor.holdpoint.transform.position;
					ProjectUtils.SetGrabState(GrabState.Freehold);
					SetColliders(false);
					interactor.hitmark.gameObject.SetActive(false);
					break;
				case GrabState.Freehold:
					ProjectUtils.SetGrabState(GrabState.Placement);
					SetColliders(true);
					interactor.hitmark.gameObject.SetActive(true);
					break;
				default:
					break;
			}
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
							interactor.holdpoint.connectedBody = null;
							interactor.holdpoint.transform.rotation = interactor.transform.root.rotation;
							interactor.holdpoint.connectedBody = rb;
							rotating = false;
						}
						break;
					}
					float timepassed = Time.deltaTime;
					if (hori != 0) {
						interactor.holdpoint.transform.Rotate(Vector3.up, -hori * timepassed * ProjectUtils.pickupRotateSpeed);
					}
					if (vert != 0) {
						interactor.holdpoint.transform.Rotate(Vector3.right, vert * timepassed * ProjectUtils.pickupRotateSpeed);
					}
					if (roll != 0) {
						interactor.holdpoint.transform.Rotate(Vector3.forward, roll * timepassed * ProjectUtils.pickupRotateSpeed);
					}
					rotating = true;
					break;
				default: break;
			}
			if (!rotating) {
				interactor.holdpoint.transform.rotation = interactor.transform.root.rotation;
			}
			switch (ProjectUtils.grabState) {
				case GrabState.Placement:
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
						interactor.holdpoint.transform.position = temppos;
					}
					break;
				case GrabState.Freehold:
					float scroll = Input.GetAxis("Mouse ScrollWheel");
					if(scroll != 0f) {
						holdist = interactor.grabPoint.transform.localPosition.magnitude;
						Vector3 holdir = interactor.grabPoint.transform.localPosition.normalized;
						holdist *= 1 + (scroll * ProjectUtils.pickupZoomScale);
						holdist = holdist > minZoomDist? holdist : minZoomDist;
						interactor.grabPoint.transform.localPosition = holdir * holdist;
					}
					interactor.holdpoint.transform.position = interactor.grabPoint.position;
					break;
			}
		}
	}
}
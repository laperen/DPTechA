using System;
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

		public void StartInteraction(Interactor interactor) {
			rb.isKinematic = false;
			if (!interactor) { return; }
			interactor.holdpoint.transform.position = rb.position;
			interactor.holdpoint.connectedBody = rb;
			interactor.downIndicatorObj.SetActive(true);
			OnStartInteract?.Invoke();
			OnInteractStart.Invoke();
		}
		private void StopCommon(Interactor interactor) {
			if (!interactor) { return; }
			interactor.downIndicatorObj.SetActive(false);
			interactor.holdpoint.connectedBody = null;
			SnapPoint.CheckTriggeredPoints(this);
			OnStopInteract?.Invoke();
			OnInteractStop.Invoke();
		}
		public void StopInteraction(Interactor interactor) {
			StopCommon(interactor);
			rb.AddForce(Vector3.up);
		}
		public void FreezeInteraction(Interactor interactor) {
			rb.isKinematic=true;
			StopCommon(interactor);
		}
		public void ConstantInteraction(Interactor interactor) {
			interactor.downIndicator.DrawFromPos(transform.position);
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			if (scroll != 0) {
				float holdist = interactor.holdpoint.transform.localPosition.magnitude;
				Vector3 holdir = interactor.holdpoint.transform.localPosition.normalized;
				holdist *= 1 + (scroll * ProjectUtils.pickupZoomScale);
				holdist = holdist > minZoomDist ? holdist : minZoomDist;
				interactor.holdpoint.transform.localPosition = holdir * holdist;
			}
			switch (ProjectUtils.playState) {
				case PlayerState.RotateObject:
				case PlayerState.Focused:
					float hori = Input.GetAxisRaw("Horizontal");
					float vert = Input.GetAxisRaw("Vertical");
					float roll = Input.GetAxisRaw("Roll");
					if (hori == 0 && vert == 0 && roll == 0) {
						if (rotating) {
							interactor.holdpoint.connectedBody = null;
							interactor.holdpoint.transform.localRotation = Quaternion.identity;
							interactor.holdpoint.connectedBody = rb;
							rotating = false;
						}
						break;
					}
					float timepassed = Time.deltaTime;
					interactor.holdpoint.transform.Rotate(Vector3.up, -hori * timepassed * ProjectUtils.pickupRotateSpeed);
					interactor.holdpoint.transform.Rotate(Vector3.right, vert * timepassed * ProjectUtils.pickupRotateSpeed);
					interactor.holdpoint.transform.Rotate(Vector3.forward, roll * timepassed * ProjectUtils.pickupRotateSpeed);
					rotating = true;
					break;
				default: break;
			}
			/*
			switch (ProjectUtils.playState) {
				case PlayerState.Focused:
					float vert = Input.GetAxisRaw("Vertical");
					Vector3 holdir = interactor.holdpoint.transform.localPosition.normalized;
					break;
				default: break;
			}
			*/
		}
	}
}
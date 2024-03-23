using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

namespace ShaunGoh {
	public class InteractablePickup : MonoBehaviour, I_Interactable {
		public Rigidbody rb;
		public InteractableType Itype{ get { return InteractableType.Pickable; } set { } }
		public UnityEvent OnInteractStart, OnInteractStop;
		private bool rotating;

		public void StartInteraction(Interactor interactor) {
			rb.isKinematic = false;
			interactor.holdpoint.transform.position = rb.position;
			interactor.holdpoint.connectedBody = rb;
			OnInteractStart.Invoke();
		}
		public void StopInteraction(Interactor interactor) {
			interactor.holdpoint.connectedBody = null;
			rb.AddForce(Vector3.up);
		}
		public void FreezeInteraction(Interactor interactor) {
			rb.isKinematic=true;
			interactor.holdpoint.connectedBody = null;
		}
		public void ConstantInteraction(Interactor interactor) {
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			if (scroll != 0) {
				float holdist = interactor.holdpoint.transform.localPosition.magnitude;
				Vector3 holdir = interactor.holdpoint.transform.localPosition.normalized;
				holdist *= 1 + (scroll * ProjectUtils.pickupZoomSpeed);
				interactor.holdpoint.transform.localPosition = holdir * holdist;
			}
			switch (ProjectUtils.playState) {
				case PlayerState.RotateObject:
					float hori = Input.GetAxisRaw("Horizontal");
					float vert = Input.GetAxisRaw("Vertical");
					if (hori == 0 && vert == 0) {
						if (rotating) {
							interactor.holdpoint.connectedBody = null;
							interactor.holdpoint.transform.localRotation = Quaternion.identity;
							interactor.holdpoint.connectedBody = rb;
							rotating = false;
						}
						break;
					}
					interactor.holdpoint.transform.Rotate(Vector3.up, -hori * ProjectUtils.pickupRotateSpeed);
					interactor.holdpoint.transform.Rotate(Vector3.right, vert * ProjectUtils.pickupRotateSpeed);
					rotating = true;
					break;
				default: break;
			}
		}
	}
}
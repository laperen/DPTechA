using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaunGoh {
	public class Interactor : MonoBehaviour {
		private Camera cam;
		public LayerMask rayMask;
		public int raydist;
		private RaycastHit hit;

		public FixedJoint holdpoint;
		private Rigidbody targetrb;
		private InteractableBase interactable;
		private InteractableLink iLink;


		private void Awake() {
			cam = Camera.main;
		}
		private void Update() {
			if (ProjectUtils.inMenu) { return; }

			if (Input.GetButtonDown("Fire1")) {
				DoInteraction();
			}
			if (Input.GetButtonUp("Fire1")) {
				StopInteraction();
			}
			if (Input.GetButtonDown("Fire2")) {
				ProjectUtils.HideCursor();
			}
			if (Input.GetButtonUp("Fire2")) {
				ProjectUtils.ShowCursor();
			}
		}
		private void DoInteraction() {
			Ray pointRay = cam.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(pointRay, out hit, raydist, rayMask)) { return; }
			Transform directTarget = hit.collider.transform;
			iLink = directTarget.GetComponent<InteractableLink>();
			interactable = directTarget.GetComponent<InteractableBase>();
			if (interactable) {
				targetrb = interactable.rb;
			} else if (iLink) {
				interactable = iLink.interactable;
				targetrb = interactable.rb;
			} else {
				Transform target = hit.transform;
				interactable = target.GetComponent<InteractableBase>();
				if (interactable) {
					targetrb = interactable.rb;
				}
			}
			if (!interactable) { return; }
			switch (interactable.interactableType) {
				case InteractableType.Pickable:
					interactable.InteractStart();
					holdpoint.transform.position = targetrb.position;
					holdpoint.connectedBody = targetrb;
					break;
				default:
					break;
			}
		}
		private void StopInteraction() {
			if (!interactable) { return; }
			switch (interactable.interactableType) {
				case InteractableType.Pickable:
					if (holdpoint && holdpoint.connectedBody) {
						holdpoint.connectedBody = null;
					}
					interactable.InteractStop();
					break;
				default:
					break;
			}
		}
	}
}
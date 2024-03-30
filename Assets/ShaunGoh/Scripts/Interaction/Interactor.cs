using UnityEngine;

namespace ShaunGoh {
	public class Interactor : MonoBehaviour {
		private Camera cam;
		public LayerMask rayMask;
		public int raydist;
		private RaycastHit hit;

		public FixedJoint holdpoint;
		public Transform grabPoint, hitmark;
		private I_Interactable interactable;
		private InteractableLink iLink;
		private bool inInteraction;
		public bool occupied { get { return inInteraction; } }


		private void Awake() {
			cam = Camera.main;
		}
		private void Update() {
			if (ProjectUtils.inMenu) { return; }

			if (Input.GetButtonDown("Fire1")) {
				if (inInteraction) {
					StopInteraction();
				} else {
					DoInteraction();
				}
			}
			if (inInteraction) {
				if (Input.GetButtonDown("Switch")) {
					SwitchInteracted();
				}
				switch (ProjectUtils.playState) {
					case PlayerState.Character:
						if (Input.GetButtonDown("Fire2")) {
							ProjectUtils.SetPlayState(PlayerState.RotateObject);
						}
						break;
					case PlayerState.RotateObject:
						if (Input.GetButtonUp("Fire2")) {
							ProjectUtils.RevertPlayState();
						}
						break;
				}
			}
			if (null != interactable) {
				interactable.ConstantInteraction(this);
			}
		}
		public RaycastHit? CastRay(){
			Ray pointRay = cam.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(pointRay, out hit, raydist, rayMask)) { return null; }
			return hit;
		}
		private void DoInteraction() {
			if (null == CastRay()) { return; }
			holdpoint.transform.localRotation = Quaternion.identity;
			Transform directTarget = hit.collider.transform;
			iLink = directTarget.GetComponent<InteractableLink>();
			interactable = directTarget.GetComponent<I_Interactable>();
			if (iLink) {
				interactable = iLink.interactable;
			} else {
				if (null == interactable) {
					interactable = directTarget.GetComponentInParent<I_Interactable>();
				}
			}
			if (null == interactable) { return; }
			interactable.StartInteraction(this);
			switch (interactable.Itype) {
				case InteractableType.Pickable:
					inInteraction = true;
					break;
				default:
					break;
			}
		}
		private void StopInteraction() {
			if (null == interactable) { return; }
			interactable.StopInteraction(this);
			interactable = null;
			inInteraction = false;
		}
		private void SwitchInteracted() {
			if (null == interactable) { return; }
			interactable.SwitchInteraction(this);
		}
	}
}
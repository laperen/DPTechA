using UnityEngine;

namespace ShaunGoh {
	public class Interactor : MonoBehaviour {
		private Camera cam;
		public LayerMask rayMask;
		public int raydist;
		public GameObject downIndicatorObj;
		[HideInInspector]
		public DownIndicator downIndicator;
		private RaycastHit hit;

		public FixedJoint holdpoint;
		private I_Interactable interactable;
		private InteractableLink iLink;
		private bool inInteraction;
		public bool occupied { get { return inInteraction; } }


		private void Awake() {
			cam = Camera.main;
			downIndicator = downIndicatorObj.GetComponent<DownIndicator>();
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
				if (Input.GetButtonDown("Freeze")) {
					FreezeInteracted();
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
		private void DoInteraction() {
			Ray pointRay = cam.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(pointRay, out hit, raydist, rayMask)) { return; }
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
		private void CommonStopInteraction() {
			interactable = null;
			inInteraction = false;
			switch (ProjectUtils.playState) {
				case PlayerState.RotateObject:
					ProjectUtils.RevertPlayState();
					break;
			}
		}
		private void StopInteraction() {
			if (null == interactable) { return; }
			interactable.StopInteraction(this);
			CommonStopInteraction();
		}
		private void FreezeInteracted() {
			if (null == interactable) { return; }
			interactable.FreezeInteraction(this);
			CommonStopInteraction();
		}
	}
}
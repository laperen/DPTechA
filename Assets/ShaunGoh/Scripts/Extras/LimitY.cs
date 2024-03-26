using UnityEngine;

namespace ShaunGoh {
	[RequireComponent(typeof(I_Interactable))]
	[RequireComponent(typeof(ObjectResetter))]
	public class LimitY : MonoBehaviour {
		public float limit;
		private ObjectResetter resetter;
		private I_Interactable interactable;
		private bool interacted;
		private void Awake() {
			resetter = this.GetComponent<ObjectResetter>();
			interactable = this.GetComponent<I_Interactable>();
			interactable.OnStartInteract += InteractStart;
			interactable.OnStopInteract += InteractStop;
		}
		private void OnDestroy() {
			interactable.OnStartInteract -= InteractStart;
			interactable.OnStopInteract -= InteractStop;
		}
		private void InteractStart() { 
			interacted = true;
		}
		private void InteractStop() {
			interacted = false;
		}
		private void Update() {
			if(!interacted && transform.position.y < limit) { 
				resetter.ResetTransform();
			}
		}
	}
}
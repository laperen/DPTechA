using System;
using UnityEngine;
using UnityEngine.Events;

namespace ShaunGoh {
	public class InteractableButton : MonoBehaviour, I_Interactable {
		public InteractableType Itype { get { return InteractableType.Button; } }
		public string snapTag;
		public string SnapTag { get { return snapTag; } }
		public UnityEvent OnInteractStart;
		public event Action OnStartInteract, OnStopInteract;

		public void StartInteraction(Interactor interactor) {
			if (!interactor) { return; }
			OnStartInteract?.Invoke();
			OnInteractStart.Invoke();
		}
		public void StopInteraction(Interactor interactor) {
			if (!interactor) { return; }
			OnStopInteract?.Invoke();
		}
		public void FreezeInteraction(Interactor interactor) {
			if (!interactor) { return; }
			OnStopInteract?.Invoke();
		}
		public void ConstantInteraction(Interactor interactor) {
		}
	}
}
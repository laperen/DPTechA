using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShaunGoh {
	public class InteractableBase : MonoBehaviour {
		public Rigidbody rb;
		[HideInInspector]
		public InteractableType interactableType;
		public UnityEvent OnInteractStart, OnInteractStop;
		public virtual void InteractStart() {
			OnInteractStart.Invoke();
		}
		public virtual void InteractStop() {
			OnInteractStop.Invoke();
		}
	}
}

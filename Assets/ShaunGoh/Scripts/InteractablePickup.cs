using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaunGoh {
	public class InteractablePickup : InteractableBase {
		private void Awake() {
			interactableType = InteractableType.Pickable;
		}
	}
}
using UnityEngine;

namespace ShaunGoh {
	public class ObjectFollower : MonoBehaviour {
		public Transform target;
		public bool position = true, rotation = true;
		private void Update() {
			if(!target){ return; }
			if (position) {
				transform.position = target.position;
			}
			if (rotation) {
				transform.rotation = target.rotation;
			}
		}
	}
}
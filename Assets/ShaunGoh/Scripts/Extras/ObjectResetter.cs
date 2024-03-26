using UnityEngine;

namespace ShaunGoh {
	public class ObjectResetter : MonoBehaviour {
		private Vector3 sourcePos;
		private Quaternion sourceRot;
		private void Awake() {
			sourcePos = transform.position;
			sourceRot = transform.rotation;
		}
		public void ResetTransform() { 
			transform.position = sourcePos;
			transform.rotation = sourceRot;
		}
	}
}
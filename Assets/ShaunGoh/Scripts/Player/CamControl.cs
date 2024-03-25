using UnityEngine;

namespace ShaunGoh {
	public class CamControl : MonoBehaviour {
		public Transform trans;
		public Transform vert;
		public float turnspeed = 2;

		private void Update() {
			if (ProjectUtils.inMenu) { return; }
			Rotation();
		}
		private void Rotation() {
			float lookX = Input.GetAxis("Mouse X");
			float lookY = Input.GetAxis("Mouse Y");
			trans.Rotate(Vector3.up, lookX * turnspeed);
			vert.Rotate(Vector3.right, -lookY * turnspeed);
		}
	}
}

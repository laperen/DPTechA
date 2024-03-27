using UnityEngine;

namespace ShaunGoh {
	public class CamControl : MonoBehaviour {
		public Transform trans;
		public Transform vert;
		private void Update() {
			if (ProjectUtils.inMenu) { return; }
			Rotation();
		}
		private void Rotation() {
			float lookX = Input.GetAxis("Mouse X");
			float lookY = Input.GetAxis("Mouse Y");
			float timepassed = Time.deltaTime;
			trans.Rotate(Vector3.up, lookX * ProjectUtils.camTurnspeed * timepassed);
			vert.Rotate(Vector3.right, -lookY * ProjectUtils.camTurnspeed * timepassed);
		}
	}
}

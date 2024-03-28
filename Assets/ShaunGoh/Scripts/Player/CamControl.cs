using UnityEngine;

namespace ShaunGoh {
	public class CamControl : MonoBehaviour {
		public Transform trans;
		public Transform vert;
		private float vertangle;
		private void Update() {
			if (ProjectUtils.inMenu) { return; }
			Rotation();
		}
		private void Rotation() {
			float lookX = Input.GetAxis("Mouse X");
			float lookY = Input.GetAxis("Mouse Y");
			float timepassed = Time.deltaTime;
			if (lookX != 0) {
				trans.Rotate(Vector3.up, lookX * ProjectUtils.camTurnspeed * timepassed);
			}
			if (lookY != 0) {
				vertangle -= lookY * ProjectUtils.camTurnspeed * timepassed;
				vertangle = Mathf.Clamp(vertangle, -90, 90);
				vert.localRotation = Quaternion.Euler(vertangle, 0, 0);
			}
		}
	}
}

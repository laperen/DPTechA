using UnityEngine;

namespace ShaunGoh {
	public class StartProperties : MonoBehaviour {
		public static StartProperties instance;
		public float rotateSpeed = 0.2f, pickupZoom = 1;

		private void Awake() {
			if(null != instance ) {
				Destroy(this.gameObject);
				return;
			}
			ProjectUtils.inMenu = false;
			ProjectUtils.SetPlayState(PlayerState.Character);
			ProjectUtils.pickupRotateSpeed = rotateSpeed;
			ProjectUtils.pickupZoomScale = pickupZoom;
			instance = this;
		}
		private void Start() {
			ProjectUtils.HideCursor();
		}
	}
}
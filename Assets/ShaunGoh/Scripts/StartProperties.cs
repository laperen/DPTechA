using UnityEngine;

namespace ShaunGoh {
	public class StartProperties : MonoBehaviour {
		public static StartProperties instance;
		public float pickupRotateSpeed = 180f, pickupZoom = 1;

		public float playerMovespeed = 40f;
		public float playerRunspeed = 120f;
		public float camTurnspeed = 1080f;

		private void Awake() {
			if(null != instance ) {
				Destroy(this.gameObject);
				return;
			}
			ProjectUtils.inMenu = false;
			ProjectUtils.SetPlayState(PlayerState.Character);
			ProjectUtils.pickupRotateSpeed = pickupRotateSpeed;
			ProjectUtils.pickupZoomScale = pickupZoom;
			ProjectUtils.playerMovespeed = playerMovespeed;
			ProjectUtils.playerRunspeed = playerRunspeed;
			ProjectUtils.camTurnspeed = camTurnspeed;
			instance = this;
		}
		private void Start() {
			ProjectUtils.HideCursor();
		}
		private void Update() {
			if (Input.GetButtonDown("Cancel")) {
				ProjectUtils.inMenu = !ProjectUtils.inMenu;
				if (ProjectUtils.inMenu) {
					ProjectUtils.ShowCursor();
				} else {
					ProjectUtils.HideCursor();
				}
			}
		}
	}
}
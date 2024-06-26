using UnityEngine;

namespace ShaunGoh {
	public class StartProperties : MonoBehaviour {
		public static StartProperties instance;
		public float pickupRotateSpeed = 180f, pickupZoom = 1;

		public float playerMovespeed = 40f;
		public float playerRunspeed = 120f;
		public float camTurnspeed = 1080f;

		public GameObject GameplayCanvases, MenuCanvases;
		private UISelector GameplayMenuSelector;

		private void Awake() {
			if(null != instance ) {
				Destroy(this.gameObject);
				return;
			}
			GameplayMenuSelector = GameplayCanvases.GetComponent<UISelector>();
			ProjectUtils.playStateChanged += OnPlayStateChange;
			ProjectUtils.inMenu = false;
			ProjectUtils.SetPlayState(PlayerState.Character);
			ProjectUtils.pickupRotateSpeed = pickupRotateSpeed;
			ProjectUtils.pickupZoomScale = pickupZoom;
			ProjectUtils.playerMovespeed = playerMovespeed;
			ProjectUtils.playerRunspeed = playerRunspeed;
			ProjectUtils.camTurnspeed = PlayerPrefs.HasKey(ProjectUtils.mouseSensitivityPrefKey) ? PlayerPrefs.GetFloat(ProjectUtils.mouseSensitivityPrefKey) : camTurnspeed;
			instance = this;
		}
		private void OnDestroy() {
			ProjectUtils.playStateChanged -= OnPlayStateChange;
		}
		private void Start() {
			ProjectUtils.HideCursor();
		}
		private void Update() {
			if (Input.GetButtonDown("Cancel")) {
				ProjectUtils.inMenu = !ProjectUtils.inMenu;
				if (ProjectUtils.inMenu) {
					ProjectUtils.ShowCursor();
					GameplayCanvases.SetActive(false);
					MenuCanvases.SetActive(true);
				} else {
					ProjectUtils.HideCursor();
					GameplayCanvases.SetActive(true);
					MenuCanvases.SetActive(false);
				}
			}
		}
		private void OnPlayStateChange() {
			switch (ProjectUtils.playState) {
				case PlayerState.Character:
					switch(ProjectUtils.grabState) {
						case GrabState.Placement:
						case GrabState.Freehold:
							GameplayMenuSelector.ShowItem(2);
							break;
						default:
							GameplayMenuSelector.ShowItem(0);
							break;
					}
					break;
				case PlayerState.Focused:
					switch(ProjectUtils.grabState){
						case GrabState.Placement:
						case GrabState.Freehold:
							GameplayMenuSelector.ShowItem(3);
							break;
						default:
							GameplayMenuSelector.ShowItem(1);
							break;
					}
					break;
				case PlayerState.RotateObject:
					GameplayMenuSelector.ShowItem(3);
					break;
				default:
					break;
			}
		}
	}
}
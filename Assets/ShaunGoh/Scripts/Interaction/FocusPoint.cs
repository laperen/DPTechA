using UnityEngine;

namespace ShaunGoh {
	public class FocusPoint : MonoBehaviour {
		public Transform playerPoint;
		private ObjectFollower player;
		private PlayerMovement movement;
		private void Awake() {
			PlayerMark[] marks = GameObject.FindObjectsOfType<PlayerMark>();
			for(int i = 0, max = marks.Length; i < max; i++) {
				if (marks[i].gameObject.activeInHierarchy) {
					player = marks[i].GetComponent<ObjectFollower>();
					break;
				}
			}
			movement = GameObject.FindObjectOfType<PlayerMovement>();
		}
		public void ToggleFocus() {
			if (null == player) { return; }
			switch (ProjectUtils.playState) {
				case PlayerState.Focused:
					player.TransitionTo(movement.playerPoint, ProjectUtils.RevertPlayState);
					break;
				default:
					ProjectUtils.SetPlayState(PlayerState.Focused);
					player.TransitionTo(playerPoint);
					break;
			}
		}
	}
}
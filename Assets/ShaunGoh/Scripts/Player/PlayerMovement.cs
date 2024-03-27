using UnityEngine;

namespace ShaunGoh {
	public class PlayerMovement : MonoBehaviour {
		public Rigidbody rb;
		public Transform playerPoint;
		private Transform camcontrol;

		private void Awake() {
			camcontrol = ProjectUtils.GetPlayerMark().transform;
		}
		private void FixedUpdate() {
			if (ProjectUtils.inMenu) { return; }
			Movement();
		}
		private void Movement() {
			bool haltstate = true;
			switch (ProjectUtils.playState) {
				case PlayerState.Character:
					haltstate = false;
					break;
				default:
					break;
			}
			float hori = haltstate ? 0 : Input.GetAxisRaw("Horizontal");
			float vert = haltstate ? 0 : Input.GetAxisRaw("Vertical");
			Vector3 movedir = new Vector3(hori, 0, vert);
			movedir.Normalize();
			movedir *= Time.fixedDeltaTime;
			movedir.y = rb.velocity.y;
			rb.velocity = camcontrol.TransformDirection(movedir * (Input.GetButton("Fire3") ? ProjectUtils.playerRunspeed : ProjectUtils.playerMovespeed));
		}
	}
}
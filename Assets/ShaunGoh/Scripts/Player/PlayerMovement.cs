using UnityEngine;

namespace ShaunGoh {
	public class PlayerMovement : MonoBehaviour {
		public Rigidbody rb;
		public Transform playerPoint;
		public float movespeed = 1;
		public float runspeed = 5;
		private Transform camcontrol;

		private void Awake() {
			camcontrol = ProjectUtils.GetPlayerMark().transform;
		}
		private void FixedUpdate() {
			if (ProjectUtils.inMenu) { return; }
			Movement();
		}
		private void Movement() {
			if(ProjectUtils.playState != PlayerState.Character) { return; }
			bool haltstate = ProjectUtils.playState != PlayerState.Character;
			float hori = haltstate ? 0 : Input.GetAxisRaw("Horizontal");
			float vert = haltstate ? 0 : Input.GetAxisRaw("Vertical");
			Vector3 movedir = new Vector3(hori, 0, vert);
			movedir.Normalize();
			movedir.y = rb.velocity.y;
			rb.velocity = camcontrol.TransformDirection(movedir * (Input.GetButton("Fire3") ? runspeed : movespeed));
		}
	}
}
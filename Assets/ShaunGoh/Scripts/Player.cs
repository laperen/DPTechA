using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaunGoh {
	public class Player : MonoBehaviour {
		public Transform trans;
		public Transform vert;
		public Rigidbody rb;
		public float turnspeed = 2;
		public float movespeed = 1;
		public float runspeed = 5;

		private void Update() {
			if (ProjectUtils.inMenu) { return; }
			Rotation();
		}
		private void FixedUpdate() {
			if (ProjectUtils.inMenu) { return; }
			Movement();
		}
		private void Rotation() {
			if (Input.GetButton("Fire2")) { return; }
			float lookX = Input.GetAxis("Mouse X");
			float lookY = Input.GetAxis("Mouse Y");
			trans.Rotate(Vector3.up, lookX * turnspeed);
			vert.Rotate(Vector3.right, -lookY * turnspeed);
		}
		private void Movement() {
			float hori = Input.GetAxisRaw("Horizontal");
			float vert = Input.GetAxisRaw("Vertical");
			Vector3 movedir = new Vector3(hori, 0, vert);
			movedir.Normalize();
			movedir.y = rb.velocity.y;
			rb.velocity = trans.TransformDirection(movedir * (Input.GetButton("Fire3") ? runspeed : movespeed));
		}
	}
}
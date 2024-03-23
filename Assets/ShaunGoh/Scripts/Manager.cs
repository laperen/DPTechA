using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaunGoh {
	public class Manager : MonoBehaviour {
		public static Manager instance;

		private void Awake() {
			if(null != instance ) {
				Destroy(this.gameObject);
				return;
			}
			ProjectUtils.inMenu = false;
			ProjectUtils.SetPlayState(PlayerState.Character);
			ProjectUtils.pickupRotateSpeed = 0.2f;
			ProjectUtils.pickupZoomScale = 1;
			instance = this;
		}
		private void Start() {
			ProjectUtils.HideCursor();
		}
	}
}
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
			instance = this;
		}
		private void Start() {
			ProjectUtils.HideCursor();
		}
	}
}
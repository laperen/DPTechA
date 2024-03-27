using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShaunGoh {
	public class SettingsScript : MonoBehaviour {
		public Slider mouseSensitivity;

		private void Start() {
			mouseSensitivity.value = ProjectUtils.camTurnspeed;
		}
		public void OnMouseSensitivityChange(float value) { 
			ProjectUtils.camTurnspeed = value;
		}
	}
}
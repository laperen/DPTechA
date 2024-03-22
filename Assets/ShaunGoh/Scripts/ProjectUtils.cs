using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaunGoh {
	public enum InteractableType { None, Pickable, Pushable, Immobile, Tool, Panel }

	public class ProjectUtils {
		public static bool inMenu;
		public static void HideCursor() {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		public static void ShowCursor() {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}

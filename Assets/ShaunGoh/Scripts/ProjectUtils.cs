using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShaunGoh {
	public enum InteractableType { None, Pickable, Pushable, Immobile, Tool, Panel }
	public enum PlayerState { Character, RotateObject, Focused }

	public class ProjectUtils {
		public static bool inMenu;
		private static PlayerState prevPlayState;
		public static float pickupRotateSpeed = 0.2f;
		public static PlayerState playState { get; private set; }

		public static void HideCursor() {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		public static void ShowCursor() {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		public static void SetPlayState(PlayerState newstate) {
			prevPlayState = playState;
			playState = newstate;
		}
		public static void RevertPlayState() { 
			playState = prevPlayState;
		}
	}
	public interface I_Interactable {
		InteractableType Itype { get; set; }
		void StartInteraction(Interactor interactor);
		void StopInteraction(Interactor interactor);
		void ConstantInteraction(Interactor interactor);
		void FreezeInteraction(Interactor interactor);
	}
}

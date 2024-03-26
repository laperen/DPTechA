using System;
using UnityEngine;
using UnityEngine.Events;

namespace ShaunGoh {
	public enum InteractableType { None, Pickable, Pushable, Immobile, Tool, Panel, Button }
	public enum PlayerState { Character, RotateObject, Focused }
	[Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	[Serializable]
	public class IntEvent: UnityEvent<int> { }
	[Serializable]
	public class StringEvent : UnityEvent<string> { }
	public class ProjectUtils {
		public static bool inMenu;
		private static PlayerState prevPlayState;
		public static float pickupRotateSpeed, pickupZoomScale;
		public static PlayerState playState { get; private set; }
		private static PlayerMark player;

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
		public static PlayerMark GetPlayerMark(bool retest = false) {
			if (!retest && player) { return player; }
			PlayerMark[] marks = GameObject.FindObjectsOfType<PlayerMark>();
			for (int i = 0, max = marks.Length; i < max; i++) {
				if (marks[i].gameObject.activeInHierarchy) {
					player = marks[i];
					return player;
				}
			}
			return null;
		}
	}
	public interface I_Interactable {
		InteractableType Itype { get; }
		string SnapTag { get; }
		event Action OnStartInteract;
		event Action OnStopInteract;
		void StartInteraction(Interactor interactor);//on mouse click
		void StopInteraction(Interactor interactor);//on mouse click while this object is in a running interaction
		void ConstantInteraction(Interactor interactor);//on update, controls are placed within interactable
		void FreezeInteraction(Interactor interactor);//on key F
	}
}

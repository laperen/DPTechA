using System;
using System.Collections.Generic;
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
	[Serializable]
	public class ColorEvent : UnityEvent<Color> { }
	public class ProjectUtils {
		public static bool inMenu;
		private static List<PlayerState> prevPlayStates;
		public static float pickupRotateSpeed, pickupZoomScale;
		public static float playerMovespeed;
		public static float playerRunspeed;
		public static float camTurnspeed;
		public static event Action playStateChanged;
		public static PlayerState playState { get; private set; }
		public static bool grabState { get; private set; }
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
			if (null == prevPlayStates) { prevPlayStates = new(); }
			prevPlayStates.Add(newstate);
			playState = newstate;
			playStateChanged?.Invoke();
		}
		public static void RevertPlayState() { 
			int index = prevPlayStates.Count - 1;
			prevPlayStates.RemoveAt(index);
			playState = prevPlayStates[index - 1];
			playStateChanged?.Invoke();
		}
		public static void SetGrabState(bool state) {
			grabState = state;
			playStateChanged?.Invoke();
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

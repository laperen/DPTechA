using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace ShaunGoh {
    public class UISelector : MonoBehaviour {
		private List<GameObject> items;
		private void Awake() {
			items = new();
			for (int i = 0, max = transform.childCount; i < max; i++) { 
				items.Add(transform.GetChild(i).gameObject);
			}
		}
		public void ShowItem(GameObject item) {
			for(int i = 0, max = items.Count; i < max; i++) {
				items[i].SetActive(items[i] == item);
			}
		}
		public void ShowItem(int index) {
			for (int i = 0, max = items.Count; i < max; i++) {
				items[i].SetActive(i == index);
			}
		}
	}
}
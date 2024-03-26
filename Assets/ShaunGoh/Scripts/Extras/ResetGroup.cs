using UnityEngine;
using UnityEngine.Events;

namespace ShaunGoh {
	public class ResetGroup : MonoBehaviour {
		private ObjectResetter[] resetters;
		private SnapPoint[] snapPoints;
		public UnityEvent Resetted;
		private void Awake() {
			resetters = this.GetComponentsInChildren<ObjectResetter>();
			snapPoints = this.GetComponentsInChildren<SnapPoint>();
		}
		public void PerformReset() {
			for (int i = 0, max = snapPoints.Length; i < max; i++) {
				snapPoints[i].ResetSnap();
			}
			for (int i = 0, max = resetters.Length; i < max ; i++) {
				resetters[i].ResetTransform();
			}
			Resetted.Invoke();
		}
	}
}
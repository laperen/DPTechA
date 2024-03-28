using UnityEngine;

namespace ShaunGoh {
	public class SnapPointScoreChecker : MonoBehaviour {
		private SnapPoint[] points;
		private int maxScore, currScore;
		public IntEvent ReflectMaxScore, ReflectCurrentScore;
		private void Awake() {
			points = this.GetComponentsInChildren<SnapPoint>();
			maxScore = points.Length;
			ReflectMaxScore.Invoke(maxScore);
			for(int i = 0; i < maxScore; i++) {
				SnapPoint point = points[i];
				point.Snapped += AddPoint;
				point.Unsnapped += RemovePoint;
			}
		}
		private void OnDestroy() {
			for (int i = 0; i < maxScore; i++) {
				SnapPoint point = points[i];
				point.Snapped -= AddPoint;
				point.Unsnapped -= RemovePoint;
			}
		}
		private void AddPoint() {
			currScore++;
			ReflectCurrentScore.Invoke(currScore);
		}
		private void RemovePoint() {
			currScore--;
			ReflectCurrentScore.Invoke(currScore);
		}
	}
}
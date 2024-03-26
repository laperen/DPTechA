using System;
using UnityEngine;

namespace ShaunGoh {
	public class ScoreTimer : MonoBehaviour {
		public StringEvent ReflectTime;
		public BoolEvent ResetState;
		private float timePassed;
		private int maxScore, currentScore;
		private bool started;
		private bool complete;
		public void SetMax(int max) { 
			maxScore = max;
		}
		public void SetCurrent(int current) {
			if (complete) { return; }
			currentScore = current;
			if(currentScore >= maxScore) {
				complete = true;
			}
		}
		private void Update() {
			if (complete) { return; }
			if (!started && currentScore >= 1) {
				started = true;
				ResetState.Invoke(true);
			}
			if (started && currentScore < maxScore) {
				timePassed += Time.deltaTime;
				ReflectTime.Invoke(SecondsToTime(timePassed));
			}
		}
		private string SecondsToTime(float seconds) {
			TimeSpan t = TimeSpan.FromSeconds(seconds);
			return string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
		}
		public void ResetTimer() {
			ResetState.Invoke(false);
			started = false;
			complete = false;
			timePassed = 0;
			currentScore = 0;
			ReflectTime.Invoke(SecondsToTime(timePassed));
		}
	}
}
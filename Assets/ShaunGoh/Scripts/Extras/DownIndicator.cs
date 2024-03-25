using UnityEngine;

namespace ShaunGoh {
	public class DownIndicator : MonoBehaviour {
		public LineRenderer linerend;
		public LayerMask rayMask;
		public int raydist;
		private RaycastHit hit;

		public void DrawFromPos(Vector3 pos) {
			if(!linerend){ return; }
			if (Physics.Raycast(pos, Vector3.down, out hit, raydist, rayMask)) {
				linerend.SetPosition(0, pos);
				linerend.SetPosition(1, hit.point);
			}
		}
	}
}
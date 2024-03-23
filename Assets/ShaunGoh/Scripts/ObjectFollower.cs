using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaunGoh {
	public class ObjectFollower : MonoBehaviour {
		public Transform target;
		private void Update() {
			if(!target){ return; }
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imperceptible {
	public class FollowTargetLoosly : MonoBehaviour {
		[SerializeField] private GameObject target;
		[SerializeField] private Vector3    offset;
		[SerializeField] private Vector3    offsetRotation;

		private Vector3 _positionVel = Vector3.zero;
		
		// Update is called once per frame
		void LateUpdate() {
			var ree = target.transform.rotation*offset;
			transform.position =
				Vector3.SmoothDamp(transform.position, target.transform.position+new Vector3(ree.x,ree.y,ree.z), ref _positionVel, 0.1f);
			transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation*Quaternion.Euler(offsetRotation), 10f * Time.deltaTime);
		}
	}
}
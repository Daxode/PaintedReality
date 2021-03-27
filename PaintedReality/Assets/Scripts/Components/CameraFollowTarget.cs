using System;
using Unity.Entities;

public enum CameraTargetFollowType {
	Player,
	Static
}

namespace com.daxode.Components {
	[GenerateAuthoringComponent]
	public struct CameraFollowTarget : IComponentData, IComparable<CameraFollowTarget> {
		
		public int Priority;
		public CameraTargetFollowType Type;
		
		private bool Equals(CameraFollowTarget other) {
			return Priority == other.Priority;
		}
		
		public int CompareTo(CameraFollowTarget other) {
			return Priority.CompareTo(other.Priority);
		}
		
		public static bool operator ==(CameraFollowTarget currentTarget, CameraFollowTarget otherTarget) {
			return (currentTarget.Priority == otherTarget.Priority);
		}

		public static bool operator !=(CameraFollowTarget currentTarget, CameraFollowTarget otherTarget) {
			return (currentTarget.Priority != otherTarget.Priority);
		}

		public static bool operator >(CameraFollowTarget currentTarget, CameraFollowTarget otherTarget) {
			return (currentTarget.Priority < otherTarget.Priority);
		}

		public static bool operator <(CameraFollowTarget currentTarget, CameraFollowTarget otherTarget) {
			return (currentTarget.Priority > otherTarget.Priority);
		}
		
		
		public static bool operator >=(CameraFollowTarget currentTarget, CameraFollowTarget otherTarget) {
			return (currentTarget.Priority >= otherTarget.Priority);
		}

		public static bool operator <=(CameraFollowTarget currentTarget, CameraFollowTarget otherTarget) {
			return (currentTarget.Priority <= otherTarget.Priority);
		}
	}
}

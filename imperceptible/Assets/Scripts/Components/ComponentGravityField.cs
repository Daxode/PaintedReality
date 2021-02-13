using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Imperceptible.Components {
	public enum GravityFieldType {
		Parallel,
		Sphere
	}
	
	[RequireComponentTag(typeof(PhysicsCollider))]
	public struct ComponentGravityField : IComponentData {
		public GravityFieldType GravityFieldType;
		public int              Priority;
		public Vector3          GravityAcceleration;
	}
}

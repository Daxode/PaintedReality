using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace com.daxode.Components {
	public enum GravityFieldType {
		Parallel,
		Sphere
	}
	
	[RequireComponentTag(typeof(PhysicsCollider))]
	public struct GravityField : IComponentData {
		public GravityFieldType GravityFieldType;
		public int              Priority;
		public float3          GravityAcceleration;
	}
}

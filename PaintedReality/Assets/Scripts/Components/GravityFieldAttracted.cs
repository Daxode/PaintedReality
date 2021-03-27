using com.daxode.Components.Authoring;
using Unity.Entities;

namespace com.daxode.Components {
	[GenerateAuthoringComponent]
	[RequireComponentTag(typeof(AuthorComponentEntityBuffer))]
	public struct GravityFieldAttracted : IComponentData {
		public Entity GravityFieldCurrentEntity;
		public bool   IsOnGround;
		public float  AngularVelocityAffectanceRate;
	}
}

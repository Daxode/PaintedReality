using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Imperceptible.Components {
	[GenerateAuthoringComponent]
	[RequireComponentTag(typeof(Authoring.AuthorComponentEntityBuffer))]
	public struct GravityFieldAttracted : IComponentData {
		public Entity GravityFieldCurrentEntity;
		public bool   IsOnGround;
	}
}

using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Imperceptible.Components {
	[GenerateAuthoringComponent]
	[RequireComponentTag(typeof(Authoring.AuthorComponentEntityBuffer))]
	public struct ComponentGravityFieldAttracted : IComponentData {
		public Entity                GravityFieldCurrentEntity;
	}
}

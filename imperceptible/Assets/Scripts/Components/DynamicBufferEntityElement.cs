using Unity.Entities;
using UnityEngine;

namespace Imperceptible.Components {
	[InternalBufferCapacity(8)]
	public struct DynamicBufferEntityElement : IBufferElementData {
		// Actual value each buffer element will store.
		public Entity Value;

		// The following implicit conversions are optional, but can be convenient.
		public static implicit operator Entity(DynamicBufferEntityElement e) {
			return e.Value;
		}

		public static implicit operator DynamicBufferEntityElement(Entity e) {
			return new DynamicBufferEntityElement {Value = e};
		}
	}
}
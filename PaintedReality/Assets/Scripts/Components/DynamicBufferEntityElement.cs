using Unity.Entities;

namespace com.daxode.Components {
	[InternalBufferCapacity(8)]
	public struct DynamicBufferEntityElement : IBufferElementData {
		// Actual value each buffer element will store.
		private Entity _value;

		// The following implicit conversions are optional, but can be convenient.
		public static implicit operator Entity(DynamicBufferEntityElement e) {
			return e._value;
		}

		public static implicit operator DynamicBufferEntityElement(Entity e) {
			return new DynamicBufferEntityElement {_value = e};
		}
	}
}
using Unity.Entities;
using UnityEngine;

namespace Imperceptible.Components.Authoring {
	public class AuthorComponentEntityBuffer : MonoBehaviour, IConvertGameObjectToEntity {
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
			dstManager.AddBuffer<DynamicBufferEntityElement>(entity);
		}
	}
}
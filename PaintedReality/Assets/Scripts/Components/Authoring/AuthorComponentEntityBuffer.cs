using Unity.Entities;
using UnityEngine;

namespace com.daxode.Components.Authoring {
	public class AuthorComponentEntityBuffer : MonoBehaviour, IConvertGameObjectToEntity {
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
			dstManager.AddBuffer<DynamicBufferEntityElement>(entity);
		}
	}
}
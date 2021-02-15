using Unity.Entities;
using UnityEngine;

namespace Imperceptible.Components.Authoring {
	public class AuthorComponentPlayer : MonoBehaviour, IConvertGameObjectToEntity {
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
			dstManager.AddComponentData(entity, new Tags.TagPlayer());
		}
	}
}
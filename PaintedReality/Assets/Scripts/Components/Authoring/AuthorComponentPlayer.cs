using com.daxode.Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace com.daxode.Components.Authoring {
	public class AuthorComponentPlayer : MonoBehaviour, IConvertGameObjectToEntity {
		[SerializeField] private float angularVelocityAffectanceRate = 10;
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
			dstManager.AddComponentData(entity, new GravityFieldAttracted() {
				AngularVelocityAffectanceRate = angularVelocityAffectanceRate
			});
			dstManager.AddBuffer<DynamicBufferEntityElement>(entity);
			dstManager.AddComponentData(entity, new TagPlayer());
			dstManager.AddComponentData(entity, new LivingJumper());
		}
	}
}
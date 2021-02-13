using Imperceptible.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Imperceptible.Systems.GravityFields {
	public class GFPull : SystemBase {
		protected override void OnUpdate() {
			var timeDelta = Time.DeltaTime;
			Entities.ForEach((ref PhysicsVelocity physicsVelocity, 
			                  in PhysicsMass physicsMass, 
			                  in ComponentGravityFieldAttracted componentGravityFieldTarget, 
			                  in LocalToWorld ltw) => {
				var allGravityFields = GetComponentDataFromEntity<ComponentGravityField>(true);
				if (!allGravityFields.HasComponent(componentGravityFieldTarget.GravityFieldCurrentEntity)) return;
				var allLocalToWorld = GetComponentDataFromEntity<LocalToWorld>(true);

				var gravityField    = allGravityFields[componentGravityFieldTarget.GravityFieldCurrentEntity];
				var gravityFieldLTW = allLocalToWorld[componentGravityFieldTarget.GravityFieldCurrentEntity];
				
				switch (gravityField.GravityFieldType) {
					case GravityFieldType.Parallel:
						physicsVelocity.Linear += gravityFieldLTW.Up*gravityField.GravityAcceleration*timeDelta/physicsMass.InverseMass;
						break;
					case GravityFieldType.Sphere:
						float3 direction = ltw.Position - gravityFieldLTW.Position;
						direction *= gravityField.GravityAcceleration;
						physicsVelocity.Linear += direction*timeDelta/physicsMass.InverseMass;
						break;
				}
			}).Schedule();
		}
	}
}
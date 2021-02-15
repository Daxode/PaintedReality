using Imperceptible.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

namespace Imperceptible.Systems.GravityFields {
	public class GFPull : SystemBase {
		protected override void OnUpdate() {
			var timeDelta = Time.DeltaTime;
			Entities.ForEach((ref PhysicsVelocity       physicsVelocity,
			                  in  PhysicsMass           physicsMass,
			                  in  GravityFieldAttracted componentGravityFieldTarget,
			                  in  LocalToWorld          ltw) => {
				var allGravityFields = GetComponentDataFromEntity<GravityField>(true);
				if (!allGravityFields.HasComponent(componentGravityFieldTarget.GravityFieldCurrentEntity)) return;
				var allLocalToWorld = GetComponentDataFromEntity<LocalToWorld>(true);

				var gravityField    = allGravityFields[componentGravityFieldTarget.GravityFieldCurrentEntity];
				var gravityFieldLTW = allLocalToWorld[componentGravityFieldTarget.GravityFieldCurrentEntity];

				float3 gravity   = gravityField.GravityAcceleration;
				float3 direction = new float3(0, 1, 0);

				switch (gravityField.GravityFieldType) {
					case GravityFieldType.Parallel:
						//gravity = math.mul(gravityFieldLTW.Value, new float4(gravity, 1)).xyz;
						break;
					case GravityFieldType.Sphere:
						direction = math.normalize(ltw.Position - gravityFieldLTW.Position);
						break;
				}

				var sideVec          = math.normalize(math.cross(direction, ltw.Forward));
				var LocalToDirection = new float3x3(sideVec, direction, math.normalize(math.cross(direction, sideVec)));
				gravity = math.mul(LocalToDirection, gravity);
				physicsVelocity.Linear += gravity * timeDelta / physicsMass.InverseMass;


				var angularMoveDir                 = math.cross(gravity, ltw.Up);
				var inertiaOrientationInWorldSpace = math.mul(ltw.Rotation.value, physicsMass.InertiaOrientation);
				var angularVelocityInertiaSpace =
					math.rotate(math.inverse(inertiaOrientationInWorldSpace), angularMoveDir);
				physicsVelocity.Angular += angularVelocityInertiaSpace * timeDelta * physicsMass.InverseInertia * 10;
			}).Schedule();
		}
	}
}
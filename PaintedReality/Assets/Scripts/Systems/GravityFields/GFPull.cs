using com.daxode.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace com.daxode.Systems.GravityFields {
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
				var gravityFieldLtw = allLocalToWorld[componentGravityFieldTarget.GravityFieldCurrentEntity];

				var gravity   = gravityField.GravityAcceleration;
				var direction = new float3( 0, 1, 0);

				switch (gravityField.GravityFieldType) {
					case GravityFieldType.Parallel:
						//gravity = math.mul(new float4(gravity, 1),gravityFieldLtw.Value).xyz;
						//direction = math.normalize(math.mul(gravityFieldLtw.Value, new float4(direction, 1)).xyz);
						break;
					case GravityFieldType.Sphere:
						direction = math.normalize(ltw.Position - gravityFieldLtw.Position);
						break;
				}

				var sideVec          = math.normalize(math.cross(direction, gravityFieldLtw.Forward));
				var localToDirection = new float3x3(sideVec, direction, math.normalize(math.cross(direction, sideVec)));
				gravity = math.mul(gravity, localToDirection);
				physicsVelocity.Linear += gravity * timeDelta / physicsMass.InverseMass;


				var angularMoveDir                 = math.cross(gravity, ltw.Up);
				var inertiaOrientationInWorldSpace = math.mul(ltw.Rotation.value, physicsMass.InertiaOrientation);
				var angularVelocityInertiaSpace =
					math.rotate(math.inverse(inertiaOrientationInWorldSpace), angularMoveDir);
				physicsVelocity.Angular += angularVelocityInertiaSpace * timeDelta * physicsMass.InverseInertia * componentGravityFieldTarget.AngularVelocityAffectanceRate;
			}).Schedule();
		}
	}
}
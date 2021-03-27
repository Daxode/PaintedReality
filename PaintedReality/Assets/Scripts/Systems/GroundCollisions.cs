using com.daxode.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace com.daxode.Systems {
	[UpdateAfter(typeof(EndFramePhysicsSystem))]
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	public class GroundCollisions : JobComponentSystem {
		private BuildPhysicsWorld _buildPhysicsWorld;
		private StepPhysicsWorld  _stepPhysicsWorld;

		protected override void OnCreate() {
			base.OnCreate();
			_buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
			_stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
		}

		private struct SystemJobOnCollisionGround : ICollisionEventsJob {
			public ComponentDataFromEntity<GravityFieldAttracted> AllGravityAffected;
			public ComponentDataFromEntity<LocalToWorld> AllLocalToWorld;
			
			public void Execute(CollisionEvent collisionEvent) {
				var entityA = collisionEvent.EntityA;
				var entityB = collisionEvent.EntityB;

				SetGroundStateOnEntity(entityA, collisionEvent);
				SetGroundStateOnEntity(entityB, collisionEvent);
			}

			private void SetGroundStateOnEntity(Entity entityA, CollisionEvent collisionEvent) {
				if (!AllGravityAffected.HasComponent(entityA)) return;

				var ltw = AllLocalToWorld[entityA];
				if (!(math.dot(ltw.Up, collisionEvent.Normal) > 0.4f)) return; //Only if ground, not sides
				
				var gravAffected = AllGravityAffected[entityA];
				gravAffected.IsOnGround = true;
				AllGravityAffected[entityA] = gravAffected;
			}
		}
		
		protected override JobHandle OnUpdate(JobHandle inputDeps) {
			var job = new SystemJobOnCollisionGround {
				AllGravityAffected = GetComponentDataFromEntity<GravityFieldAttracted>(),
				AllLocalToWorld = GetComponentDataFromEntity<LocalToWorld>()
			};

			var jobHandle = job.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
			return jobHandle;
		}
	}
}
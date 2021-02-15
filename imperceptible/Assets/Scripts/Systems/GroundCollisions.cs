using Imperceptible.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Imperceptible.Systems {
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
		
		struct SystemJobOnCollisionGround : ICollisionEventsJob {
			public ComponentDataFromEntity<GravityFieldAttracted> AllGravityAffected;

			public void Execute(CollisionEvent collisionEvent) {
				var entityA = collisionEvent.EntityA;
				var entityB = collisionEvent.EntityB;
			
				SetGroundStateOnEntity(entityA);
				SetGroundStateOnEntity(entityB);
			}

			private void SetGroundStateOnEntity(Entity entityA) {
				if (!AllGravityAffected.HasComponent(entityA)) return;
				
				var gravAffected = AllGravityAffected[entityA];
				gravAffected.IsOnGround = true;
				AllGravityAffected[entityA] = gravAffected;
			}
		}
		
		protected override JobHandle OnUpdate(JobHandle inputDeps) {
			var job = new SystemJobOnCollisionGround();
			job.AllGravityAffected = GetComponentDataFromEntity<GravityFieldAttracted>();

			var jobHandle = job.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
			return jobHandle;
		}
	}
}
using com.daxode.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace com.daxode.Systems.GravityFields {
	[UpdateAfter(typeof(EndFramePhysicsSystem))]
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	public class GFTrigger : JobComponentSystem {
		private BuildPhysicsWorld _buildPhysicsWorld;
		private StepPhysicsWorld  _stepPhysicsWorld;

		protected override void OnCreate() {
			base.OnCreate();
			_buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
			_stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
		}

		struct SystemJobOnTriggerGravityField : ITriggerEventsJob {
			[ReadOnly] public ComponentDataFromEntity<GravityFieldAttracted> AllGravityAffectables;
			public BufferFromEntity<DynamicBufferEntityElement> BufferOfGravityAffectEntities;
		
			public void Execute(TriggerEvent triggerEvent) {
				var entityA = triggerEvent.EntityA;
				var entityB = triggerEvent.EntityB;
			
				if (AllGravityAffectables.HasComponent(entityA)) { 
					BufferOfGravityAffectEntities[entityA].Add(entityB);
				}
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputDeps) {
			var job = new SystemJobOnTriggerGravityField() {
				AllGravityAffectables = GetComponentDataFromEntity<GravityFieldAttracted>(true),
				BufferOfGravityAffectEntities = GetBufferFromEntity<DynamicBufferEntityElement>()
			};

			var jobHandle = job.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
			jobHandle.Complete();
			return jobHandle;
		}
	}
}

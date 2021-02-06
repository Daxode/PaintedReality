using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using Material = UnityEngine.Material;

public class Spawner : SystemBase {
	protected override void OnCreate() {
		Addressables.LoadAssetAsync<Mesh>("model/sphere").Completed += handleMesh => {
			Addressables.LoadAssetAsync<Material>("mat/dark").Completed += handleMaterial => {
				var ree = EntityManager.CreateArchetype(
					typeof(Translation),
					typeof(Rotation),
					typeof(LocalToWorld),
					typeof(RenderMesh),
					typeof(RenderBounds),
					typeof(PerInstanceCullingTag),
					typeof(BlendProbeTag),
					typeof(PhysicsCollider)
				);
				var uwu = EntityManager.CreateEntity(ree);
				EntityManager.SetName(uwu,"Farkle");
				EntityManager.AddSharedComponentData(uwu, new RenderMesh {
					mesh = handleMesh.Result,
					material = handleMaterial.Result,
					castShadows = ShadowCastingMode.On,
					receiveShadows = true
				});
				var kage = EntityManager.GetComponentData<Translation>(uwu);
				kage.Value += new float3(0, 10, 0);
				EntityManager.SetComponentData(uwu, kage);
				EntityManager.SetComponentData(uwu, new RenderBounds {
					Value = new AABB{Extents = new float3(0.5f,0.5f,0.5f)}
				});
			};
		};
	}


	protected override void OnUpdate() {
		// Assign values to local variables captured in your job here, so that it has
		// everything it needs to do its work when it runs later.
		// For example,
		//     float deltaTime = Time.DeltaTime;

		// This declares a new kind of job, which is a unit of work to do.
		// The job is declared as an Entities.ForEach with the target components as parameters,
		// meaning it will process all entities in the world that have both
		// Translation and Rotation components. Change it to process the component
		// types you want.


		Entities.ForEach((ref Translation translation, in Rotation rotation) => {
			// Implement the work to perform for each entity here.
			// You should only access data that is local or that is a
			// field on this job. Note that the 'rotation' parameter is
			// marked as 'in', which means it cannot be modified,
			// but allows this job to run in parallel with other jobs
			// that want to read Rotation component data.
			// For example,
			//     translation.Value += math.mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;
		}).Schedule();
	}
}
using Imperceptible.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Imperceptible.Systems.GravityFields {
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[UpdateAfter(typeof(GFTrigger))]
	public class GFApplyPriorityField : SystemBase {
		protected override void OnUpdate() {
			var gravFields = GetComponentDataFromEntity<ComponentGravityField>();
			Entities.ForEach((ref DynamicBuffer<DynamicBufferEntityElement> buffer, ref ComponentGravityFieldAttracted gravityFieldAttracted) => {
				int max = -1;
				gravityFieldAttracted.GravityFieldCurrentEntity = Entity.Null;
				foreach (var entity in buffer) {
					var gravityFieldData = gravFields[entity];
					if (max < gravityFieldData.Priority) {
						max = gravityFieldData.Priority;
						gravityFieldAttracted.GravityFieldCurrentEntity = entity;
					}
				}
			}).Schedule();
		}
	}
}
using com.daxode.Components;
using Unity.Entities;

namespace com.daxode.Systems.GravityFields {
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[UpdateAfter(typeof(GFTrigger))]
	public class GFApplyPriorityField : SystemBase {
		protected override void OnUpdate() {
			var gravFields = GetComponentDataFromEntity<GravityField>();
			Entities.ForEach((ref DynamicBuffer<DynamicBufferEntityElement> buffer, ref GravityFieldAttracted gravityFieldAttracted) => {
				var max = -1;
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
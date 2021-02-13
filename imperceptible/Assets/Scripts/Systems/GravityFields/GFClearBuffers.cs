using Imperceptible.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Imperceptible.Systems.GravityFields {
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[UpdateBefore(typeof(GFTrigger))]
	public class GFClearBuffers : SystemBase {
		protected override void OnUpdate() {
			var gravFields = GetComponentDataFromEntity<ComponentGravityField>();
			Entities.ForEach((ref DynamicBuffer<DynamicBufferEntityElement> buffer) => {
				buffer.Clear();
			}).Schedule();
		}
	}
}
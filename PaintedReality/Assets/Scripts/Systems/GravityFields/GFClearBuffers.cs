using com.daxode.Components;
using Unity.Entities;

namespace com.daxode.Systems.GravityFields {
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[UpdateBefore(typeof(GFTrigger))]
	public class GFClearBuffers : SystemBase {
		protected override void OnUpdate() {
			Entities.WithAll<GravityFieldAttracted>().ForEach((ref DynamicBuffer<DynamicBufferEntityElement> buffer) => {
				buffer.Clear();
			}).Schedule();
		}
	}
}
using com.daxode.Components;
using Unity.Entities;

namespace com.daxode.Systems {
	[UpdateBefore(typeof(GroundCollisions))]
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	public class ClearGroundCollisions : SystemBase {
		protected override void OnUpdate() {
			Entities.ForEach((ref GravityFieldAttracted grav) => {
				grav.IsOnGround = false;
			}).Schedule();
		}
	}
}
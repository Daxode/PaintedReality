using Imperceptible.Components;
using Unity.Entities;

namespace Imperceptible.Systems {
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
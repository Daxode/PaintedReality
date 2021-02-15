using Unity.Entities;

namespace Imperceptible.Components {
	[GenerateAuthoringComponent]
	public struct LivingJumper : IComponentData {
		public bool  HasHitGround;
		public int   Jumps;
		public int   JumpsLeft;
		public float JumpTime;
		public float JumpTimeLeft;
		public bool  IsJumping;
		public bool  IsGoingUp;
		public bool  InitialJump;
	}
}

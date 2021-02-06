using System;
using imperceptible.input;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace imperceptible {
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	public class JumpyBalls : SystemBase {
		private InputActionAssetImperceptible _inputAsset;
		private float                         _timeDelta;
		
		protected override void OnCreate() {
			//var archetype = EntityManager.CreateArchetype();
			//var ree  = EntityManager.CreateEntity(archetype);
			_inputAsset = new InputActionAssetImperceptible();
			
			_timeDelta = Time.DeltaTime;
			Debug.Log("uwuwu");
			_inputAsset.Enable();
			
			/*_inputAsset.Player.Move.performed += context => {
				var moveVec    = context.ReadValue<Vector2>();
				var moveFloat2 = new float2(moveVec);
				Entities.ForEach((ref PhysicsVelocity physicsVelocity) => {
					physicsVelocity.Linear.xz += moveFloat2;
				}).Schedule();
			};*/
			
			_inputAsset.Player.Jump.performed += context => {
				Entities.ForEach((ref PhysicsVelocity physicsVelocity) => {
					physicsVelocity.Linear.y += 2;
				}).Schedule();
			};
			
			_inputAsset.Player.Move.performed += context => {
				_moveDel = context.ReadValue<Vector2>();
				_shouldMove = !_moveDel.Equals(float2.zero);
				if(_shouldMove) _moveDel *= _timeDelta * 10;
			};
		}

		private float2 _moveDel;
		private bool    _shouldMove = false;
		protected override void OnUpdate() {
			_timeDelta = Time.DeltaTime;
			var moveDel = _moveDel;
			if (_shouldMove) {
				Entities.ForEach((ref PhysicsVelocity physicsVelocity) => {
					physicsVelocity.Linear.xz += moveDel;
				}).Schedule();	
			}
		}
	}
}
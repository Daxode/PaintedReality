using com.daxode.Components;
using com.daxode.Components.Tags;
using com.daxode.input;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace com.daxode.Systems {
    [UpdateAfter(typeof(GroundCollisions))]
    [UpdateBefore(typeof(Jumps))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public class PlayerMovement : SystemBase {
        private MainInputAction _mia     = new MainInputAction();
        private float3          _moveVec = float3.zero;
        private Entity          _player;
        private bool            _isPlayerRdy;
        //private float3 
        
        
        protected override void OnStartRunning() {
            var queryPlayer = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<TagPlayer>());
            var entitiesPlayer = queryPlayer.ToEntityArray(Allocator.Temp);
            _player = entitiesPlayer[0];
            

            _mia.Player.Move.performed += context => {
                _moveVec.xy = context.ReadValue<Vector2>();
                var ltw = EntityManager.GetComponentData<LocalToWorld>(_player);
                _moveVec = (ltw.Forward * _moveVec.y) + (ltw.Right * _moveVec.x);
            };

            
            _mia.Player.Look.performed += context => {
                //Debug.Log(context.ReadValue<Vector2>());
                
            };

            _mia.Player.Fire.performed += context => {
                Debug.Log("BAMM WHHHat");
            };
            
            _mia.Player.QuickSelect.performed += context => {
                Debug.Log("TOO FAST FOR ME BOIIIIII");
            };
            
            _mia.Player.Move.canceled += context => _moveVec = 0;
            _mia.Player.Jump.performed += context => SetJumpState(true);
            _mia.Player.Jump.canceled += context => SetJumpState(false);

            _mia.Enable();
            _isPlayerRdy = true;
        }

        private void SetJumpState(bool val) {
            var livingJumper = EntityManager.GetComponentData<LivingJumper>(_player);
            livingJumper.IsJumping = val;
            EntityManager.SetComponentData(_player, livingJumper);
        }

        protected override void OnUpdate() {
            if (!_isPlayerRdy) return;
            
            var  timeDelta     = Time.DeltaTime;
            var moveDirection = timeDelta * _moveVec;
            Entities.WithAll<TagPlayer>().ForEach((ref PhysicsVelocity physicsVelocity, in GravityFieldAttracted gravityFieldAttracted) => {
                var speed = 10f;
                if (gravityFieldAttracted.IsOnGround) speed = 20f;
                physicsVelocity.Linear += moveDirection * speed;
            }).Schedule();
        }
    }
}

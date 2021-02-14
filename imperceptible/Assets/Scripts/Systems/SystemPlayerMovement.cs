using Imperceptible.Components.Tags;
using Imperceptible.Input;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Imperceptible.Systems {
    public class SystemPlayerMovement : SystemBase {
        private MainInputAction _mia     = new MainInputAction();
        private float3          _moveVec = float3.zero;
        private Entity          _player;
        private bool            _isPlayerRdy = false;
        
        protected override void OnStartRunning() {
            _mia.Player.Jump.performed += context => {
                Debug.Log("wuuuuw");
            };
            
            EntityQuery queryPlayer = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<TagPlayer>());
            var entitiesPlayer = queryPlayer.ToEntityArray(Allocator.Temp);
            _player = entitiesPlayer[0];

            _mia.Player.Move.performed += context => {
                _moveVec.xy = context.ReadValue<Vector2>();
                var ltw = EntityManager.GetComponentData<LocalToWorld>(_player);
                _moveVec = (ltw.Forward * _moveVec.y) + (ltw.Right * _moveVec.x);
                Debug.Log(_moveVec);
            };
            
            _mia.Player.Move.canceled += context => {
                _moveVec = 0;
            };
        
            _mia.Enable();
            _isPlayerRdy = true;
        }

        protected override void OnUpdate() {
            if (!_isPlayerRdy) return;
            
            float  timeDelta     = Time.DeltaTime;
            float3 moveDirection = timeDelta * _moveVec * 10;
            Entities.ForEach((ref PhysicsVelocity physicsVelocity, in TagPlayer tagPlayer) => {
                physicsVelocity.Linear += moveDirection;
            }).Schedule();
        }
    }
}

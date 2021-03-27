using com.daxode.Components;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace com.daxode.Systems {
    [UpdateAfter(typeof(GroundCollisions))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public class Jumps : SystemBase {
        protected override void OnUpdate() {
            var timeDelta     = Time.DeltaTime;
            
            Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref LivingJumper livingJumper, in GravityFieldAttracted gravityFieldAttracted, in LocalToWorld ltw) => {
                if (gravityFieldAttracted.IsOnGround) livingJumper.HasHitGround = true;
                
                if (livingJumper.IsJumping) {
                    if (livingJumper.InitialJump) {
                        livingJumper.HasHitGround = false;
                        livingJumper.InitialJump = false;
                        
                        Debug.Log("Do initial jump");
                        physicsVelocity.Linear += ltw.Up * 5; //Applies a jump
                    }
                }

                if ((livingJumper.HasHitGround) && livingJumper.IsJumping) {
                    livingJumper.InitialJump = true;
                }
            }).Schedule();
        }
    }
}

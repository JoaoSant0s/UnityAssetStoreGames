using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using Unity.Physics.Authoring;

using JoaoSantos.General;

namespace JoaoSantos.Runner3D.WorldElement
{
    [UpdateAfter(typeof(PlayerForwardMoveSystem))]
    public class PlayerJumpSystem : SystemBase
    {
        private BuildPhysicsWorld buildPhysicsWorld = default;

        protected override void OnCreate()
        {
            base.OnCreate();
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        }
        protected override void OnUpdate()
        {
            var world = buildPhysicsWorld.PhysicsWorld;

            Entities
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((ref PhysicsVelocity velocity, ref Translation translation, ref PlayerJumpComponentData playerJumpData, in CollisionFilterComponentData collisionFilterData) =>
            {
                Jump(ref velocity, ref translation, ref playerJumpData, in collisionFilterData);

                ResetJumpUpdate(ref playerJumpData);
            }).Run();
        }

        #region Private Methods

        private void Jump(ref PhysicsVelocity velocity, ref Translation translation, ref PlayerJumpComponentData playerJumpData, in CollisionFilterComponentData collisionFilterData)
        {
            if (!Input.GetKeyUp(KeyCode.Space)) return;

            var startPosition = translation.Value;
            var endPosition = startPosition + playerJumpData.raycastJumpOffset;

            if (!CheckRayCastPossibleJump(startPosition, endPosition, ref playerJumpData, in collisionFilterData) || playerJumpData.Jumping) return;

            playerJumpData.Jumping = true;
            playerJumpData.StartJumpTime = Time.ElapsedTime;

            float3 linearImpulse = new float3(0, playerJumpData.jumpForce, 0);

            ApplyImpulse(ref velocity, linearImpulse);
        }

        private bool CheckRayCastPossibleJump(float3 rayFrom, float3 rayTo, ref PlayerJumpComponentData playerJumpData, in CollisionFilterComponentData collisionFilterData)
        {
            var collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;

            Unity.Physics.RaycastInput input = new Unity.Physics.RaycastInput()
            {
                Start = rayFrom,
                End = rayTo,
                Filter = {
                    BelongsTo = collisionFilterData.BelongsToValue,
                    CollidesWith = collisionFilterData.CollidesWithValue,
                    GroupIndex = collisionFilterData.groupIndex
                }
            };

            RaycastHitTuple hit = new RaycastHitTuple();

            RaycastWrapper.SingleRayCast(collisionWorld, input, ref hit);

            return hit.collided;
        }

        private void ResetJumpUpdate(ref PlayerJumpComponentData playerJumpData)
        {
            if (!playerJumpData.Jumping) return;

            if (Time.ElapsedTime - playerJumpData.StartJumpTime < playerJumpData.resetJumpDelay) return;
            playerJumpData.Jumping = false;
        }

        #endregion

        public static void ApplyImpulse(ref PhysicsVelocity pv, float3 impulse)
        {
            pv.Linear += impulse;
        }
    }
}
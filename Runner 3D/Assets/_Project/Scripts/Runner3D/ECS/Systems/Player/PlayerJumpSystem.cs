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
            .ForEach((ref PhysicsVelocity velocity, ref Translation translation, ref Rotation rotation, in PlayerJumpComponentData playerJumpData) =>
            {
                Jump(ref velocity, ref translation, in playerJumpData);
            }).Run();
        }

        private void Jump(ref PhysicsVelocity velocity, ref Translation translation, in PlayerJumpComponentData playerJumpData)
        {
            if (!Input.GetKeyUp(KeyCode.Space)) return;

            var startPosition = translation.Value;
            var endPosition = startPosition + playerJumpData.raycastJumpOffset;

            if (!CheckPossibleJump(startPosition, endPosition, in playerJumpData)) return;

            float3 linearImpulse = new float3(0, playerJumpData.jumpForce, 0);

            ApplyImpulse(ref velocity, linearImpulse);
        }

        private bool CheckPossibleJump(float3 rayFrom, float3 rayTo, in PlayerJumpComponentData data)
        {
            var collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;

            var collisionFilterDescription = data.collisionFilterDescription;

            Unity.Physics.RaycastInput input = new Unity.Physics.RaycastInput()
            {
                Start = rayFrom,
                End = rayTo,
                Filter = {
                    BelongsTo = collisionFilterDescription.BelongsToValue,
                    CollidesWith = collisionFilterDescription.CollidesWithValue,
                    GroupIndex = collisionFilterDescription.groupIndex
                }
            };

            RaycastHitTuple hit = new RaycastHitTuple();

            RaycastWrapper.SingleRayCast(collisionWorld, input, ref hit);

            return hit.collided;
        }

        public static void ApplyImpulse(ref PhysicsVelocity pv, float3 impulse)
        {
            pv.Linear += impulse;
        }
    }
}
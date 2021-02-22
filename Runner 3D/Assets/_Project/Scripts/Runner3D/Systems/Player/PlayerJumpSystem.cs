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
            Entities
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((ref PhysicsVelocity velocity, ref Translation translation, ref Rotation rotation, in PlayerJumpComponentData data) =>
            {
                Jump(ref velocity, ref translation, in data);
                ResetRotation(ref rotation);
            }).Run();
        }

        private void Jump(ref PhysicsVelocity velocity, ref Translation translation, in PlayerJumpComponentData data)
        {
            if (!Input.GetKeyUp(KeyCode.Space)) return;

            var startPosition = translation.Value;
            var endPosition = startPosition + data.raycastJumpOffset;

            if (!CheckPossibleJump(startPosition, endPosition)) return;

            float3 linearImpulse = new float3(0, data.jumpForce, 0);

            ApplyImpulse(ref velocity, linearImpulse);

        }

        private bool CheckPossibleJump(float3 rayFrom, float3 rayTo)
        {
            var collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            Unity.Physics.RaycastInput input = new Unity.Physics.RaycastInput()
            {
                Start = rayFrom,
                End = rayTo,
                Filter = {
                    BelongsTo = ~2u,
                    CollidesWith = ~0u
                }
            };

            Unity.Physics.RaycastHit hit = new Unity.Physics.RaycastHit();

            RaycastWrapper.SingleRayCast(collisionWorld, input, ref hit);
            Debugs.Log(hit.Entity);

            return true;
        }

        private void ResetRotation(ref Rotation rotation)
        {
            rotation.Value = quaternion.Euler(0, 0, 0);
        }

        public static void ApplyImpulse(ref PhysicsVelocity pv, float3 impulse)
        {
            pv.Linear += impulse;
        }
    }
}
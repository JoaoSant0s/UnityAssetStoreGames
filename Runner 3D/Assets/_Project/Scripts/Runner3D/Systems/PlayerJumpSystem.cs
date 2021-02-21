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

namespace JoaoSantos.Runner3D.WorldElement
{
    [UpdateAfter(typeof(PlayerForwardMoveSystem))]
    public class PlayerJumpSystem : SystemBase
    {
        private BuildPhysicsWorld buildPhysicsWorld = default;

        protected override void OnCreate()
        {
            base.OnCreate();
            this.buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        }
        protected override void OnUpdate()
        {
            Entities
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref Rotation rotation, ref Translation translation, in PlayerMovementComponentData data) =>
            {
                Jump(entity, in data);
                ResetRotation(ref rotation);
            }).Run();
        }

        private void Jump(Entity entity, in PlayerMovementComponentData data)
        {
            if (!Input.GetKeyUp(KeyCode.Space)) return;

            var index = PhysicsWorldExtensions.GetRigidBodyIndex(this.buildPhysicsWorld.PhysicsWorld, entity);
            float3 linearImpulse = data.jumpForce * new float3(0, 1f, 0);
            Debugs.Log(entity, index, linearImpulse);

            this.buildPhysicsWorld.PhysicsWorld.ApplyLinearImpulse(index, linearImpulse);            
        }

        private void ResetRotation(ref Rotation rotation)
        {
            rotation.Value = quaternion.Euler(0, 0, 0);
        }
    }
}
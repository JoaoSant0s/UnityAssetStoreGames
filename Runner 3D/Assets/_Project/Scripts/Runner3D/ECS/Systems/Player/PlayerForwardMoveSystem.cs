using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

using JoaoSantos.General;

namespace JoaoSantos.Runner3D.WorldElement
{
    [AlwaysSynchronizeSystem]
    public class PlayerForwardMoveSystem : JobComponentSystem
    {

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var dt = Time.DeltaTime;

            Entities
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((ref PhysicsVelocity velocity, ref Rotation rotation, in PlayerMovementComponentData data) =>
            {
                if (!data.enableForwardMove) return;

                ApplyForwardMovement(ref velocity, dt, in data);

                rotation.FreezeRotation();
            }).Run();
            
            return default;
        }        

        private void ApplyForwardMovement(ref PhysicsVelocity velocity, float dt, in PlayerMovementComponentData data)
        {
            var zVelocity = velocity.Linear.z + data.speed * dt;

            if (zVelocity > data.maxVelocity)
            {
                zVelocity = data.maxVelocity;
            }

            velocity.Linear.z = zVelocity;
        }
    }
}
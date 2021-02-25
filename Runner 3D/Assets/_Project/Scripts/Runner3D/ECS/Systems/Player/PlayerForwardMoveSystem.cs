using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

namespace JoaoSantos.Runner3D.WorldElement
{
    public class PlayerForwardMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;

            Entities
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((ref PhysicsVelocity velocity, ref Rotation rotation, in PlayerMovementComponentData data) =>
            {
                if (!data.isForwardMoving) return;

                ApplyForwardMovement(ref velocity, dt, in data);
            }).Run();
        }

        private void ApplyForwardMovement(ref PhysicsVelocity velocity, float dt, in PlayerMovementComponentData data)
        {
            if (velocity.Linear.z >= data.maxVelocity) return;
            velocity.Linear += new float3(0, 0, data.speed * dt);
        }
    }
}
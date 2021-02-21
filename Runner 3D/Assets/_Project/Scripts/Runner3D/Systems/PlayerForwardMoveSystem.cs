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
            Entities
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((ref PhysicsVelocity velocity, ref Rotation rotation, in PlayerMovementComponentData data) =>
            {
                ApplyForwardMovement(ref velocity, in data);
                ResetRotation(ref rotation);
            }).Run();
        }

        private void ApplyForwardMovement(ref PhysicsVelocity velocity, in PlayerMovementComponentData data)
        {
            velocity.Linear = new float3(0, 0, data.limitSpeed);
        }

        private void ResetRotation(ref Rotation rotation)
        {
            rotation.Value = quaternion.Euler(0, 0, 0);
        }

    }
}
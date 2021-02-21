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
    [UpdateAfter(typeof(PlayerForwardMoveSystem))]
    public class PlayerDirectionMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
           .WithAll<PlayerTag>()
           .WithoutBurst()
           .ForEach((ref Translation translation, ref Rotation rotation, ref PlayerMovementComponentData data) =>
           {
               PlayerMovementTrigger(ref translation, ref data);
               ResetRotation(ref rotation);
           }).Run();
        }

        private void ResetRotation(ref Rotation rotation)
        {
            rotation.Value = quaternion.Euler(0, 0, 0);
        }

        private void PlayerMovementTrigger(ref Translation translation, ref PlayerMovementComponentData data)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                MoveLeft(ref translation, ref data);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                MoveRight(ref translation, ref data);
            }
        }

        private void MoveLeft(ref Translation translation, ref PlayerMovementComponentData data)
        {
            if (!data.CanMoveLeft) return;

            data.movementIndex--;
            translation.Value += new float3(-data.transversalMovement.movementDistance, 0, 0);
        }

        private void MoveRight(ref Translation translation, ref PlayerMovementComponentData data)
        {
            if (!data.CanMoveRight) return;

            data.movementIndex++;
            translation.Value += new float3(data.transversalMovement.movementDistance, 0, 0);
        }
    }
}

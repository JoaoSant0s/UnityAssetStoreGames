
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


namespace JoaoSantos.General
{

    public struct RaycastHitTuple
    {
        public RaycastHit raycastHit;
        public bool collided;
    }
    public static class RaycastWrapper
    {
        [BurstCompile]
        public struct RaycastJob : IJobParallelFor
        {
            [ReadOnly] public CollisionWorld world;
            [ReadOnly] public NativeArray<RaycastInput> inputs;
            public NativeArray<RaycastHitTuple> results;

            public unsafe void Execute(int index)
            {
                RaycastHit hit;
                var collided = world.CastRay(inputs[index], out hit);
                results[index] = new RaycastHitTuple { raycastHit = hit, collided = collided };
            }
        }

        public static JobHandle ScheduleBatchRayCast(CollisionWorld world, NativeArray<RaycastInput> inputs, NativeArray<RaycastHitTuple> results)
        {
            JobHandle rcj = new RaycastJob
            {
                inputs = inputs,
                results = results,
                world = world

            }.Schedule(inputs.Length, 5);
            return rcj;
        }

        public static void SingleRayCast(CollisionWorld world, RaycastInput input, ref RaycastHitTuple result)
        {
            var rayInputs = new NativeArray<RaycastInput>(1, Allocator.TempJob);
            var rayResults = new NativeArray<RaycastHitTuple>(1, Allocator.TempJob);
            rayInputs[0] = input;
            var handle = ScheduleBatchRayCast(world, rayInputs, rayResults);
            handle.Complete();
            result = rayResults[0];
            rayInputs.Dispose();
            rayResults.Dispose();
        }
    }
}
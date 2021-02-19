using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

using JoaoSantos.General;

namespace JoaoSantos.Runner3D.WorldElement
{
    [AlwaysSynchronizeSystem]
    [UpdateAfter(typeof(PickupCollectableSystem))]
    public class DeleteEntitySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

            Entities
                .WithAll<DeleteTag>()
                .ForEach((Entity entity) =>
                {
                    commandBuffer.DestroyEntity(entity);
                }).Run();

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();

            return default;

        }
    }
}
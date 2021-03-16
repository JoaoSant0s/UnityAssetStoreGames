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

            var elapsedTime = Time.ElapsedTime;

            Entities
                .ForEach((Entity entity, DeleteComponent deleted) =>
                {
                    if (elapsedTime - deleted.startTime < deleted.delay) return;

                    commandBuffer.DestroyEntity(entity);                    
                }).Run();

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();

            return default;

        }
    }
}
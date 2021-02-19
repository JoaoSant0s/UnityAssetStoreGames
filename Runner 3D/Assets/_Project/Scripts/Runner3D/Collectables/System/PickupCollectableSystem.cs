using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

using JoaoSantos.General;

namespace JoaoSantos.Runner3D.WorldElement
{
    [AlwaysSynchronizeSystem]
    [UpdateAfter(typeof(SpawnCollectablesSystem))]
    public class PickupCollectableSystem : JobComponentSystem
    {
        private BeginInitializationEntityCommandBufferSystem bufferSystem;
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;

        protected override void OnCreate()
        {
            base.OnCreate();
            bufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            TriggerJob triggerJob = new TriggerJob
            {
                players = GetComponentDataFromEntity<PlayerTag>(true),
                tracks = GetComponentDataFromEntity<TrackTag>(true),
                entitiesToDelete = GetComponentDataFromEntity<DeleteTag>(true),
                commandBuffer = bufferSystem.CreateCommandBuffer()
            };

            var job = triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

            job.Complete();

            return job;
        }

        private struct TriggerJob : ITriggerEventsJob
        {
            [ReadOnly]
            public ComponentDataFromEntity<PlayerTag> players;
            [ReadOnly]
            public ComponentDataFromEntity<TrackTag> tracks;

            [ReadOnly]
            public ComponentDataFromEntity<DeleteTag> entitiesToDelete;

            public EntityCommandBuffer commandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                EntityTrigger(triggerEvent.EntityA, triggerEvent.EntityB);
                EntityTrigger(triggerEvent.EntityB, triggerEvent.EntityA);
            }

            private void EntityTrigger(Entity entityA, Entity entityB)
            {
                Debugs.Log("Check", players, tracks, entitiesToDelete, entityA, entityB);

                if (!players.HasComponent(entityA)) return;
                if (tracks.HasComponent(entityB)) return;

                Debugs.Log("HasComponent", entityA);

                if (entitiesToDelete.HasComponent(entityB)) return;

                Debugs.Log("!HasComponent", entityB);

                commandBuffer.AddComponent(entityB, new DeleteTag());
            }
        }
    }
}
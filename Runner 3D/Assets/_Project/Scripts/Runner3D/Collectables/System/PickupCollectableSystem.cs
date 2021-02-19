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
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class PickupCollectableSystem : JobComponentSystem
    {
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;
        private EndSimulationEntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            TriggerJob triggerJob = new TriggerJob
            {
                players = GetComponentDataFromEntity<PlayerTag>(),
                tracks = GetComponentDataFromEntity<TrackTag>(),
                entitiesToDelete = GetComponentDataFromEntity<DeleteTag>(),
                entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
            };

            // triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

            var jobHandle = triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

            commandBufferSystem.AddJobHandleForProducer(jobHandle);            

            // jobHandle.Complete();

            return jobHandle;
        }
        
        private struct TriggerJob : ITriggerEventsJob
        {
            [ReadOnly]
            public ComponentDataFromEntity<PlayerTag> players;
            [ReadOnly]
            public ComponentDataFromEntity<TrackTag> tracks;
            [ReadOnly]
            public ComponentDataFromEntity<DeleteTag> entitiesToDelete;

            public EntityCommandBuffer entityCommandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                EntityTrigger(triggerEvent.EntityA, triggerEvent.EntityB);
               // EntityTrigger(triggerEvent.EntityB, triggerEvent.EntityA);
            }

            private void EntityTrigger(Entity entityA, Entity entityB)
            {
                Debugs.Log("Check", players, tracks, entitiesToDelete, entityA, entityB);

                if (!players.HasComponent(entityA)) return;
                if (tracks.HasComponent(entityB)) return;

                Debugs.Log("HasComponent", entityA);

                if (entitiesToDelete.HasComponent(entityB)) return;

                Debugs.Log("!HasComponent", entityB);

                entityCommandBuffer.AddComponent(entityB, new DeleteTag());                
            }
        }
    }
}
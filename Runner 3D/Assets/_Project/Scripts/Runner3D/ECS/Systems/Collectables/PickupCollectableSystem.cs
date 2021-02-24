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
    [UpdateAfter(typeof(PlayerForwardMoveSystem))]
    [UpdateAfter(typeof(StepPhysicsWorld))]
    [UpdateBefore(typeof(EndFramePhysicsSystem))]
    public class PickupCollectableSystem : SystemBase
    {
        #region Systems

        private ExportPhysicsWorld exportPhysicsWorld = default;
        private BuildPhysicsWorld buildPhysicsWorld = default;
        private StepPhysicsWorld stepPhysicsWorld = default;
        private EndFramePhysicsSystem endFramePhysicsSystem = default;
        private EndSimulationEntityCommandBufferSystem commandBufferSystem;

        #endregion

        private EntityQuery collectableQuery = default;

        protected override void OnCreate()
        {
            base.OnCreate();
            exportPhysicsWorld = World.GetOrCreateSystem<ExportPhysicsWorld>();

            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            endFramePhysicsSystem = World.GetOrCreateSystem<EndFramePhysicsSystem>();
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            collectableQuery = GetEntityQuery(
                new EntityQueryDesc
                {
                    All = new ComponentType[] { typeof(CollectableComponentData) }
                }
            );
        }

        protected override void OnUpdate()
        {
            MainTrigger();
        }

        private void MainTrigger()
        {
            var amount = collectableQuery.CalculateEntityCount();

            if (amount == 0) return;

            Dependency = JobHandle.CombineDependencies(exportPhysicsWorld.GetOutputDependency(), Dependency);
            Dependency = JobHandle.CombineDependencies(stepPhysicsWorld.FinalSimulationJobHandle, Dependency);

            Debugs.Log("Collectable Amount", amount);

            TriggerJob triggerJob = new TriggerJob
            {
                players = GetComponentDataFromEntity<PlayerTag>(),
                tracks = GetComponentDataFromEntity<TrackTag>(),
                entitiesToDelete = GetComponentDataFromEntity<DeleteTag>(),
                entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
            };

            Dependency = triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);

            Dependency.Complete();

            endFramePhysicsSystem.AddInputDependency(Dependency);
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
                EntityTrigger(triggerEvent.EntityB, triggerEvent.EntityA);
            }

            private void EntityTrigger(Entity entityA, Entity entityB)
            {
                if (!players.HasComponent(entityA)) return;
                if (tracks.HasComponent(entityB)) return;

                if (entitiesToDelete.HasComponent(entityB)) return;

                entityCommandBuffer.AddComponent(entityB, new DeleteTag());
            }
        }
    }
}
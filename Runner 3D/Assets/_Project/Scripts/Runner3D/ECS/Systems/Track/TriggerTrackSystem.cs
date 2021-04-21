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
    public class TriggerTrackSystem : SystemBase
    {
        #region Systems
        private ExportPhysicsWorld m_ExportPhysicsWorld;
        private BuildPhysicsWorld buildPhysicsWorld = default;
        private StepPhysicsWorld stepPhysicsWorld = default;
        private EndFramePhysicsSystem endFramePhysicsSystem = default;
        private EndSimulationEntityCommandBufferSystem commandBufferSystem;

        #endregion

        private EntityQuery trackQuery = default;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_ExportPhysicsWorld = World.GetOrCreateSystem<ExportPhysicsWorld>();

            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            endFramePhysicsSystem = World.GetOrCreateSystem<EndFramePhysicsSystem>();
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            trackQuery = GetEntityQuery(typeof(TrackTag));
        }

        protected override void OnUpdate()
        {
            TriggerTrackJob();
        }

        private void TriggerTrackJob()
        {
            var amount = trackQuery.CalculateEntityCount();

            if (amount == 0) return;

            Dependency = JobHandle.CombineDependencies(m_ExportPhysicsWorld.GetOutputDependency(), Dependency);
            Dependency = JobHandle.CombineDependencies(stepPhysicsWorld.FinalSimulationJobHandle, Dependency);

            TriggerJob triggerJob = new TriggerJob
            {
                players = GetComponentDataFromEntity<PlayerTag>(),
                tracks = GetComponentDataFromEntity<TrackTag>(),
                entitiesTriggered = GetComponentDataFromEntity<TrackTriggeredTag>(),

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
            public ComponentDataFromEntity<TrackTriggeredTag> entitiesTriggered;

            public EntityCommandBuffer entityCommandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                EntityTrigger(triggerEvent.EntityA, triggerEvent.EntityB);
                EntityTrigger(triggerEvent.EntityB, triggerEvent.EntityA);
            }

            private void EntityTrigger(Entity entityA, Entity entityB)
            {
                if (!players.HasComponent(entityA)) return;
                if (!tracks.HasComponent(entityB)) return;

                if (entitiesTriggered.HasComponent(entityB)) return;

                entityCommandBuffer.AddComponent(entityB, new TrackTriggeredTag());
                entityCommandBuffer.AddSharedComponent(entityB, new TrackTriggeredSharedData());
            }
        }
    }
}
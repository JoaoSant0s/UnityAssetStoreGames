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
    [UpdateAfter(typeof(TriggerTrackSystem))]
    public class SpawnTrackSystem : SystemBase
    {
        private const float trackYOffsetPosition = 0f;

        private EntityQuery triggedEntityQuery = default;

        private float nextTrackPosition = default;
        private float nextYPosition = default;

        #region Unity Methods

        protected override void OnCreate()
        {
            base.OnCreate();
            triggedEntityQuery = GetEntityQuery(typeof(TrackTriggeredTag), typeof(TrackTriggeredSharedData));

            triggedEntityQuery.SetSharedComponentFilter(new TrackTriggeredSharedData() { actived = false });
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            CalculateNextTrackPosition();
        }

        protected override void OnUpdate()
        {            
            if (!LevelManager.Instance.HasAsset()) return;

            var querySize = triggedEntityQuery.CalculateEntityCount();

            if (querySize == 0) return;

            var triggers = triggedEntityQuery.ToEntityArray(Allocator.TempJob);
            var mainTrigger = triggers[0];

            EntityManager.SetSharedComponentData<TrackTriggeredSharedData>(mainTrigger, new TrackTriggeredSharedData() { actived = true });
            EntityManager.AddComponentData<DeleteComponent>(mainTrigger, new DeleteComponent(){
                delay = 3f,
                startTime = Time.ElapsedTime
            });

            var asset = LevelManager.Instance.CurrentAsset;

            Entity trackPrefab = TrackGenerator.Instance.GetTrackPrefab(asset);

            NativeArray<Entity> instancesArray = new NativeArray<Entity>(1, Allocator.Temp);
            EntityManager.Instantiate(trackPrefab, instancesArray);

            var trackEntity = instancesArray[0];

            var trackData = EntityManager.GetComponentData<TrackComponentData>(trackEntity);

            this.nextYPosition += trackYOffsetPosition;
            this.nextTrackPosition += trackData.size;

            float3 position = new float3(0, this.nextYPosition, this.nextTrackPosition);

            Debugs.Log("position", trackEntity, position);

            EntityManager.UpdateTranslationComponentData(trackEntity, position);

            instancesArray.Dispose();
            triggers.Dispose();

            LevelManager.Instance.UpdateToNextLevel();
        }

        #endregion

        #region Private Methods

        private void CalculateNextTrackPosition()
        {
            Entities
                .WithoutBurst()
                .ForEach((Entity entity, ref TrackComponentData track) =>
                {
                    LocalToWorld world = EntityManager.GetComponentData<LocalToWorld>(entity);
                    var position = world.Position;

                    this.nextYPosition = position.y;
                    this.nextTrackPosition = position.z;
                }).Run();
        }

        #endregion
    }
}
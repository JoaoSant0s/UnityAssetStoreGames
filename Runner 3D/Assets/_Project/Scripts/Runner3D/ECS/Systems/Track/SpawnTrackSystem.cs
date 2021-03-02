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
        private const float trackYOffsetPosition = -0.1f;

        private EntityQuery triggedEntityQuery = default;

        private float nextTrackPosition = default;
        private float nextYPosition = default;

        #region Unity Methods

        protected override void OnCreate()
        {
            base.OnCreate();
            triggedEntityQuery = GetEntityQuery(typeof(TrackTriggeredTag));
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

            var asset = LevelManager.Instance.CurrentAsset;
            Debugs.Log("TrackTriggeredTag", querySize, asset);

            //Get Level Track
            // Entity trackPrefab = default;

            // NativeArray<Entity> instancesArray = new NativeArray<Entity>(1, Allocator.Temp);
            // EntityManager.Instantiate(trackPrefab, instancesArray);

            // var trackEntity = instancesArray[0];

            // var trackData = EntityManager.GetComponentData<TrackComponentData>(trackEntity);
            // float3 position = new float3(0, this.nextYPosition, this.nextTrackPosition);
            // EntityManager.UpdateTranslationComponentData(trackEntity, position);

            // this.nextYPosition += trackYOffsetPosition;
            // this.nextTrackPosition += trackData.size;

            // instancesArray.Dispose();        
        }

        #endregion

        #region Private Methods

        private void CalculateNextTrackPosition()
        {
            Entities
                .WithoutBurst()
                .ForEach((ref TrackComponentData track) =>
                {
                    this.nextTrackPosition += track.size;
                    this.nextYPosition += trackYOffsetPosition;
                }).Run();

            Debugs.Log("Direct", this.nextTrackPosition, this.nextYPosition);
        }

        #endregion
    }
}
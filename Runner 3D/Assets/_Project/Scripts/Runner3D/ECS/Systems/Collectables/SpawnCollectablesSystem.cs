
using System;
using Unity.Entities;
using Unity.Collections;

using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

using Unity.Jobs;
using Unity.Burst;

using JoaoSantos.General;

using UnityEngine;

namespace JoaoSantos.Runner3D.WorldElement
{
    [UpdateAfter(typeof(PlayerForwardMoveSystem))]
    [UpdateBefore(typeof(TriggerTrackSystem))]
    public class SpawnCollectablesSystem : SystemBase
    {
        private EntityQuery collectablePointQuery = default;

        protected override void OnCreate()
        {
            collectablePointQuery = GetEntityQuery(typeof(CollectablePointTag), typeof(CollectablePointSharedData));

            collectablePointQuery.SetSharedComponentFilter(new CollectablePointSharedData() { spawned = false });
        }

        protected override void OnUpdate()
        {
            SpawnCollectables();
        }

        private void SpawnCollectables()
        {
            var size = collectablePointQuery.CalculateEntityCount();

            if (size == 0) return;

            Entities
            .WithStructuralChanges()
            .ForEach((ref PrefabEntity prefabEntity) =>
            {
                var collectablePrefab = prefabEntity.collectablePrefab;

                NativeArray<Entity> instancesArray = new NativeArray<Entity>(size, Allocator.Temp);
                EntityManager.Instantiate(collectablePrefab, instancesArray);

                var entities = collectablePointQuery.ToEntityArray(Allocator.TempJob);

                for (int i = 0; i < size; i++)
                {
                    var entityPoint = entities[i];
                    var collectableInstance = instancesArray[i];

                    LocalToWorld world = EntityManager.GetComponentData<LocalToWorld>(entityPoint);

                    EntityManager.UpdateTranslationComponentData(collectableInstance, world.Position);                    

                    SetStartLocalPosition(collectableInstance);

                    EntityManager.SetSharedComponentData<CollectablePointSharedData>(entityPoint, new CollectablePointSharedData() { spawned = true });
                }

                instancesArray.Dispose();
                entities.Dispose();

            }).Run();
        }

        private void SetStartLocalPosition(Entity entity)
        {
            var collectable = EntityManager.GetComponentData<CollectableComponentData>(entity);
            var translation = EntityManager.GetComponentData<Translation>(entity);

            collectable.StartPosition = translation.Value;

            EntityManager.SetComponentData<CollectableComponentData>(entity, collectable);
        }
    }
}
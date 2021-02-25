
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
    [UpdateBefore(typeof(TrackSpawnSystem))]
    public class SpawnCollectablesSystem : SystemBase
    {
        private EntityQuery collectablePointQuery = default;

        protected override void OnCreate()
        {
            collectablePointQuery = GetEntityQuery(typeof(Translation), typeof(CollectablePointTag), typeof(CollectablePointSharedData));

            collectablePointQuery.SetSharedComponentFilter(new CollectablePointSharedData() { spawned = false });
        }

        protected override void OnUpdate()
        {
            SpawnCollectables();
        }

        private void SpawnCollectables()
        {
            var size = collectablePointQuery.CalculateEntityCount();
            Debug.Log(size);

            if (size == 0) return;

            Entities
            .WithStructuralChanges()
            .ForEach((ref PrefabEntity prefabEntity) =>
            {
                var collectableEntity = prefabEntity.collectablePrefab;

                NativeArray<Entity> nativeArray = new NativeArray<Entity>(size, Allocator.Temp);
                EntityManager.Instantiate(collectableEntity, nativeArray);

                var entities = collectablePointQuery.ToEntityArray(Allocator.TempJob);

                for (int i = 0; i < size; i++)
                {
                    var entity = entities[i];

                    Translation point = EntityManager.GetComponentData<Translation>(entity);

                    Debugs.Log(entity, point.Value);

                    EntityManager.SetComponentData<Translation>(nativeArray[i], point);
                    EntityManager.SetSharedComponentData<CollectablePointSharedData>(entity, new CollectablePointSharedData() { spawned = true });
                }

                nativeArray.Dispose();
                entities.Dispose();

            }).Run();
        }
    }
}
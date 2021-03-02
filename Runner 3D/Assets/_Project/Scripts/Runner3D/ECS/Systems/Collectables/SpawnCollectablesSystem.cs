
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

            Debugs.Log("Instantiate collectable amount", size);

            Entities
            .WithStructuralChanges()
            .ForEach((ref PrefabEntity prefabEntity) =>
            {
                var collectableEntity = prefabEntity.collectablePrefab;

                NativeArray<Entity> instancesArray = new NativeArray<Entity>(size, Allocator.Temp);
                EntityManager.Instantiate(collectableEntity, instancesArray);

                var entities = collectablePointQuery.ToEntityArray(Allocator.TempJob);

                for (int i = 0; i < size; i++)
                {
                    var entity = entities[i];

                    LocalToWorld world = EntityManager.GetComponentData<LocalToWorld>(entity);

                    EntityManager.UpdateTranslationComponentData(instancesArray[i], world.Position);

                    EntityManager.SetSharedComponentData<CollectablePointSharedData>(entity, new CollectablePointSharedData() { spawned = true });
                }

                instancesArray.Dispose();
                entities.Dispose();

            }).Run();
        }
    }
}
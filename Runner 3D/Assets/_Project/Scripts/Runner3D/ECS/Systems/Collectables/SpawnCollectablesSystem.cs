
using System;
using Unity.Entities;
using Unity.Collections;

using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

using Unity.Jobs;
using Unity.Burst;

using JoaoSantos.General;


namespace JoaoSantos.Runner3D.WorldElement
{
    [UpdateAfter(typeof(PlayerForwardMoveSystem))]
    public class SpawnCollectablesSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            if (!SpawnCollectablesManager.Instance.HasSpawnStack()) return;

            SpawnCollectables();
        }

        private void SpawnCollectables()
        {
            var spawn = SpawnCollectablesManager.Instance.GetSpawn();

            Entities.ForEach((ref PrefabEntity prefabEntity) =>
            {
                var collectableEntity = prefabEntity.collectablePrefab;

                var points = spawn.SpawnCollectablePoints;

                NativeArray<Entity> nativeArray = new NativeArray<Entity>(points.Count, Allocator.Temp);
                EntityManager.Instantiate(collectableEntity, nativeArray);

                for (int i = 0; i < points.Count; i++)
                {
                    EntityManager.SetComponentData<Translation>(nativeArray[i], new Translation { Value = points[i].position });
                }

                nativeArray.Dispose();

            });
        }
    }
}
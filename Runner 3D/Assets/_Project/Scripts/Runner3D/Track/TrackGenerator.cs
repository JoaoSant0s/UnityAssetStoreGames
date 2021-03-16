using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using JoaoSantos.General;
using JoaoSantos.General.Asset;
using System;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

namespace JoaoSantos.Runner3D.WorldElement
{
    [DisallowMultipleComponent]

    public class TrackGenerator : SingletonBehaviour<TrackGenerator>, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        [Header("Prefabs")]
        [SerializeField]
        private List<GameObject> basePrefabs;

        private List<Entity> entitiesPrefabs;

        #region  Unity Methods        

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            this.entitiesPrefabs = new List<Entity>();
            for (int i = 0; i < basePrefabs.Count; i++)
            {
                var prefab = basePrefabs[i];
                Entity entityPrefab = conversionSystem.GetPrimaryEntity(prefab);
                this.entitiesPrefabs.Add(entityPrefab);
            }            
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.AddRange(basePrefabs);
        }

        #endregion

        public Entity GetTrackPrefab(PoolAsset asset)
        {
            var entityPrefab = entitiesPrefabs.Find(entity =>
            {
                var contains = ECSWrapper.EntityManager.HasComponent<TrackComponentData>(entity);
                if (!contains) return false;

                var value = ECSWrapper.EntityManager.GetComponentData<TrackComponentData>(entity);
                return value.id == asset.id;
            });
            return entityPrefab;
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

namespace JoaoSantos.General
{    
    [AddComponentMenu("Custom Authoring/Leader Authoring")]
    public class LeaderAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject followerObject;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            FollowEntity followEntity = followerObject.GetComponent<FollowEntity>();

            if (followEntity == null)
            {
                followEntity = followerObject.AddComponent<FollowEntity>();
            }

            followEntity.entityToFollow = entity;
        }
    }
}
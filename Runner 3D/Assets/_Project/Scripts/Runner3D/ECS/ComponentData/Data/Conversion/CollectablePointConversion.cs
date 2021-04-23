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

namespace JoaoSantos.Runner3D.WorldElement
{
    public class CollectablePointConversion : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent(entity, typeof(CollectablePointTag));
            dstManager.AddSharedComponentData(entity, new CollectablePointSharedData() { spawned = false });
        }
    }
}


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

namespace JoaoSantos.General
{
    public static class EntityManagerExtensions
    {
        public static void UpdateTranslationComponentData(this EntityManager manager, Entity entity, float3 value)
        {
            var translation = manager.GetComponentData<Translation>(entity);

            translation.Value = value;

            manager.SetComponentData<Translation>(entity, translation);
        }
    }
}
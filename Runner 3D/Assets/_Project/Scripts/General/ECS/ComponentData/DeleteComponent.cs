using UnityEngine;


using Unity.Entities;
using Unity.Collections;

using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

using Unity.Jobs;
using Unity.Burst;

namespace JoaoSantos.General
{
    [GenerateAuthoringComponent]
    public struct DeleteComponent : IComponentData
    {
        public float delay;
        public double startTime;
    }
}

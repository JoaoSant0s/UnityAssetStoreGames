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
    public struct TrackTriggeredSharedData : ISharedComponentData
    {
        public bool actived;
    }
}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Entities;
using Unity.Collections;

using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

using Unity.Jobs;
using Unity.Burst;

using Unity.Physics;
using Unity.Physics.Authoring;
using JoaoSantos.General;

namespace JoaoSantos.Runner3D.WorldElement
{
    [GenerateAuthoringComponent]
    public struct TrackComponentData : IComponentData
    {        
        public float size;

    }
}
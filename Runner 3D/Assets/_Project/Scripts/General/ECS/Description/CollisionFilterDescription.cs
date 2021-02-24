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

namespace JoaoSantos.General
{
    [Serializable]
    public class CollisionFilterDescription
    {
        public PhysicsCategoryTags belongsTo;

        public PhysicsCategoryTags collidesWith;

        public int groupIndex;

        public uint BelongsToValue
        {
            get { return belongsTo.Value; }
        }

        public uint CollidesWithValue
        {
            get { return collidesWith.Value; }
        }
    }
}
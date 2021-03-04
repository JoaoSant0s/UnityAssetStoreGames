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
    public class ECSWrapper
    {
        public static EntityManager EntityManager
        {
            get { return World.DefaultGameObjectInjectionWorld.EntityManager; }
        }
    }
}
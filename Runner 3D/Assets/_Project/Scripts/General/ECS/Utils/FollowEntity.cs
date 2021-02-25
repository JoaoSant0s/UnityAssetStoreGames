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
    public class FollowEntity : MonoBehaviour
    {
        public Entity entityToFollow;

        private EntityManager manager;

        private void Awake()    
        {
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void LateUpdate()
        {
            Translation endPos = manager.GetComponentData<Translation>(entityToFollow);

            transform.position = endPos.Value;
        }
    }
}
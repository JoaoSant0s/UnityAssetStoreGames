using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;


namespace JoaoSantos.General
{
    public class DotsWrapper
    {
        private static EntityManager EntityManager
        {
            get
            {
                return World.DefaultGameObjectInjectionWorld.EntityManager;
            }
        }

        public static void Instantiate(Entity entity, NativeArray<Entity> entities)
        {            
            EntityManager.Instantiate(entity, entities);
        }

        public static EntityArchetype CreateArchetype(params ComponentType[] types)
        {            
            return EntityManager.CreateArchetype(types);
        }

        public static void CreateEntity(EntityArchetype type, NativeArray<Entity> entities)
        {            
            EntityManager.CreateEntity(type, entities);
        }
    }
}


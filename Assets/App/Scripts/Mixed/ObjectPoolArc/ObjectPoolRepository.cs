using System;
using System.Collections.Generic;
using App.Scripts.Libs.Architecture;
using App.Scripts.Libs.ObjectPool;

namespace App.Scripts.Mixed.ObjectPoolArc
{
    public sealed class ObjectPoolRepository : Repository
    {
        private Dictionary<Type, object> _objectPools;
        
        public override void Initialize()
        {
            _objectPools = new Dictionary<Type, object>();
        }

        public override void Save()
        {
        }
        
        public T GetObjectFromPool<T>() where T : IPoolable, new()
        {
            var objPool = GetObjectPool<T>();
            return objPool.Get();
        }
        
        public void ReturnObjectToPool<T>(T obj) where T : IPoolable, new ()
        {
            var objPool = GetObjectPool<T>();
            objPool.Return(obj);
        }

        private ObjectPool<T> GetObjectPool<T>() where T : IPoolable, new()
        {
            var type = typeof(T);

            if (_objectPools.TryGetValue(type, out var pool))
            {
                return (ObjectPool<T>)pool;
            }
            
            var objectPool = new ObjectPool<T>();
            _objectPools[type] = objectPool;
            return objectPool;
        }

    }
}
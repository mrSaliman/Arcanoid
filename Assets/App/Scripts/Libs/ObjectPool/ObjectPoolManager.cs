using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Libs.ObjectPool
{
    public class ObjectPoolManager
    {
        private Dictionary<Type, object> _poolDictionary = new();

        public void AddPool<T>(Func<T> objectFactory, int initialSize = 0) where T : IPoolable
        {
            if (_poolDictionary.ContainsKey(typeof(T))) return;
            var pool = new ObjectPool<T>(objectFactory, initialSize);
            _poolDictionary.Add(typeof(T), pool);
        }
        
        public T Get<T>() where T : IPoolable
        {
            var elementType = typeof(T);

            if (_poolDictionary.TryGetValue(elementType, out var value) && value is ObjectPool<T> pool)
            {
                return pool.Get();
            }

            Debug.LogWarning($"Object pool for type {elementType} not found.");
            return default;
        }

        public void Return<T>(T element) where T : IPoolable
        {
            var elementType = typeof(T);
            
            if (_poolDictionary.TryGetValue(elementType, out var value))
            {
                var pool = value as ObjectPool<T>;
                pool?.Return(element);
            }
            else
            {
                Debug.LogWarning($"Object pool for type {elementType} not found.");
            }
        }
        
        public void Return(IPoolable element)
        {
            var elementType = element.GetType();

            if (_poolDictionary.TryGetValue(elementType, out var value))
            {
                var method = value.GetType().GetMethod("Return");
                method?.Invoke(value, new object[] { element });
            }
            else
            {
                Debug.LogWarning($"Object pool for type {elementType} not found.");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Libs.ObjectPool
{
    public sealed class ObjectPool<T> where T : IPoolable
    {
        private readonly Stack<T> _objects = new();
        private readonly Func<T> _objectFactory;

        public ObjectPool(Func<T> objectFactory, int initialSize = 0)
        {
            _objectFactory = objectFactory;

            Resize(initialSize);
        }

        private T CreateObject()
        {
            return _objectFactory();
        }

        public T Get()
        {
            if (_objects.Count == 0)
            {
                return CreateObject();
            }

            var obj = _objects.Pop();
            obj.Activate();
            return obj;
        }

        public void Return(T obj)
        {
            if (obj != null)
            {
                obj.Deactivate();
                _objects.Push(obj);
            }
        }

        private void Resize(int newSize)
        {
            if (newSize < 0)
            {
                Debug.LogWarning("New size cannot be negative.");
                return;
            }

            var diff = newSize - _objects.Count;
            if (diff <= 0) return;
            for (var i = 0; i < diff; i++)
            {
                _objects.Push(CreateObject()); 
            }
        }
    }



}
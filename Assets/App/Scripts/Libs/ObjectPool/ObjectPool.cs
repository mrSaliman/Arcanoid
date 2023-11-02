using System.Collections.Generic;

namespace App.Scripts.Libs.ObjectPool
{
    public sealed class ObjectPool<T> where T : IPoolable, new()
    {
        private readonly Stack<T> _objects = new();
    
        public T Get()
        {
            if (_objects.Count == 0) return new T();
            
            var obj = _objects.Pop();
            obj.Activate();
            return obj;
        }

        public void Return(T obj)
        {
            obj.Deactivate();
            _objects.Push(obj);
        }
    }
}
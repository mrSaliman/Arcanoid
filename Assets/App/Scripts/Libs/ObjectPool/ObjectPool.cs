using System.Collections.Generic;

namespace App.Scripts.Libs.ObjectPool
{
    public class ObjectPool<T> where T : new()
    {
        private readonly Stack<T> _objects = new Stack<T>();
    
        public T Get()
        {
            return _objects.Count == 0 ? new T() : _objects.Pop();
        }

        public void Return(T obj)
        {
            _objects.Push(obj);
        }
    }
}
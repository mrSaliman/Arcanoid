using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace App.Scripts.Libs.NodeArchitecture
{
    public abstract class ContextNode : MonoBehaviour
    {
        [SerializeField]
        private List<ContextNode> children;
        private ContextNode _parent;

        private readonly List<object> _instances = new();
        private readonly List<IContextUpdate> _updaters = new();
        private readonly List<IContextFixedUpdate> _fixedUpdaters = new();
        private readonly List<IContextLateUpdate> _lateUpdaters = new();

        public void Construct()
        {
            OnConstruct();

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var node = children[i];
                node._parent = this;
                node.Construct();
            }
        }

        public void Destruct()
        {
            for (int i = 0, count = children.Count; i < count; i++)
            {
                var node = children[i];
                node._parent = null;
                node.Destruct();
            }

            OnDestruct();
        }

        protected virtual void OnConstruct()
        {
        }

        protected virtual void OnDestruct()
        {
        }

        public void RegisterInstances(IEnumerable<object> instances)
        {
            foreach (var instance in instances)
            {
                RegisterInstance(instance);
            }
        }

        public void RegisterInstance(object service)
        {
            if (service == null)
            {
                return;
            }

            _instances.Add(service);

            if (service is IContextUpdate updater)
            {
                _updaters.Add(updater);
            }

            if (service is IContextFixedUpdate fixedUpdater)
            {
                _fixedUpdaters.Add(fixedUpdater);
            }

            if (service is IContextLateUpdate lateUpdater)
            {
                _lateUpdaters.Add(lateUpdater);
            }
        }

        public void SendEvent<T>() where T : ContextEvent
        {
            SendEventToInstances<T>();

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var node = children[i];
                node.SendEvent<T>();
            }
        }

        private void SendEventToInstances<T>() where T : ContextEvent
        {
            for (int i = 0, count = _instances.Count; i < count; i++)
            {
                var service = _instances[i];
                SendEventToInstance<T>(service);
            }
        }

        private void SendEventToInstance<T>(object service) where T : ContextEvent
        {
            var type = service.GetType();
            while (type != null && type != typeof(object) && type != typeof(MonoBehaviour))
            {
                var methods = type.GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly
                );

                for (int i = 0, count = methods.Length; i < count; i++)
                {
                    var method = methods[i];
                    if (method.GetCustomAttribute<T>() != null)
                    {
                        InvokeInstanceMethod(service, method);
                    }
                }

                type = type.BaseType;
            }
        }

        private void InvokeInstanceMethod(object service, MethodBase method)
        {
            var parameters = method.GetParameters();
            var count = parameters.Length;

            var args = new object[count];
            for (var i = 0; i < count; i++)
            {
                var parameter = parameters[i];
                args[i] = ResolveInstance(parameter.ParameterType);
            }

            method.Invoke(service, args);
        }

        public object ResolveInstance(Type type)
        {
            if (type == typeof(ContextNode))
            {
                return this;
            }

            var node = this;
            while (node != null)
            {
                if (node.FindInstance(type, out var service))
                {
                    return service;
                }

                node = node._parent;
            }

            throw new Exception($"Can't resolve instance {type.Name}!");
        }

        public T ResolveInstance<T>()
        {
            var node = this;
            while (node != null)
            {
                if (node.FindInstance<T>(out var service))
                {
                    return service;
                }

                node = node._parent;
            }

            throw new Exception($"Can't resolve instance {typeof(T).Name}!");
        }

        public T NewInstance<T>()
        {
            var objectType = typeof(T);
            var constructors = objectType.GetConstructors(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly
            );

            if (constructors.Length != 1)
            {
                throw new Exception($"Undefined constructor for type {objectType.Name}");
            }

            var constructor = constructors[0];
            var parameters = constructor.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0, count = parameters.Length; i < count; i++)
            {
                var parameter = parameters[i];
                args[i] = ResolveInstance(parameter.ParameterType);
            }

            return (T) constructor.Invoke(args);
        }

        public IEnumerable<T> ResolveInstances<T>()
        {
            var node = this;
            while (node != null)
            {
                if (node.FindInstance<T>(out var service))
                {
                    yield return service;
                }

                node = node._parent;
            }
        }

        private bool FindInstance<T>(out T service)
        {
            for (int i = 0, count = _instances.Count; i < count; i++)
            {
                var current = _instances[i];
                if (current is T tService)
                {
                    service = tService;
                    return true;
                }
            }

            service = default;
            return false;
        }

        private bool FindInstance(Type targetType, out object service)
        {
            for (int i = 0, count = _instances.Count; i < count; i++)
            {
                service = _instances[i];
                var serviceType = service.GetType();
                if (targetType.IsAssignableFrom(serviceType))
                {
                    return true;
                }
            }

            service = default;
            return false;
        }

        public T[] GetChildren<T>()
        {
            var result = new List<T>();

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var node = children[i];
                if (node is T tNode)
                {
                    result.Add(tNode);
                }
            }

            return result.ToArray();
        }

        public T GetChild<T>(Func<T, bool> predicate = null) where T : ContextNode
        {
            predicate ??= _ => true;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var node = children[i];
                if (node is not T tNode)
                {
                    continue;
                }

                if (predicate.Invoke(tNode))
                {
                    return tNode;
                }
            }

            throw new Exception($"Node of type {typeof(T).Name} is not found!");
        }

        public void AddChild(ContextNode node)
        {
            children.Add(node);
            node._parent = this;
            node.Construct();
        }

        public void RemoveChild(ContextNode node)
        {
            if (children.Remove(node))
            {
                node._parent = null;
            }
        }

        public virtual void OnUpdate()
        {
            for (int i = 0, count = _updaters.Count; i < count; i++)
            {
                var listener = _updaters[i];
                listener.OnUpdate();
            }

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];
                child.OnUpdate();
            }
        }

        public virtual void OnFixedUpdate()
        {
            for (int i = 0, count = _fixedUpdaters.Count; i < count; i++)
            {
                var listener = _fixedUpdaters[i];
                listener.OnFixedUpdate();
            }

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];
                child.OnFixedUpdate();
            }
        }

        public virtual void OnLateUpdate()
        {
            for (int i = 0, count = _lateUpdaters.Count; i < count; i++)
            {
                var listener = _lateUpdaters[i];
                listener.OnLateUpdate();
            }

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];
                child.OnLateUpdate();
            }
        }
    }
}
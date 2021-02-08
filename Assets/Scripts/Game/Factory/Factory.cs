namespace Base.Game.Factory
{
    using Base.Game.Signal;
    using System.Collections.Generic;
    using UnityEngine;

    public class Factory<T, T1> : IFactory<T> where T : MonoBehaviour
    {
        private Queue<T> _pool;
        private List<T> _prefabs;
        private Factory()
        {
            _pool = new Queue<T>();
            _prefabs = new List<T>();
        }
        ~Factory()
        {
            SignalBus<T1, T>.Instance.UnRegister(OnDestroyed);
        }
        private void SetHandle()
        {
            SignalBus<T1, T>.Instance.Register(OnDestroyed);
        }

        private void OnDestroyed(T obj)
        {
            if (_pool.Contains(obj))
                return;
            _pool.Enqueue(obj);
        }

        public T GetObject()
        {
            if (_pool.Count > 0)
                return _pool.Dequeue();
            return MonoBehaviour.Instantiate(_prefabs[UnityEngine.Random.Range(0, _prefabs.Count)].gameObject).GetComponent<T>();
        }

        public class Builder
        {
            private Factory<T,T1> _factory;

            public Builder()
            {
                _factory = new Factory<T,T1>();
            }

            public Builder SetPrefab(T prefab)
            {
                if (!_factory._prefabs.Contains(prefab))
                {
                    _factory._prefabs.Add(prefab);
                }
                return this;
            }

            public Builder SetPrefab(string prefabPath)
            {
                T prefab = Resources.Load<GameObject>(prefabPath).GetComponent<T>();
                if (!_factory._prefabs.Contains(prefab))
                {
                    _factory._prefabs.Add(prefab);
                }
                return this;
            }

            public Builder SetHandle()
            {
                _factory.SetHandle();
                return this;
            }

            public IFactory<T> Build()
            {
                return _factory;
            }

        }

    }
}
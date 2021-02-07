using UnityEngine;

namespace Core
{
    public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var go = new GameObject($"_{typeof(T).Name}");
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }
            else
            {
                _instance = (T)this;
            }
        }
    }
}
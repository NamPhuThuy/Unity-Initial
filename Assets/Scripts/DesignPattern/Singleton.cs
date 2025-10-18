using UnityEngine;

namespace NamPhuThuy
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();
        public static T Ins
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));
                        if (_instance == null)
                        {
                            var singletonObj = new GameObject(typeof(T).Name);
                            _instance = singletonObj.AddComponent<T>();
                        }
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                // Uncomment if you want the singleton to persist across scenes
                // DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
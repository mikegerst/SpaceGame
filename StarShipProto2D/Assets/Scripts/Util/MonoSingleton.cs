using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                    Debug.Log($"{typeof(T).ToString()} is NULL.");
                return _instance;
            }
        }

        private void Awake()
        {
            if(_instance==null)
                _instance = this as T;
            else
                Destroy(this.gameObject);
        }
    }
}


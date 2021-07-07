using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibrary
{
    
    public class ScriptableVariable<T> : ScriptableObject
    {
        public T Data;
    }

    public abstract class SerializedScriptableVariable<T> : ScriptableObject
    {
        public abstract T Data { get; }
    }

    public class NonSerializedScriptableVariable<T> : ScriptableObject
    {
        [NonSerialized,ShowInInspector, HideInEditorMode, DisableInPlayMode]
        private T _data;

        public T Data => _data;

        public void Injection(T data)
        {
            _data = data;
        }
    }

    public abstract class ScriptableReferencer<T> : ScriptableObject
    {
        [ShowInInspector,DisableInEditorMode,DisableInPlayMode]
        public T Reference { get; private set; }

        public virtual void InjectReference(T set)
        {
            if(set.Equals(this)) 
                throw new ArgumentException("The injected object can't be the same object; This causes an internal loop of self Reference",
                    new StackOverflowException("Same object injected"));
            Reference = set;
        }
    }
    
}

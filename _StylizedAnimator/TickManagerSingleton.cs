using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StylizedAnimator
{
    // It uses Singleton because there's only one manager of this type per Game
    public sealed class TickManagerSingleton 
    {
        static TickManagerSingleton() { }

        private TickManagerSingleton()
        {
            TickManager = new StylizedTickManager();
        }
        public static TickManagerSingleton Instance { get; } = new TickManagerSingleton();

        public static StylizedTickManager TickManager;
    }

}

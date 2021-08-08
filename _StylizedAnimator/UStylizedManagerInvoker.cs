using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StylizedAnimator
{
    public class UStylizedManagerInvoker : MonoBehaviour, IStylizedManagerHolder
    {
        [SerializeField]
        private TickManagerParameters _parameters
            = new TickManagerParameters();
        [NonSerialized,ShowInInspector]
        private StylizedTickManager _tickerManager;


        private void Awake()
        {
            _tickerManager = new StylizedTickManager(_parameters,this);
            TickManagerSingleton.TickManager = _tickerManager;

        }

        private void Start()
        {
            _tickerManager.StartTicks();
        }


        private void OnEnable()
        {
            _tickerManager?.OnEnable();
        }

        private void OnDisable()
        {
            _tickerManager?.OnDisable();
        }

        public StylizedTickManager GetManager()
        {
            return _tickerManager;
        }

    }


}

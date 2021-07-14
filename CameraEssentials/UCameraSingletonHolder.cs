using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibrary
{
    public class UCameraSingletonHolder : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        [Button]
        private void ChangeCameraTest(Camera newCamera)
        {
            CameraSingleton.InvokeUpdateCamera(newCamera);
        }

        private void Start()
        {
            CameraSingleton.FirstInvoke(mainCamera);
        }
    }

    public sealed class CameraSingleton
    {
        static CameraSingleton() { }
        public static CameraSingleton Instance { get; } = new CameraSingleton();

        private CameraSingleton()
        {
            _onCameraChange = new List<ICameraSingletonListener>();
            _onFirstCall = new Queue<ICameraInitiationListener>();
        }

        private static Camera _mainCamera;
        public static Camera MainCamera
        {
            get => _mainCamera;
            set => InvokeUpdateCamera(value);
        }

        private static List<ICameraSingletonListener> _onCameraChange;
        private static Queue<ICameraInitiationListener> _onFirstCall;

        public static void InvokeUpdateCamera(Camera newCamera)
        {
            _mainCamera = newCamera;
            foreach (ICameraSingletonListener listener in _onCameraChange)
            {
                listener.OnCameraChange(_mainCamera);
            }
        }

        public static void FirstInvoke(Camera mainCamera)
        {
            _mainCamera = mainCamera;
            while (_onFirstCall.Count > 0)
            {
                _onFirstCall.Dequeue().OnCameraInitiation(_mainCamera);
            }
            // Keep the Queue alive for OnSceneChange
        }

        public static void Subscribe(ICameraInitiationListener listener)
        {
            if(_mainCamera == null)
                _onFirstCall.Enqueue(listener);
            else
            {
                listener.OnCameraInitiation(_mainCamera);
            }

            if (listener is ICameraSingletonListener singletonListener)
                _onCameraChange.Add(singletonListener);
        }

        public static void UnSubscribe(ICameraSingletonListener listener)
        {
            _onCameraChange.Remove(listener);
        }
    }

    public interface ICameraSingletonListener : ICameraInitiationListener
    {
        void OnCameraChange(Camera injection);
    }
    /// <summary>
    /// Call the first time by <seealso cref="UCameraSingletonHolder"/>
    /// </summary>
    public interface ICameraInitiationListener
    {
        void OnCameraInitiation(Camera injection);

    }
}

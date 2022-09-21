using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils_Project.Scene
{
    public sealed class ULoadSceneManager : MonoBehaviour, ILoadSceneManagerStructureRead<ILoadSceneAnimator>
    {
        [SerializeField]
        private MainSceneLoadTransitionWrapper mainLoadTransitionWrapper = new MainSceneLoadTransitionWrapper();
        [SerializeField]
        private CombatLoadTransitionWrapper combatLoadTransitionWrapper = new CombatLoadTransitionWrapper();

        public ILoadSceneAnimator MainLoadType => mainLoadTransitionWrapper;
        public ILoadSceneAnimator CombatLoadType => combatLoadTransitionWrapper;

        [Button]
        private void TryMainLoadAnimation(float animationTime = 1) => TryAnimation(MainLoadType, animationTime);

        [Button]
        private void TryCombatLoadAnimation(float animationTime = 1) => TryAnimation(CombatLoadType, animationTime);

        private void TryAnimation(ILoadSceneAnimator animator, float animationTime = 1)
        {
            _transitionHandle =
                Timing.RunCoroutine(_DoTransition(animationTime, LoadCallBacks.NullCallBacks, animator, 0));
        }

        private void Awake()
        {
            LoadSceneManagerSingleton.Injection(this);
            mainLoadTransitionWrapper.Awake();
            combatLoadTransitionWrapper.Awake();
        }


        private CoroutineHandle _transitionHandle;

        private void HandleViolations(string sceneName)
        {
            if (_transitionHandle.IsRunning)
                throw new AccessViolationException($"Trying to load another scene while Loading is Active - Map: " +
                                                   $"{sceneName}");
        }

        public void LoadScene(
            LoadSceneParameters loadParameters,
            LoadCallBacks loadCallBacks,
            LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            var sceneName = loadParameters.SceneName;
            HandleViolations(sceneName);

            var animator = HandleAndGetAnimator(loadParameters.Type);
            _transitionHandle =
                Timing.RunCoroutine(_DoLoadScene(sceneName, loadCallBacks, animator, loadMode, loadParameters.OnLoadDelay));

        }

        public void JustTransition(
            LoadSceneParameters.LoadType type, float waitUntilSeconds, LoadCallBacks loadCallBacks, float afterLoadDelay)
        {
            HandleViolations(null);

            var animator = HandleAndGetAnimator(type);
            _transitionHandle =
                Timing.RunCoroutine(_DoTransition(waitUntilSeconds, loadCallBacks, animator, afterLoadDelay));
        }



        private ILoadSceneAnimator HandleAndGetAnimator(LoadSceneParameters.LoadType type)
        {
            switch (type)
            {
                case LoadSceneParameters.LoadType.CombatLoad:
                    return combatLoadTransitionWrapper;
                default:
                    mainLoadTransitionWrapper.InitializeState(true);
                    return mainLoadTransitionWrapper;
                case LoadSceneParameters.LoadType.MainLoadFromRight:
                    mainLoadTransitionWrapper.InitializeState(false);
                    return mainLoadTransitionWrapper;
            }
        }

        private static IEnumerator<float> _DoLoadScene(
            string targetScene,
            LoadCallBacks callBacks,
            ILoadSceneAnimator animator,
            LoadSceneMode loadMode = LoadSceneMode.Single,
            float afterLoadDelay = 0)
        {
            animator.SetActive(true);
            yield return Timing.WaitUntilDone(_InitialAnimation(callBacks,animator));

            var loadOperation = SceneManager.LoadSceneAsync(targetScene, loadMode);
            do
            {
                yield return Timing.WaitForOneFrame;
                animator.TickingLoad(loadOperation.progress);
            } while (!loadOperation.isDone);

            yield return Timing.WaitUntilDone(_FinalAnimation(callBacks,animator, afterLoadDelay));
            animator.SetActive(false);
        }

        private static IEnumerator<float> _DoTransition(
            float finishAfterSeconds,
            LoadCallBacks callBacks,
            ILoadSceneAnimator animator,
            float afterLoadDelay = 0)
        {
            animator.SetActive(true);
            yield return Timing.WaitUntilDone(_InitialAnimation(callBacks,animator));

            if (finishAfterSeconds < 0) finishAfterSeconds = .1f;
            float currentPercentage;
            float timer = 0;
            do
            {
                yield return Timing.WaitForOneFrame;
                timer += Timing.DeltaTime;
                currentPercentage = timer / finishAfterSeconds;
                if (currentPercentage > 1) currentPercentage = 1;
                animator.TickingLoad(currentPercentage);

            } while (currentPercentage < 1);

            yield return Timing.WaitUntilDone(_FinalAnimation(callBacks,animator,afterLoadDelay));
            animator.SetActive(false);
        }



        public static IEnumerator<float> _InitialAnimation(LoadCallBacks callbacks, ILoadSceneAnimator animator)
        {
            callbacks.OnStartTransition?.Invoke();
            yield return Timing.WaitUntilDone(animator._DoInitialAnimation());
            callbacks.OnStartLoading?.Invoke();
        }

        public static IEnumerator<float> _FinalAnimation(LoadCallBacks callbacks,ILoadSceneAnimator animator, float delayAfterLoad)
        {
            callbacks.OnLoadingFinish?.Invoke();
            yield return Timing.WaitForSeconds(delayAfterLoad);
            yield return Timing.WaitUntilDone(animator._OnAfterLoadAnimation());
            callbacks.OnFinishTransition?.Invoke();
        }
    }

}

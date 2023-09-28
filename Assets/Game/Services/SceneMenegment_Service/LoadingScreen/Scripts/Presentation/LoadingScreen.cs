using System;
using System.Collections;
using UnityEngine;

namespace WKosArch.Services.Scenes
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private GameObject _goContent;
        [SerializeField] private float _delayTime;

        public void Show(Action onComplete = null)
        {
            _goContent.SetActive(true);
            onComplete?.Invoke();
        }

        public void Hide(Action onComplete = null)
        {
            StartCoroutine(CloseLoadingScreenWithDelay(_delayTime));
            onComplete?.Invoke();
        }

        private IEnumerator CloseLoadingScreenWithDelay(float delay)
        {

            yield return new WaitForSeconds(delay);

            _goContent.SetActive(false);
        }
    }
}
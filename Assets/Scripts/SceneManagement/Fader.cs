using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    public class Fader : MonoBehaviour {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate() {
            canvasGroup.alpha = 1;
        }
        
        // go from alpha 0 to 1
        public Coroutine FadeOut(float time) {
            return Fade(1, time);
        }

        // go from alpha 1 to 0
        public Coroutine FadeIn(float time) {
            return Fade(0, time);
        }

        public Coroutine Fade(float target, float time) {
            // cancel any running coroutines
            if (currentActiveFade != null) {
                StopCoroutine(currentActiveFade);
            }
            // run fadeout coroutine
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentActiveFade;
        }

        // check if alpha is NOT approximately close to target and move toward target
        private IEnumerator FadeRoutine(float target, float time) {
            while (!Mathf.Approximately(canvasGroup.alpha, target)) {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            } 
        }

        
    }
}
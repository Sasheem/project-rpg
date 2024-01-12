using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    public class Fader : MonoBehaviour {
        CanvasGroup canvasGroup;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate() {
            canvasGroup.alpha = 1;
        }
        
        // go from alpha 0 to 1
        public IEnumerator FadeOut(float time) {
            // go over a few frames, every frame update alpha by a certain amount
            // so after the time it will arrive at value 1
            // while alpha is not 1 
            while (canvasGroup.alpha < 1) { 
                canvasGroup.alpha += Time.deltaTime / time;
                // wait by using yield
                // tells unity this coroutine needs to run again on the next frame
                yield return null;
            } 
        }

        // go from alpha 1 to 0
        public IEnumerator FadeIn(float time) {
            // while alpha is not 0 
            while (canvasGroup.alpha > 0) { 
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            } 
        }
    }
}
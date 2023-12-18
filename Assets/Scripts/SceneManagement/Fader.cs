using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    public class Fader : MonoBehaviour {
        CanvasGroup canvasGroup;

        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();

            StartCoroutine(FadeOutIn());
        }

        IEnumerator FadeOutIn() {
            yield return FadeOut(3f);
            print("Faded out");
            yield return FadeIn(1f);
            print("Faded in");
        }
        
        // go from alpha 0 to 1
        IEnumerator FadeOut(float time) {
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
        IEnumerator FadeIn(float time) {
            // while alpha is not 0 
            while (canvasGroup.alpha > 0) { 
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            } 
        }
    }
}
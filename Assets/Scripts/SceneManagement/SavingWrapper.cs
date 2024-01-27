using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement {

	public class SavingWrapper : MonoBehaviour {
		
		const string defaultSaveFile = "save";

		[SerializeField] float fadeInTime = 0.2f;

		private void Awake() {
			StartCoroutine(LoadLastScene());
		}

		public IEnumerator LoadLastScene() {
			Fader fader = FindObjectOfType<Fader>();
			// Fade out completely
			fader.FadeOutImmediate();
			yield return GetComponent<JsonSavingSystem>().LoadLastScene(defaultSaveFile);
			// fade in
			yield return fader.FadeIn(fadeInTime);
		}
		
		private void Update() {
			if (Input.GetKeyDown(KeyCode.S)) {
				Save();
			}
			if (Input.GetKeyDown(KeyCode.L)) {
				Load();
			}
			if (Input.GetKeyDown(KeyCode.D)) {
				Delete();
			}
		}
 		public void Save() {
			GetComponent<JsonSavingSystem>().Save(defaultSaveFile);
		}
		
		public void Load() {
			GetComponent<JsonSavingSystem>().Load(defaultSaveFile);
		}

		public void Delete() {
			GetComponent<JsonSavingSystem>().Delete(defaultSaveFile);
		}
	}
}
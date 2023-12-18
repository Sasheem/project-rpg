using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        enum DestinationIdentifier {
            A, B, C, D
        }
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;
        

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            // error checking that sceneToLoad is set
            if (sceneToLoad < 0) {
                Debug.LogError("Scene to loan not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);;
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            // Possible rotation solution
            // Vector3 directionVector = (otherPortal.spawnPoint.position - otherPortal.transform.position).normalized;
            // player.transform.rotation = Quaternion.LookRotation(directionVector);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>()) {
                // ignore portal if it isn't the current one belonging to this script at the time
                if (portal == this) continue;
                // ignore portal if it has wrong destination
                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }
    }   
}

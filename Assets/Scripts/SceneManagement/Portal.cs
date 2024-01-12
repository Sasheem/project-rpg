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

            // Dont get rid of portal until new world has loaded up
            DontDestroyOnLoad(gameObject);
            
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            // begin fading out over time
            yield return fader.FadeOut(fadeOutTime);

            // save current level
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // load current level
            savingWrapper.Load();
            
            // finding corresponding portal
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            // waiting for some time so camera can stabilize
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            UpdatePlayer(otherPortal);

            // destroy this portal
            Destroy(gameObject);
        }

        /*
            Possible error located here.
            Does this run again after a player exits a portal into a new scene?
        */
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            // player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            // player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            // player.GetComponent<NavMeshAgent>().enabled = true;
            

            // Possible rotation solution - does not work anymore
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

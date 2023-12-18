using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics {
    public class CinematicControlRemover : MonoBehaviour {
        GameObject player;
        private void Start() {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }
        // two cases
        // disable control
        void DisableControl(PlayableDirector pd) {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        // enable control
        void EnableControl(PlayableDirector pd) {
            print("EnableControl");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
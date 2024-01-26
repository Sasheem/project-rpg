using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats {
    public class ExperienceDisplay : MonoBehaviour {
        Experience experience;

        private void Awake() {
            // cache experience points
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update() {
            GetComponent<Text>().text = string.Format("{0:0}", experience.GetPoints());
        }
    }
}

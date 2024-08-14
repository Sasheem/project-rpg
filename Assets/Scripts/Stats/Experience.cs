using UnityEngine;
using GameDevTV.Saving;
using System;
using Newtonsoft.Json.Linq;

namespace RPG.Stats {
    public class Experience : MonoBehaviour, IJsonSaveable {
        [SerializeField] float experiencePoints = 0;

        // functions that return void and take no arguements
        public event Action onExperienceGained;

        // Gain the appropriate experience and alert subscribers 
        // of the event onExperienceGained
        public void GainExperience(float experience) {
            experiencePoints += experience;
            onExperienceGained();
        }

        public float GetPoints()
        {
            return experiencePoints;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(experiencePoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            experiencePoints = state.ToObject<float>();
        }
    }
}

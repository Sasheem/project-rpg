using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat {
    public class Weapon : MonoBehaviour {
        [SerializeField] UnityEvent onHit;

        // Call to to Fighter.cs to trigger unity events
        public void OnHit() {
            onHit.Invoke();
        }
    }
}
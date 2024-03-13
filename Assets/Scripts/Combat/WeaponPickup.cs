using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float respawnTime = 5f;
        
        // Equip this weapon if player enters collider
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        // Respawn weapon pickup after x seconds
        private IEnumerator HideForSeconds(float seconds) {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        // Show or hide weapon pickup game objects
        private void ShowPickup(bool shouldShow) {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform) {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0)) {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}

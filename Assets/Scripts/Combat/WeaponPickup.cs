using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;
        
        // Equip this weapon if player enters collider
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player") {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
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
    }
}

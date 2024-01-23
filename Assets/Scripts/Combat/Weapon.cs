using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 20f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";


        // instantiate the weapon
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab != null) {
                Transform handTransform = GetTransform(rightHand, leftHand);

                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }

            // REWATCH lecture for this
            // overrideController will be null if this is just the root character AnimatorController
            // otherwise it will have the value of the AnimatorOverrideController that is in the animator.runtimeAnimatorController
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null) {
                animator.runtimeAnimatorController = animatorOverride;
            
            } else if (overrideController != null) {
                // If it is already an override then find its parent and put that as the runtimeANimatorController instead
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;                
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            // more efficent way of doing the same thing
            Transform oldWeapon = rightHand.Find(weaponName) ?? leftHand.Find(weaponName);
            if (oldWeapon == null) return;
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile() {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target) {
            // remember params for Instantiate are
            // 1 - what, 2 - where, and 3 - rotation
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }

        public float GetDamage() {
            return weaponDamage;
        }

        public float GetRange() {
            return weaponRange;
        }
    }
}
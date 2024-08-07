using RPG.Attributes;
using UnityEngine;
using GameDevTV.Inventories;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";


        // instantiate the weapon
        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator) {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;
            if (equippedPrefab != null) {
                Transform handTransform = GetTransform(rightHand, leftHand);

                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
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
            return weapon;
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage) {
            // remember params for Instantiate are
            // 1 - what, 2 - where, and 3 - rotation
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetDamage() {
            return weaponDamage;
        }

        public float GetPercentageBonus() {
            return percentageBonus;
        }

        public float GetRange() {
            return weaponRange;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat) {
            if (stat == stat.Damage) {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return percentageBonus;
            }
        }
    }
}
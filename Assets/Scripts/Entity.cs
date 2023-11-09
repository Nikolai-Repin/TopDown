using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] protected bool invulnerable;
    [SerializeField] protected bool intangible;
    [SerializeField] public float healthAmount;
    private RoomInfo room;

    //Deals damage to entity if invulnerable, returns true if damage was dealt
    public virtual bool TakeDamage(float damage) {
        if (intangible) {
            return false;
        }

        if (!invulnerable) {
            healthAmount -= damage;
            if (healthAmount <= 0) {
                Die();
            }
        }
        return true;
    }

    protected void FireInRings(GameObject projectile, int projectileCount, float rotationAmount, float rotationOffset) {
        //Outer for loop controls how many rings of projectiles
        for (int k = 1; k <= 2; k++) {
            //Inner for loop controls how many projectiles in each ring
            for (int i = 0; i < projectileCount; i++) {
                GameObject bullet = Instantiate(projectile, transform.position, new Quaternion());
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.team = "Enemy";
                Quaternion fireAngle = Quaternion.Euler(new Vector3(0, 0, (rotationAmount*i)+rotationOffset));
                bulletScript.LaunchProjectile(fireAngle, 10/k);
            }
            rotationOffset += rotationAmount/2;
        }
    }

    //Finds the closest game object from a array of collider2D and their distance from Vector3 origin
    public GameObject FindClosest (Collider2D[] targets, Vector3 origin) {
        if (targets.Length > 0) {
            GameObject closest = targets[0].transform.gameObject;
            float closestLen = (targets[0].transform.position - origin).sqrMagnitude;
            float curLen = closestLen;

            for (int i = 1; i < targets.Length; i++) {
                curLen = (targets[i].transform.position - origin).sqrMagnitude;
                if (curLen < closestLen) {
                    closestLen = curLen;
                    closest = targets[i].transform.gameObject;
                }
            }

            return closest;
        }
        return null;
    }

    //Finds the closest game object from a list of collider2D and their distance from Vector3 origin
    public GameObject FindClosest (List<Collider2D> targets, Vector3 origin) {
        return FindClosest(targets.ToArray(), origin);
    }

    //Destroys the entity
    public virtual void Die () {
        if (room != null) {room.RemoveEntity(this);}
        Destroy(transform.gameObject);
    }

    //Returns 0, mainly exists to be overridden in PlayerController so that weapons don't break
    public virtual float GetDamage() {
        return 0;
    }

    //Returns 0, mainly exists to be overridden in PlayerController so that weapons don't break
    public virtual float GetManaRechargeSpeed() {
        return 0;
    }

    public void SetRoom(RoomInfo r) {room = r;}
    
}

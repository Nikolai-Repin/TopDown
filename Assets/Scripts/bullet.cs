using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    [SerializeField] private float maxLife; //How long a bullet should exist for, in seconds, I think.
    [SerializeField] private float damage; // How much damage a bullet should do
    [SerializeField] private float projectileSpeed; //How fast bullet move
    [SerializeField] private int pierce; //How many entities it should inte
    [SerializeField] private bool reflectable; //Should it be flectable by melee weapons

    public GameObject creator; //Who created this bullet
    private float lifeTime = 0f; //How long the bullet has existed for

    //Defines how the bullet should move when the bullet is first fired
    public virtual void LaunchProjectile(Quaternion rotation) {
        SetProjectileVelocity(rotation, projectileSpeed);
    }

    //Sets bullet velocity based on rotation, using bullet speed
    public virtual void SetProjectileVelocity(Quaternion rotation) {
        SetProjectileVelocity(rotation, projectileSpeed);
    }

    //Sets bullet velocity based on rotation and how fast it should move
    public virtual void SetProjectileVelocity(Quaternion rotation, float strength) {
        Vector2 newVelocity = new Vector2();
        newVelocity.x = strength * Mathf.Cos(rotation.eulerAngles.z * Mathf.Deg2Rad);
        newVelocity.y = strength * Mathf.Sin(rotation.eulerAngles.z * Mathf.Deg2Rad);
        GetComponent<Rigidbody2D>().velocity = newVelocity;
    }

    void Start() {
        damage = Controller.GetDamage();
    }

    void Update() {
        lifeTime += Time.deltaTime; //Update bullet lifetime
        //This being called every frame could be laggy, it's likely that there's a better way to do this
        //Kill bullet if it's too old
        if (lifeTime >= maxLife) {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            Health health = other.gameObject.GetComponent<Health>();
            health.TakeDamage(damage);
            pierce--;
        }

        if (other.gameObject.tag == "Wall") { //Hardcoding because I don't have the time today to set up a way to handle what bullets should interact with, maybe check if they have the same parent?
            pierce--;

        }

        if (pierce <= 0) {
            Destroy(gameObject);
        }

        if (other.GetComponent<Bullet>() == null) { //Hardcoding because I don't have the time today to set up a way to handle what bullets should interact with, maybe check if they have the same parent?
            Destroy(gameObject);
        }
    }
    public float getProjectileSpeed() {
        return projectileSpeed;
    }

    public bool getReflectable() {
        return reflectable;
    }
}
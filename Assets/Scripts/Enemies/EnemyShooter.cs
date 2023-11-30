using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyShooter : Enemy
{
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected int clipSize;
    [SerializeField] protected float reloadTime;
    [SerializeField] private int ammo;
    [SerializeField] private bool moveWhileShooting;
    private float reloadLastTime;
    protected GameObject target;
    public enum Phase
    {
        Aiming = 1,
        Firing = 2,
    }
    private Phase curPhase;

    void Start() {
        base.Start();
        curPhase = Phase.Aiming;
        reloadLastTime = Time.time;
        ammo = clipSize;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (target != null) {
            if (ammo > 0 && Time.time > reloadLastTime && (target.transform.position - transform.position).sqrMagnitude <= 2500) {
                if (weapon.GetComponent<Weapon>().Fire()) {
                    ammo--;
                    curPhase = Phase.Firing;
                    if (!moveWhileShooting) {
                        GetComponent<AIBase>().canMove = false;
                    }
                }                
            } else if (ammo == 0) {
                GetComponent<AIBase>().canMove = true;
                curPhase = Phase.Aiming;
                ammo = clipSize;
                reloadLastTime = Time.time + Random.Range(reloadTime, reloadTime+(reloadTime/3));
            }

            if (curPhase != Phase.Firing || moveWhileShooting) {
                weapon.GetComponent<Weapon>().SetTarget(target.transform.position);
            }
        } else {
            target = FindClosestPlayer(visRange);
            ammo = 0;
        }
    }
}
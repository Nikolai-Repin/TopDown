using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TrackerController : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float offset;
    [SerializeField] private LayerMask walls;
    [SerializeField] private LayerMask enemies;
    [System.Flags]
    public enum AI
    {
        Melee = 1,
        Range = 2,
    }
    [SerializeField] AI ai = AI.Melee;

    [Space, Header("Pathfinder Settings")]
    [SerializeField] public float endReachedDistanceMelee;
    [SerializeField] public float endReachedDistanceRange;

    [SerializeField] public Transform target;
    public AIPath aiPath;

    private void Start()
    {
        
        transform.parent.GetComponent<Enemy>().trackerController = this;
        aiPath = transform.parent.GetComponent<AIPath>();
        switch (ai) {
            case AI.Melee: 
            {
                aiPath.endReachedDistance = endReachedDistanceMelee;
                break;
            }

            case AI.Range: 
            {
                aiPath.endReachedDistance = endReachedDistanceRange;
                break;
            }
        }
    }

    private void Update()
    {
        if (ai == AI.Melee && target != null) 
        {
            transform.position = target.transform.position;
        }
        else if (ai == AI.Range && target != null) 
        {
            transform.position = target.transform.position;
            var dir = transform.parent.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.forward);
            transform.position += dir.normalized * offset;

            // Tests for walls in between the enemy and the player
            RaycastHit2D hit = Physics2D.Raycast(target.position, dir.normalized, offset, walls.value);
            if (hit.collider != null)
            {
                
                hit = Physics2D.Raycast(target.position, dir.normalized, offset, enemies.value | walls.value);
                if (hit.transform.tag == "Enemy")
                {
                    Debug.DrawLine(target.position, transform.position, Color.blue);
                }
                else
                {
                    Debug.DrawLine(target.position, transform.position, Color.red);
                    transform.position = target.transform.position;
                }
                
            }
            else
            {
                Debug.DrawLine(target.position, transform.position, Color.green);
            }
        }
    }

    public void SetTarget(Transform newTarget) 
    {
        target = newTarget;
    }

    public void SetAI(AI newAI) 
    {
        ai = newAI;
        if (ai == AI.Melee)
        {
            aiPath.endReachedDistance = endReachedDistanceMelee;
        }
        else if (ai == AI.Range)
        {
            aiPath.endReachedDistance = endReachedDistanceRange;
        }
    }

    public LayerMask GetWalls() {return walls;}
}

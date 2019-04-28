using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerController))]
public class EnemyController : MonoBehaviour
{

    public enum EnemyState
    {
        PATROL,
        HUNT,
        ATTACK,
        RETREAT
    };

    //private Rigidbody rb;

    //handlers
    public float walkSpeed;
    private bool hasTarget;
    public float detectRadius = 10f;
    public float maxLookDistance = 10f;
    public int enemyDamage = 10;
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;

    private float waitTime;
    public float startWaitTime;

    public Transform[] moveSpots;
    private int randomSpot;
    private Transform currentPoint;


    //components
    public GameObject thePlayer;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        //rb = GetComponent<Rigidbody>();
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        randomSpot = Random.Range(0, moveSpots.Length);
        GetPathPoints();
    }

    // Update is called once per frame
    void Update()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
        attackCooldown -= Time.deltaTime;

        //get distance between player and enemy
        float distance = Vector3.Distance(thePlayer.transform.position, transform.position);

        //if distance <= lookRadius, move to Target
        if (distance <= detectRadius)
        {
            agent.SetDestination(thePlayer.transform.position);

            if (distance <= agent.stoppingDistance)
            {
                //face target and attack
                FaceTarget();
                Attack();
            }
        } else
        {
            Patrol();
        }
    }

    void Attack()
    {
        if (attackCooldown <= 0f)
        {
            thePlayer.GetComponent<PlayerStats>().TakeDamage(enemyDamage);
            attackCooldown = 1f / attackSpeed;
        }
        Debug.Log("Enemy attacked and dealt" + enemyDamage + " damage");
    }


    //function moves the enemy to the player target.
    void MoveToTarget()
    {
        float distance = Vector3.Distance(thePlayer.transform.position, transform.position);

        if (distance <= detectRadius)
        {
            agent.SetDestination(thePlayer.transform.position);

            if (distance <= agent.stoppingDistance)
            {
                //attack target
                FaceTarget();
            }
        }
    }


    //function makes the enemy face the player target.
    void FaceTarget()
    {
        Vector3 direction = (thePlayer.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }


    //WIP.
    //Function is designed to have enemies patrol if they have no current player target.
    void Patrol()
    {
        agent.SetDestination(moveSpots[randomSpot].position);
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 1f) {
            randomSpot = Random.Range(0, moveSpots.Length);
        }
    }

    //might do better later.
    private void GetPathPoints()
    {
        GameObject[] pathPoints = GameObject.FindGameObjectsWithTag("PathPoint");
        moveSpots = new Transform[pathPoints.Length];

        for (int i = 0; i < moveSpots.Length; i++)
        {
            Transform s = pathPoints[i].transform;
            moveSpots[i] = s;
        }
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }


}

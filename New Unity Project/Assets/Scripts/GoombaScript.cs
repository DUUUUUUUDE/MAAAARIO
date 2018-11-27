using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaScript : MonoBehaviour , IRestartGameElement
{

	enum state {none, patrol, chase};
    state GoombaState;
    NavMeshAgent agent;
    Transform playerTrans;

    Vector3 patrolPoint;

    public float stopChasingDist;
    public float startChasingDist;

    public List<Transform> PatrolPos;

    Vector3 StartPos;

    public void RestartGame()
    {
        transform.position = StartPos;

        patrolPoint = PatrolPos[0].position;
        ChangeState(state.patrol);

        gameObject.SetActive(true);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTrans = PlayerManager.instace._Movement.transform;

        patrolPoint = PatrolPos[0].position;
        ChangeState(state.patrol);

        StartPos = transform.position;


        GameManager.instance.AddRestartGameElement(this);
    }


    private void Update()
    {
        switch (GoombaState)
        {
            case state.patrol:
                PatrolState();
                break;
            case state.chase:
                ChaseState();
                break;
        }
    }


    void ChangeState (state newState)
    {
        if (newState == GoombaState) { return; }

        if (newState == state.patrol)
        {
            agent.SetDestination(patrolPoint);
            PatrolState();
        }
        else
        {
            ChaseState();
        }

        GoombaState = newState;

    }


    void PatrolState ()
    {
        if(Vector3.Distance(transform.position, playerTrans.position) < startChasingDist)
        {
            ChangeState(state.chase);
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {

            Vector3 newPos = PatrolPos [Random.Range (0, PatrolPos.Count - 1)].position;
            while (newPos == patrolPoint)
            {
                newPos = PatrolPos[Random.Range(0, PatrolPos.Count)].position;
            }

            patrolPoint = newPos;
            agent.SetDestination(patrolPoint);
        }

    }


    void ChaseState()
    {
        agent.SetDestination(playerTrans.position);

        if (Vector3.Distance(transform.position, playerTrans.position) > stopChasingDist)
        {
            ChangeState(state.patrol);
            return;
        }
        if (agent.pathStatus == NavMeshPathStatus.PathPartial && agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            ChangeState(state.patrol);
            return;
        }

    }

    bool dead;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if ((playerTrans.position.y - transform.position.y) > 1.5f)
            {
                PlayerManager.instace._Movement._Velocity.y = 10;
                dead = true;
                gameObject.SetActive (false);
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !dead)
            PlayerManager.instace.Damage(1,transform.position);
    }

}

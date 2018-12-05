using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IRestartGameElement
{

    public static PlayerManager instace;
    public PlayerMovement _Movement;
    public PlayerUI _UI;

    public Animator animator;

    public int MaxLifes;
    public int Lifes;

    public float MaxHP;
    public float HP;
    public float Coins;

    public Transform SpawnPoint;


    public void RestartGame()
    {
        _Movement.transform.position = SpawnPoint.position;
        _Movement.transform.forward = SpawnPoint.forward;
        HP = MaxHP;

        _Movement._Velocity = Vector3.zero;

        _Movement.gameObject.SetActive(true);
    }

    private void Awake()
    {
        if (instace == null)
            instace = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
            
    }

    private void Start()
    {
        _Movement = GetComponentInChildren<PlayerMovement>();
        _UI = GetComponent<PlayerUI>();

        animator = GetComponentInChildren<Animator>();

        HP = MaxHP;

        GameManager.instance.AddRestartGameElement(this);

        _UI.RefreshUI();

        ChangeState(States.Idle);
    }

    public enum States {Idle, Walk, Run, Fall, Jump, Jump_Double, Jump_Triple, Jump_Long, Jump_Wall, Punch1, Punch2, Punch3, Hit, Die};
    public static States State;

    public void ChangeState(States newState)
    {
        State = newState;

        animator.SetFloat ("Speed",_Movement._Velocity.magnitude / _Movement._RunMoveSpeed);

        switch (State)
        {
            case States.Jump:
                animator.SetTrigger("Jump1");
                break;
            case States.Jump_Double:
                animator.SetTrigger("Jump2");
                break;
            case States.Jump_Triple:
                animator.SetTrigger("Jump3");
                break;
            case States.Jump_Long:
                animator.SetTrigger("JumpLong");
                break;
            case States.Punch1:
                animator.SetTrigger("Punch1");
                break;
            case States.Punch2:
                animator.SetTrigger("Punch2");
                break;
            case States.Punch3:
                animator.SetTrigger("Punch3");
                break;
            case States.Hit:
                animator.SetTrigger("Damage");
                break;
            case States.Die:
                animator.SetTrigger("Death");
                break;
        }

    }

        #region DAMAGE

        public void Damage (float dmg , Vector3 DamagePos)
    {
        if (!invulnerable)
        {
            ChangeState(States.Hit);


            HP -= dmg;

            if (HP <= 0)
            {
                ChangeState(States.Die);
                Lifes -= 1;
                GameOver();
                if (Lifes == 0)
                    Application.Quit();

            }


            Vector3 DamageVector = (_Movement.transform.position - DamagePos);
            DamageVector.y = 0;
            DamageVector.Normalize();
            DamageVector.y = 0.5f;

            DamageVector *= 15;

            _Movement._Velocity = DamageVector;


            StartCoroutine(InvulnerabilityCO());

            _UI.RefreshHealth();
        }
    }

    bool invulnerable;
    IEnumerator InvulnerabilityCO ()
    {
        invulnerable = true;
        yield return new WaitForSeconds(1);
        invulnerable = false;
    }

    #endregion

    void GameOver ()
    {
        _Movement.gameObject.SetActive(false);
        _UI.GameOver();
    
    }
}
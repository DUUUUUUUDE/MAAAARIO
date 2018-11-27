using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IRestartGameElement
{

    public static PlayerManager instace;
    public PlayerMovement _Movement;
    public PlayerUI _UI;

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

        HP = MaxHP;

        GameManager.instance.AddRestartGameElement(this);

        _UI.RefreshUI();
    }

    public enum States {Idle, Walk, Run, Fall, Jump, Jump_Double, Jump_Triple, Jump_Wall, Punch1, Punch2, Punch3, Hit, Die};
    public static States State;

    public void ChangeState (States newState)
    {
        State = newState;
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
                GameOver();
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

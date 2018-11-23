using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region PUBLIC VARIABLES
    public float _MaxJumpHeight;
    public float _MinJumpHeight;
    public float _TimeToJumpMaxHeight;
    public float _ChangeDirAir;
    public float _ChangeDirGround;
    public float _NormalMoveSpeed;
    public float _RunMoveSpeed;
    #endregion

    #region VARS JUMP
    [HideInInspector]
    public bool CanJump;
    [HideInInspector]
    public bool Jump;
    #endregion

    #region DescendSlope
    bool LateGrounded;
    bool OnSlope;
    #endregion

    #region HIDDEN PUBLIC VARIABLES
    [HideInInspector]
    public float _MoveSpeed;
    [HideInInspector]
    public Vector3 _Velocity;
    [HideInInspector]
    public Vector3 _Move;
    #endregion

    #region PRIVATE VARIABLES
    protected float     _Gravity;
    protected float     _NormalGravity;
    protected float     _MaxJumpVelocity;
    protected float     _MinJumpVelocity;
    protected float     _VelocityGroundSmoothingX;
    protected float     _VelocityGroundSmoothingZ;
    protected float     _VelocityAirSmoothing;
    protected float     _ChangeDirectionTimeAir;
    protected float     _ChangeDirectionTimeGround;
    protected float     _VerticalAxis;
    protected float     _HorizontalAxis;
    CharacterController _CharacterController;
    protected int       _JumpNUM;
    public bool         _OnWall;
    #endregion

    protected void Start()
    {

        _NormalGravity = -(2 * _MaxJumpHeight) / Mathf.Pow(_TimeToJumpMaxHeight, 2);
        _Gravity = _NormalGravity;
        _MaxJumpVelocity = Mathf.Abs(_Gravity) * _TimeToJumpMaxHeight;
        _MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_Gravity) + _MaxJumpHeight);
        _ChangeDirectionTimeAir = _ChangeDirAir;
        _ChangeDirectionTimeGround = _ChangeDirGround;
        _MoveSpeed = _NormalMoveSpeed;
        _CharacterController = GetComponent<CharacterController>();
    }

    public virtual void Walk()
    {
        _MoveSpeed = _NormalMoveSpeed;
    }

    public virtual void Run()
    {
        _MoveSpeed = _RunMoveSpeed;
    }

   


    public void SetMoveDirection(Vector3 Direction)
    {
        _Move.x = Direction.x;
        _Move.z = Direction.z;
    }

    public void Update()
    {
        NormalMovement();
    }

    public virtual void NormalMovement()
    {
        if (_CharacterController.collisionFlags == CollisionFlags.Above && _Velocity.y > 0)
        {
            _Velocity.y = 0;
        }

        // setup target velocity
        float targetVelocityX;
        float targetVelocityZ;
        targetVelocityX = _Move.x * _MoveSpeed;
        targetVelocityZ = _Move.z * _MoveSpeed;

        // setup velocity
        _Velocity.x = Mathf.SmoothDamp(_Velocity.x, targetVelocityX, ref _VelocityGroundSmoothingX, (_CharacterController.isGrounded) ? _ChangeDirectionTimeGround : _ChangeDirectionTimeAir);
        _Velocity.z = Mathf.SmoothDamp(_Velocity.z, targetVelocityZ, ref _VelocityGroundSmoothingZ, (_CharacterController.isGrounded) ? _ChangeDirectionTimeGround : _ChangeDirectionTimeAir);
        if (_OnWall && _Velocity.y > 0)
            _Velocity.y += _Gravity / 3 * Time.deltaTime;
        else
            _Velocity.y += _Gravity * Time.deltaTime;


        //move
        _CharacterController.Move(_Velocity * Time.deltaTime);



        // reset fall
        if ((_CharacterController.isGrounded))
        {
            if (!LateGrounded)
            {
                LateGrounded = true;
                ChecKSlope();
                StartJumpTimer();
            }

            _Velocity.y = 0;
            CanJump = true;

        }
        else
        {
            //Slope
            if (LateGrounded)
                LateGrounded = false;
            if (OnSlope)
                DescendingSlope();
        }
    }



    #region JUMP
    public void CheckJump()
    {

        if (CanJump)
        {
            if (_OnWall)
            {
                StartJump0();
                Vector3 WallNormal = CheckWallNormal ();
                _Velocity.x = WallNormal.x * 10;
                _Velocity.x = WallNormal.z * 10;

                return;
            }

            if (onJumpCombo)
            {
                if (_JumpNUM == 1)
                    StartJump1();
                else if (_JumpNUM == 2)
                    StartJump2();
                else if (_JumpNUM == 3)
                    StartJump0();
            }
            else
            {
                StartJump0();
            }
        }
    }


    void StartJump0()
    {
        _Velocity.y = _MaxJumpVelocity;
        CanJump = false;
        //Slope
        if (OnSlope)
            OnSlope = false;

        _JumpNUM = 1;
    }
    void StartJump1()
    {
        _Velocity.y = _MaxJumpVelocity * 1.5f;
        CanJump = false;
        //Slope
        if (OnSlope)
            OnSlope = false;
        _JumpNUM++;
    }
    void StartJump2()
    {
        _Velocity.y = _MaxJumpVelocity * 2.5f;
        CanJump = false;
        //Slope
        if (OnSlope)
            OnSlope = false;
        _JumpNUM++;
    }
    //JUMP END
    public virtual void EndJump()
    {
        if (_Velocity.y > _MinJumpVelocity)
        {
            _Velocity.y = _MinJumpVelocity;
        }
    }

    void StartJumpTimer ()
    {
        if (!onJumpCombo)
            JumpCoroutine = StartCoroutine(WaitForComboCO());
    }

    bool onJumpCombo;
    Coroutine JumpCoroutine;
    IEnumerator WaitForComboCO ()
    {
        onJumpCombo = true;
        yield return new WaitForSeconds(0.1f);
        onJumpCombo = false;
    }
    

    //WALL JUMP
    Vector3 RandomCircle(Vector3 center, float radius, int a)
    {

        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return pos;
    }
    int rayNum = 8;
    float rayLenght = 0.6f;
    Vector3 CheckWallNormal ()
    {
        Vector3 center = Camera.main.transform.position;
        for (int i = 0; i < rayNum; i++)
        {
            int a = i * 30;
            Vector3 pos = RandomCircle(center, 1.0f, a);

            RaycastHit hit;
            Ray ray = new Ray(center, pos - center);

            if (Physics.Raycast(ray, out hit, rayLenght))
            {
                return hit.normal;
            }
        }

        return Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Map")
            _OnWall = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Map")
            _OnWall = false;
    }
    #endregion


    #region SLOPES
    void ChecKSlope()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit, 0))
        {
            if (hit.normal != Vector3.up && Vector3.Angle(transform.up, hit.normal) < 30)
            {
                OnSlope = true;
            }
        }
    }
    //DescendingSLope
    void DescendingSlope()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit , 0))
        {
            if (hit.normal != Vector3.up && Vector3.Angle(transform.up, hit.normal) < 30)
            {
                _CharacterController.Move(Vector3.down);
            }
        }
    }
    #endregion
}

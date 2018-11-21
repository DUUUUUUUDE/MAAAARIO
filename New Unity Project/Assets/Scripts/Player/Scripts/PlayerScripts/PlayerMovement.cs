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

    public virtual void StartJump()
    {
        if (CanJump)    //Jump
        {
            _Velocity.y = _MaxJumpVelocity;
            CanJump = false;
            //Slope
            if (OnSlope)
                OnSlope = false;
        }
    }
    //JUMP END
    public virtual void EndJump()
    {
        if (_Velocity.y > _MinJumpVelocity)
        {
            _Velocity.y = _MinJumpVelocity;
        }
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

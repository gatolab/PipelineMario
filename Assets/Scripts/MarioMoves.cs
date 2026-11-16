using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioMovController
{
    private readonly PlayerInput _input;
    private readonly AniManager _animations;
    private Rigidbody2D _rigidBody;
    
    private float _walkVelocity = 4.0f;
    private float _acceleration = 14.0f;
    private float _deceleration = 22.0f;
    private float _userMovementX;
    
    private float _jumpForce = 6.5f;
    private float _jumpVelocityX;
    
    private bool _jumping = false;
    private bool _grounded = true;
    private int _floorLayerId = LayerMask.NameToLayer("Floor");
    
    public MarioMovController(PlayerInput input, AniManager animations, Rigidbody2D rigidbody)
    {
        this._input = input;
        this._animations = animations;
        this._rigidBody = rigidbody;
    }

    public void OnUpdate()
    {
        this._userMovementX = this._input.actions["move"].ReadValue<Vector2>().x;
        if (this._userMovementX != 0.0f)
        {
            if (this._grounded)
            {
                if (this._userMovementX < 0.0f) // left
                {
                    this._animations.Sprite.flipX = false;
                }
                else // right
                {
                    this._animations.Sprite.flipX = true;
                }
            }
                
            if(this._grounded) this._animations.Animate("walk");
        }
        else
        {
            if (this._rigidBody.linearVelocityX != 0.0f)
            {
                if(this._grounded) this._animations.Animate("brake");
            }
            else
            {
                if(this._grounded) this._animations.Animate("stand");    
            }
        }

        if (this._rigidBody.linearVelocityY != 0.0f)
        {
            if(!this._grounded) this._animations.Animate("jump");
        }
    }

    public void OnFixedUpdate()
    {
        if(!this._grounded)
        {
            if (this._jumping)
            {
                this._rigidBody.linearVelocityX = this._jumpVelocityX;    
            }
            return;
        }
        
        float targetVelocity = this._userMovementX * this._walkVelocity;
        float rate = this._userMovementX == 0.0f  ? this._deceleration  : this._acceleration;
        
        this._rigidBody.linearVelocityX = Mathf.MoveTowards(
            this._rigidBody.linearVelocityX,
            targetVelocity,
            rate * Time.fixedDeltaTime
        );
        
        // Constant walking speed
        //this._rigidBody.linearVelocityX = targetVelocity;
    }

    public void Jump()
    {
        if (this._grounded)
        {
            this._jumping = true;
            this._grounded = false;
            this._jumpVelocityX = this._rigidBody.linearVelocityX;
            this._rigidBody.linearVelocityY = this._jumpForce;
        }
    }

    private bool LayerIsFloor(int layerId)
    {
        if (layerId == this._floorLayerId)
        {
            return true;
        }
        return false;
    }
    
    public void OnTriggerEnter(int layerId)
    {
        if (this.LayerIsFloor(layerId))
        {
            this._grounded = true;
            this._jumping = false;
        }
    }

    public void OnTriggerExit(int layerId)
    {
        if (this.LayerIsFloor(layerId))
        {
            this._grounded = false;
        }
    }
}

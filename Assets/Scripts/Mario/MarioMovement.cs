using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioMovement : MonoBehaviour
{
    public GameObject sparksRef;
    public GameObject tmpHitRef;
    
    private SpriteRenderer _sprite;
    
    // Horizontal movement
    private float _userInputMovX;
    private float _walkVelocity = 4.0f;
    private float _acceleration = 14.0f;
    private float _deceleration = 22.0f;
    
    // Vertical movement
    private bool _grounded = true;
    private float _jumpForce = 7.0f;
    private int _floorLayerId;
    
    // --------------------------------------------
    // Initialization
    private void Awake()
    {
        this._sprite = GameUtils.GetSpriteRendererIn(GameData.mario.gameObject.transform, "sprite");
        this._floorLayerId = LayerMask.NameToLayer("Floor");
    }

    // --------------------------------------------
    // Update loops
    private void Update()
    {
        if (GameData.mario.isDead) return;
        
        this._userInputMovX = GameUtils.ReadUserInputX();
        this.FlipMarioX();
        
        if (this._grounded)
        {
            if (this._userInputMovX == 0.0f)
            {
                if (GameData.mario.rigidBody.linearVelocityX == 0.0f)
                {
                    GameData.mario.animations.Animate("stand");
                }
                else
                {
                    GameData.mario.animations.Animate("brake");
                }
            }
            else
            {
                GameData.mario.animations.Animate("walk");
            }
        }
        else
        {
            GameData.mario.animations.Animate("jump");
        }
    }

    private void FixedUpdate()
    {
        if (GameData.mario.isDead) return;
        
        float targetVelocity = this._userInputMovX * this._walkVelocity;
        float rate = this._userInputMovX == 0.0f  ? this._deceleration  : this._acceleration;

        if (this._userInputMovX != 0.0f)
        {
            GameData.mario.rigidBody.gravityScale = 1.0f;
        }
        
        GameData.mario.rigidBody.linearVelocityX = Mathf.MoveTowards(
            GameData.mario.rigidBody.linearVelocityX, targetVelocity,
            rate * Time.fixedDeltaTime
        );
    }
    
    // --------------------------------------------
    // PlayerInput actions
    public void OnJump()
    {
        if (GameData.mario.isDead) return;
        
        GameData.mario.rigidBody.gravityScale = 1.0f;
        if (this._grounded)
        {
            this._grounded = false;
            GameData.mario.rigidBody.linearVelocityY = this._jumpForce;
        }
    }
    
    // --------------------------------------------
    // Collisions
    private void OnCollisionEnter2D(Collision2D other)
    {
        float A = GameData.mario.gameObject.transform.position.y;
        float B = other.gameObject.transform.position.y;

        float diff = B - A;
        if (diff > 1.0f && GameData.mario.rigidBody.linearVelocityY >= 0.0f) // Headbutt!
        {
            Vector3 pos = GameData.mario.gameObject.transform.position;
            pos.y += 1.2f;
            
            Instantiate(this.sparksRef, pos, Quaternion.identity);
            Instantiate(this.tmpHitRef, pos, Quaternion.identity);
        }
    }

    // Collider triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == this._floorLayerId)
        {
            this._grounded = true;    
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.layer == this._floorLayerId)
        {
            this._grounded = false;    
        }
    }
    
    // --------------------------------------------
    // misc
    public void FlipMarioX()
    {
        float userInputMovX = GameUtils.ReadUserInputX(); 
        
        if (userInputMovX < 0.0f) // left
        {
            this._sprite.flipX = false;
        }
        if (userInputMovX > 0.0f) // right
        {
            this._sprite.flipX = true;
        }
    }
    
}

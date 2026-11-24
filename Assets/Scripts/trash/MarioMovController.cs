using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioMovController
{
    private GameObject _mario;
    private readonly PlayerInput _input;
    private readonly AniManager _animations;
    private Rigidbody2D _rigidBody;
    
    // Horizontal movement
    private float _userInputMovX;
    private float _walkVelocity = 4.0f;
    private float _acceleration = 14.0f;
    private float _deceleration = 22.0f;
    private int _wallLayerId = LayerMask.NameToLayer("Wall");

    private Transform _wallLeft;
    private Transform _wallRight;
    
    // Vertical movement
    private bool _grounded = true;
    private float _jumpForce = 7.0f;
    private int _floorLayerId = LayerMask.NameToLayer("Floor");
    
    
    // --------------------------------------------
    // Initialization
    public MarioMovController(GameObject mario)
    {
        this._mario = mario;
        this._input = mario.GetComponent<PlayerInput>();
        this._animations = new AniManager(mario.GetSpriteRenderer("sprite"), "stand");
        this._rigidBody = mario.GetRigidBody();
        
        GameObject level = GameObject.FindGameObjectWithTag("Level");
        this._wallLeft = level.transform.Find("wall_left");
        this._wallRight = level.transform.Find("wall_right");
    }
    
    // --------------------------------------------
    // Update loops
    public void OnUpdate()
    {
        this.ReadUserInput();
        this.FlipMarioX();
        
        if (this._grounded)
        {
            if (this._userInputMovX == 0.0f)
            {
                if (this._rigidBody.linearVelocityX == 0.0f)
                {
                    this._animations.Animate("stand");
                }
                else
                {
                    this._animations.Animate("brake");
                }
            }
            else
            {
                this._animations.Animate("walk");
            }
        }
        else
        {
            this._animations.Animate("jump");
        }
    }
    
    public void OnFixedUpdate()
    {
        float targetVelocity = this._userInputMovX * this._walkVelocity;
        float rate = this._userInputMovX == 0.0f  ? this._deceleration  : this._acceleration;
        
        this._rigidBody.linearVelocityX = Mathf.MoveTowards(
            this._rigidBody.linearVelocityX, targetVelocity,
            rate * Time.fixedDeltaTime
        );
    }
    
    // --------------------------------------------
    // PlayerInput actions
    public void Jump2()
    {
        if (this._grounded)
        {
            this._grounded = false;
            this._rigidBody.linearVelocityY = this._jumpForce;
        }
    }
    
    // --------------------------------------------
    // Physics: Collider triggers
    public void OnTriggerEnter(GameObject gameObject)
    {
        int layerId = gameObject.layer;
        
        if (layerId == this._floorLayerId)
        {
            this._grounded = true;    
        } else if(layerId == this._wallLayerId)
        {
            float wallMargin = 0.85f; 
            
            if (gameObject.transform == this._wallLeft)
            {
                Vector2 newPos = this._mario.transform.position;
                newPos.x = this._wallRight.position.x - wallMargin;
                this._mario.transform.position = newPos;
            } else if (gameObject.transform == this._wallRight)
            {
                Vector2 newPos = this._mario.transform.position;
                newPos.x = this._wallLeft.position.x + wallMargin;
                this._mario.transform.position = newPos;
            }
        }
    }
    
    public void OnTriggerExit(int layerId)
    {
        if (layerId == this._floorLayerId)
        {
            this._grounded = false;    
        }
    }
    
    // --------------------------------------------
    // misc
    private void ReadUserInput()
    {
        this._userInputMovX = this._input.actions["move"].ReadValue<Vector2>().x;
    }

    public void FlipMarioX()
    {
        if (this._userInputMovX < 0.0f) // left
        {
            this._animations.Sprite.flipX = false;
        }
        if (this._userInputMovX > 0.0f) // right
        {
            this._animations.Sprite.flipX = true;
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
}

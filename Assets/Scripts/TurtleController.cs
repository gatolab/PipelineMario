using System;
using UnityEngine;

public class TurtleController : MonoBehaviour
{
    private AniManager _animations;
    private Rigidbody2D _rigidBody;
    
    private bool _grounded = true;
    private float _walkVelocity = 3.0f;
    private Transform _wallLeft;
    private Transform _wallRight;
    
    private int _direction;
    
    // --------------------------------------------
    // Initialization
    void Start()
    {
        this._direction = 2;
        
        this.gameObject.GetSpriteRenderer("dot").enabled = false;
        this._animations = new AniManager(this.gameObject.GetSpriteRenderer("sprite"), "stand");
        this._rigidBody = this.gameObject.GetRigidBody();
        
        GameObject level = GameObject.FindGameObjectWithTag("Level");
        this._wallLeft = level.transform.Find("wall_left");
        this._wallRight = level.transform.Find("wall_right");
    }

    // --------------------------------------------
    // Update loops
    private void Update()
    {
        this.FlipTurtleX();
        
        if (this._grounded)
        {
            if (this._rigidBody.linearVelocityX == 0.0f)
            {
                this._animations.Animate("stand");
            }
            else
            {
                this._animations.Animate("walk");
            }
        }
        else
        {
            this._animations.Animate("stand");
        }
    }

    private void FixedUpdate()
    {
        if (this._direction == 2)
        {
            this._rigidBody.linearVelocityX = this._walkVelocity;
        }
        else
        {
            this._rigidBody.linearVelocityX = -this._walkVelocity;
        }
    }

    // --------------------------------------------
    // Physics: Collider triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerId = other.gameObject.layer;
        if (layerId == LayerMask.NameToLayer("Floor"))
        {
            this._grounded = true;
        } else if(layerId == LayerMask.NameToLayer("Wall"))
        {
            float wallMargin = 0.9f; 
            
            if (other.gameObject.transform == this._wallLeft)
            {
                Vector2 newPos = this.gameObject.transform.position;
                newPos.x = this._wallRight.position.x - wallMargin;
                this.gameObject.transform.position = newPos;
            } else if (other.gameObject.transform == this._wallRight)
            {
                Vector2 newPos = this.gameObject.transform.position;
                newPos.x = this._wallLeft.position.x + wallMargin;
                this.gameObject.transform.position = newPos;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int layerId = other.gameObject.layer;
        if (layerId == LayerMask.NameToLayer("Floor"))
        {
            this._grounded = false;
        }
    }
    
    // --------------------------------------------
    // // misc
    
    public void FlipTurtleX()
    {
        if (this._direction == 1) // left
        {
            this._animations.Sprite.flipX = false;
        }
        else // right
        {
            this._animations.Sprite.flipX = true;
        }
    }
}

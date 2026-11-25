using System;
using UnityEngine;

public class TurtleController : MonoBehaviour
{
    private AniManager _animations;
    private Rigidbody2D _rigidBody;
    
    public int direction = 2;
    private float _walkVelocity = 3.0f;
    private bool _grounded = false;
    private int _floorLayerId;
    private int _floorForEnemiesLayerId;
    private int _heroAttack;
    
    private bool _upsideDown = false;
    
    // --------------------------------------------
    // Initialization
    void Awake()
    {
        this.gameObject.GetSpriteRenderer("dot").enabled = false;
        
        this._animations = new AniManager(this.gameObject.GetSpriteRenderer("sprite"), "stand");
        this._rigidBody = this.gameObject.GetRigidBody();
        this._floorLayerId = LayerMask.NameToLayer("Floor");
        this._floorForEnemiesLayerId = LayerMask.NameToLayer("FloorForEnemies");
        this._heroAttack = LayerMask.NameToLayer("HeroAttack");
    }

    // --------------------------------------------
    // Update loops
    private void Update()
    {
        this.FlipTurtleX();
        if (this._upsideDown) return;
        
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
        if (this._upsideDown) return;
        
        if (this.direction == 2)
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
        if (layerId == this._floorLayerId || layerId == this._floorForEnemiesLayerId)
        {
            this._grounded = true;
        }
        else if(layerId == this._heroAttack && this._grounded) // Headbutt!
        {
            if (this._upsideDown)
            {
                this._upsideDown = false;
                this._rigidBody.linearVelocity = new Vector2(0.0f, 4.0f);
                this._animations.Animate("stand");
                CancelInvoke("UpsideDownBack");
            }
            else
            {
                this._upsideDown = true;
                this._rigidBody.linearVelocity = new Vector2(0.0f, 4.0f);
                this._animations.Animate("upsideDown");
            
                Invoke("UpsideDownBack", 2.0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int layerId = other.gameObject.layer;
        if (layerId == this._floorLayerId || layerId == this._floorForEnemiesLayerId)
        {
            this._grounded = false;
        }
    }
    
    // --------------------------------------------
    // // misc
    
    private void UpsideDownBack()
    {
        this._upsideDown = false;
        this._animations.Sprite.color = Color.Lerp(Color.white, Color.red, 0.5f);
        this._animations.Animate("walk");
        this._walkVelocity = 4.5f;
    }
    
    public void FlipTurtleX()
    {
        if (this.direction == 1) // left
        {
            this._animations.Sprite.flipX = false;
        }
        else // right
        {
            this._animations.Sprite.flipX = true;
        }
    }
}

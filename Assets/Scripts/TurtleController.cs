using System;
using UnityEngine;

public class TurtleController : MonoBehaviour
{
    private AniManager _animations;
    private Rigidbody2D _rigidBody;
    
    private float _walkVelocity = 1.5f;
    private float _runVelocity = 3.0f;
    private float _getNervousTime = 2.5f;
    private float _upsideDownTime = 1.0f;
    
    public int direction = 2;
    private bool _grounded = false;
    private int _floorLayerId;
    private int _floorForEnemiesLayerId;
    private int _heroAttack;
    
    private bool _upsideDown = false;
    
    // --------------------------------------------
    // Initialization
    void Awake()
    {
        this.gameObject.GetSpriteRenderer("center").enabled = false;
        
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
            print("raro");
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
        else if(layerId == this._heroAttack) // Headbutt!
        {
            if (this._upsideDown)
            {
                this._upsideDown = false;
                this._rigidBody.linearVelocity = new Vector2(0.0f, 4.0f);
                this._animations.Animate("stand");
                CancelInvoke("GetNervous");
                CancelInvoke("UpsideDownBack");
            }
            else // go upside down!
            {
                this._upsideDown = true;
                this._rigidBody.linearVelocity = new Vector2(0.0f, 4.0f);
                this._animations.Animate("upsideDown");
            
                Invoke("GetNervous", this._getNervousTime);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        int layerId = other.gameObject.layer;
        if (layerId == this._floorLayerId)
        {
            this._grounded = true;
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

    private void GetNervous()
    {
        this._animations.Animate("nervous");
        this._animations.Sprite.color = Color.Lerp(Color.white, Color.red, 0.35f);
        Invoke("UpsideDownBack", this._upsideDownTime);
    }
    
    private void UpsideDownBack()
    {
        print("back!");
        
        this._upsideDown = false;
        this._rigidBody.linearVelocityY = 2.0f;
        this._walkVelocity = this._runVelocity;
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

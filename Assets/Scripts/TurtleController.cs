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
    private int _heroAttackLayerId;
    private int _pipeAreasLayerId;
    private int _enemiesLayerId;
    private int _marioLayerId;
    
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
        this._heroAttackLayerId = LayerMask.NameToLayer("HeroAttack");
        this._pipeAreasLayerId = LayerMask.NameToLayer("PipeAreas");
        this._enemiesLayerId = LayerMask.NameToLayer("Enemies");
        this._marioLayerId = LayerMask.NameToLayer("Mario");
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
    // Collisions
    private void OnCollisionEnter2D(Collision2D other)
    {
        int layerId = other.gameObject.layer;
        if (layerId == this._enemiesLayerId)
        {
            if (!this._upsideDown)
            {
                this.ReverseDirection();    
            }
        } else if (layerId == this._marioLayerId)
        {
            if (this._upsideDown)
            {
                this._rigidBody.gravityScale = 3.0f;
                this._rigidBody.linearVelocityY = 10.0f;
                
                this.GetComponent<BoxCollider2D>().enabled = false;
                SpriteRenderer sprite = this.gameObject.GetSpriteRenderer("sprite");
                sprite.GetComponent<BoxCollider2D>().enabled = false;
                sprite.GetComponent<CapsuleCollider2D>().enabled = false;
                CancelInvoke(); // Cancel all invokes
                
                Invoke("DestroyMe", 2.0f);
                GameData.level.OnEnemyDestroyed();
            }
            else
            {
                GameData.mario.Die();
            }
        }
    }

    // --------------------------------------------
    // Colliders trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerId = other.gameObject.layer;
        if (layerId == this._floorLayerId || layerId == this._floorForEnemiesLayerId)
        {
            this._grounded = true;
        }
        else if(layerId == this._heroAttackLayerId) // Headbutt!
        {
            if (this._upsideDown)
            {
                this._upsideDown = false;
                this._rigidBody.linearVelocity = new Vector2(0.0f, 4.0f);
                this._animations.Animate("stand");
                CancelInvoke(); // Cancel all invokes
            }
            else // go upside down!
            {
                this._upsideDown = true;
                this._rigidBody.linearVelocity = new Vector2(0.0f, 4.0f);
                this._animations.Animate("upsideDown");
            
                Invoke("GetNervous", this._getNervousTime);
            }
        } else if (layerId == this._pipeAreasLayerId)
        {
            string tag = other.gameObject.tag;

            this.ReverseDirection();
            this.gameObject.transform.position = GameData.level.GetSpawnPosition(tag).position;
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

    private void ReverseDirection()
    {
        if (direction == 1) direction = 2;
        else direction = 1;
    }

    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}

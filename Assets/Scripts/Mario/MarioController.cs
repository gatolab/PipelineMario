using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController: MonoBehaviour
{
    [NonSerialized] public PlayerInput input;
    [NonSerialized] public Rigidbody2D rigidBody;
    [NonSerialized] public AniManager animations;
    public bool isDead = false;
    
    // --------------------------------------------
    // Initialization
    private void Start()
    {
        GameUtils.HideSpriteRendererIn(this.transform, "center");
        GameUtils.GetSpriteRendererIn(this.transform, "sprite").flipX = true;
        
        this.input = this.GetComponent<PlayerInput>();
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.animations = new AniManager(GameUtils.GetSpriteRendererIn(this.transform, "sprite"), "stand");
    }

    public void Die()
    {
        this.isDead = true;
        this.GetComponent<BoxCollider2D>().enabled = false;
        
        SpriteRenderer sprite = this.gameObject.GetSpriteRenderer("sprite");
        sprite.GetComponent<BoxCollider2D>().enabled = false;
        sprite.GetComponent<CapsuleCollider2D>().enabled = false;

        this.rigidBody.gravityScale = 0.0f;
        this.rigidBody.linearVelocity = Vector2.zero;
        this.animations.Animate("surprised");

        Invoke("Fall", 0.75f);
    }

    private void Fall()
    {
        this.animations.Animate("dead");
        this.rigidBody.gravityScale = 3.0f;
        this.rigidBody.linearVelocityY = 10.0f;
        
        Invoke("Restart", 2.0f);
    }

    private void Restart()
    {
        this.gameObject.transform.position = new Vector3(0.0f, 4.0f, 0.0f);
        this.rigidBody.gravityScale = 0.0f;
        this.rigidBody.linearVelocity = Vector2.zero;
        this.animations.Animate("stand");
        
        this.GetComponent<BoxCollider2D>().enabled = true;
        SpriteRenderer sprite = this.gameObject.GetSpriteRenderer("sprite");
        sprite.GetComponent<BoxCollider2D>().enabled = true;
        sprite.GetComponent<CapsuleCollider2D>().enabled = true;
        
        this.isDead = false;
    }
}

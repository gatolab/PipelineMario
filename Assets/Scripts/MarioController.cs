using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController: MonoBehaviour
{
    private AniManager _animations;
    private MarioMovController _moves;
    
    private void Start()
    {
        this.GetSpriteRenderer("dot").enabled = false;
        this._animations = new AniManager(this.GetSpriteRenderer("sprite"), "stand");
        this._moves = new MarioMovController(this.GetComponent<PlayerInput>(), this._animations, this.GetRigidBody());
    }

    private void Update()
    {
        this._moves.OnUpdate();
    }

    private void FixedUpdate()
    {
        this._moves.OnFixedUpdate();
    }
    
    public void OnJump() // For "Jump" action from the PlayerInput
    {
        this._moves.Jump();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        this._moves.OnTriggerEnter(other.gameObject.layer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        this._moves.OnTriggerExit(other.gameObject.layer);
    }
}

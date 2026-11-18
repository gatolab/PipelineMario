using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController: MonoBehaviour
{
    private MarioMovController _moves;
    
    private void Start()
    {
        this.gameObject.GetSpriteRenderer("dot").enabled = false;
        this._moves = new MarioMovController(this.gameObject);
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
        this._moves.OnTriggerEnter(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        this._moves.OnTriggerExit(other.gameObject.layer);
    }
}

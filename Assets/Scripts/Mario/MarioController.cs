using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioController: MonoBehaviour
{
    [NonSerialized] public PlayerInput input;
    [NonSerialized] public Rigidbody2D rigidBody;
    [NonSerialized] public AniManager animations;
    
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
    
}

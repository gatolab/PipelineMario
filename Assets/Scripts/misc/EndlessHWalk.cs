using System;
using UnityEngine;

public class EndlessHWalk : MonoBehaviour
{
    private int _wallLayerId;
    public float wallMargin = 1.0f;
    
    // --------------------------------------------
    // Initialization
    private void Awake()
    {
        this._wallLayerId = LayerMask.NameToLayer("Wall");
    }
    
    // --------------------------------------------
    // Physics: Collider triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == this._wallLayerId)
        {
            Vector2 newPos = this.gameObject.transform.position; 
            
            if (other.gameObject == GameData.level.leftWall)
            {
                newPos.x = GameData.level.rightWall.transform.position.x - wallMargin;
            } else if (other.gameObject == GameData.level.rightWall)
            {
                newPos.x = GameData.level.leftWall.transform.position.x + wallMargin;
            }
            
            this.gameObject.transform.position = newPos;
        }
    }
}

using System;
using UnityEditor;
using UnityEngine;

public class TmpHitController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        this.gameObject.GetSpriteRenderer("center").enabled = false;

        SpriteRenderer sprite = this.gameObject.GetSpriteRenderer("colorArea");
        Color mColor = sprite.color;
        mColor.a = 0.0f;
        sprite.color = mColor;
    }

    void Start()
    {
        Invoke("DestroyMe", 0.3f);
    }
    
    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
    
    
}

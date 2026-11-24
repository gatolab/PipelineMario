using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public static class GameUtils
{
    // --------------------------------------------
    // SpriteRenderer(s)
    public static void HideSpriteRendererIn(Transform obj, string name)
    {
        SpriteRenderer sprite = GameUtils.GetSpriteRendererIn(obj, name);
        sprite.enabled = false;
    }

    public static SpriteRenderer GetSpriteRendererIn(Transform obj, string name)
    {
        Transform transform = obj.Find(name);
        if (transform)
        {
            SpriteRenderer sprite = transform.GetComponent<SpriteRenderer>();
            return sprite;
        }

        return null;
    }

    // --------------------------------------------
    // PlayerInput
    public static float ReadUserInputX()
    {
        return GameData.mario.input.actions["move"].ReadValue<Vector2>().x;
    }
    
    // --------------------------------------------
    public static int RandomInt(int min, int max)
    {
        return Random.Range(min, max + 1);
    }
}

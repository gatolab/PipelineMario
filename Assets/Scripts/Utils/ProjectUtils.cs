using UnityEngine;

public static class ProjectUtils
{
    // -------------------------------------------------------------------------------------
    // MonoBehaviour (main script) extensions
    
    public static SpriteRenderer GetSpriteRenderer(this MonoBehaviour obj, string name)
    {
        // name: Can be used with paths, example: "container/mySprite"    
        
        Transform transform = obj.transform.Find(name);
        if (transform)
        {
            return transform.GetComponent<SpriteRenderer>();
        }
        else
        {
            ProjectUtils.ShowError(obj.name, "sprite not found");
            return null;
        }
    }
    
    public static Rigidbody2D GetRigidBody(this MonoBehaviour obj)
    {
        return obj.GetComponent<Rigidbody2D>();
    }
    
    // -------------------------------------------------------------------------------------
    // misc

    public static void ShowError(string name, string msg)
    {
        Debug.LogError(">> ERROR in \"" + name + "\": " + msg);
    }
    
    // -------------------------------------------------------------------------------------
}
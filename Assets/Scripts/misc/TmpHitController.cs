using UnityEngine;

public class TmpHitController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DestroyMe", 0.3f);
    }
    
    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
    
    
}

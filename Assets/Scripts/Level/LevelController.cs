using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject enemyRef;
    
    public GameObject leftWall;
    public GameObject rightWall;

    private Transform _spawnLeft;
    private Transform _spawnRight;
    
    // --------------------------------------------
    // Initialization
    private void Awake()
    {
        this._spawnLeft = this.transform.Find("spawn_left").transform;
        this._spawnRight = this.transform.Find("spawn_right").transform;
        
        //Invoke("SpawnEnemy", 1.0f);
        InvokeRepeating("SpawnEnemy", 1.0f, 10.0f);
    }

    void SpawnEnemy()
    {
        int side = GameUtils.RandomInt(1, 2);

        Vector3 pos = this._spawnLeft.transform.position;
        if (side == 2) pos = this._spawnRight.transform.position;
        
        GameObject newEnemy = Instantiate(this.enemyRef, pos, Quaternion.identity);
        newEnemy.GetComponent<TurtleController>().direction = (side == 1) ? 2 : 1;
    }
}

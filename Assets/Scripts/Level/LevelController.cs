using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject enemyRef;
    
    public GameObject leftWall;
    public GameObject rightWall;

    private Transform _spawnLeft;
    private Transform _spawnRight;

    private int _totalEnemies = 4;
    private int _enemiesCount = 0;
    
    // --------------------------------------------
    // Initialization
    private void Awake()
    {
        GameUtils.HideSpriteRendererIn(this.transform, "center");
        
        this._spawnLeft = this.transform.Find("spawn_left").transform;
        this._spawnRight = this.transform.Find("spawn_right").transform;
        
        float initialDelay = 0.0f;
        float delayBetweenEnemies = 5.0f;
        InvokeRepeating("SpawnEnemy", initialDelay, delayBetweenEnemies);
    }

    void SpawnEnemy()
    {
        int side = GameUtils.RandomInt(1, 2);

        Vector3 pos = this._spawnLeft.transform.position;
        if (side == 2) pos = this._spawnRight.transform.position;
        
        GameObject newEnemy = Instantiate(this.enemyRef, pos, Quaternion.identity);
        newEnemy.GetComponent<TurtleController>().direction = (side == 1) ? 2 : 1;

        this._enemiesCount += 1;
        if (this._enemiesCount >= this._totalEnemies)
        {
            CancelInvoke("SpawnEnemy");
        }
    }
    
    public Transform GetSpawnPosition(string tag)
    {
        if (tag == "left")
        {
            return this._spawnLeft;
        }
        else
        {
            return this._spawnRight;
        }
    }
}

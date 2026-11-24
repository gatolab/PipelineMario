using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Awake()
    {
        GameData.level = GameObject.Find("level").GetComponent<LevelController>();
        GameData.mario = GameObject.Find("mario").GetComponent<MarioController>();
    }
}

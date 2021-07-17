using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GroundController : MonoBehaviour
{
    public UnityAction SpawnNewMiniGround;
    [SerializeField] private ObjectPool objectPool = null;
    [SerializeField] private GameObject startGround, miniGround;

    private Vector3 startGroundPos = new Vector3(0f, 0f, 0f);
    private Vector3 lastGeneratedMiniGroundPos;

    private int minXPosGameStart = -8, maxZposGameStart= 8;
    private int minXPos, maxZPos;
    public Vector3 StartGroundPos
    {
        get { return startGroundPos; }
    }
   
    public int MinXPos
    {
        get { return minXPos; }
    }
    public int MaxZPos
    {
        get { return maxZPos; }
    }
    private GameObject generatedStartGround;
    private void Awake()
    {
        SpawnNewMiniGround = SpawnMiniGround;
        GameManager.instance.PreparingGame = PrepareGame;
        generatedStartGround =  Instantiate(startGround, startGroundPos, Quaternion.identity);
    }
    void Start()
    {
        PrepareGame();
        
    }
    void Update()
    {
        switch (GameManager.instance.CurrentGameState)
        {
            case GameManager.GameState.Prepare:
               // Prepare();
                break;
            case GameManager.GameState.MainGame:
                break;
            case GameManager.GameState.FinishGame:
                break;
            default:
                break;
        }
    }
    private void PrepareGame()
    {
        generatedStartGround.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        generatedStartGround.transform.position = startGroundPos;
        minXPos = minXPosGameStart;
        maxZPos = maxZposGameStart;
        lastGeneratedMiniGroundPos = startGroundPos;
        for (int i = 0; i < 45; i++)
        {
            SpawnMiniGround();
        }
    }
    private void SpawnMiniGround()
    {
        Vector3 spawnDirection;
        spawnDirection = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.forward;
      
        if (!CheckSpawnedPos(lastGeneratedMiniGroundPos, spawnDirection))
        {
            spawnDirection = spawnDirection == Vector3.left ? Vector3.forward : Vector3.left;
        }
        var lastMiniGroundObj = objectPool.GetPooledObject(0);
        lastMiniGroundObj.transform.position = lastGeneratedMiniGroundPos + spawnDirection;
        lastGeneratedMiniGroundPos = lastMiniGroundObj.gameObject.transform.position;
        if (spawnDirection == Vector3.left)
        {
            maxZPos += 1;
        }
        else
        {
            minXPos -= 1;
        }
    }
    private bool CheckSpawnedPos(Vector3 lastGeneratedMiniGroundPos, Vector3 dir)
    {
        if ((lastGeneratedMiniGroundPos + dir).x < minXPos || (lastGeneratedMiniGroundPos + dir).z > maxZPos)
        {
            return false;
        }
        return true;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    private Vector3 targetPos;
    private float playerXPos, playerZPos, middleOfThePos;
    private Vector3 cameraStartPos;
    void Start()
    {
        cameraStartPos = transform.position;
    }
    void Update()
    {
        switch (GameManager.instance.CurrentGameState)
        {
            case GameManager.GameState.Prepare:
                transform.position = cameraStartPos;
                break;
            case GameManager.GameState.MainGame:
                CameraMove();
                break;
            case GameManager.GameState.FinishGame:
                break;
        }
    }
    private void CameraMove()
    {
        transform.position = Vector3.Lerp(transform.position, CalculateTargetPos(), Time.deltaTime);
    }
    private void SetPlayerPositions()
    {
        playerXPos = (float)playerPos.position.x;
        playerZPos = (float)playerPos.position.z;
        middleOfThePos = (Mathf.Abs(playerXPos) + Mathf.Abs(playerZPos)) / 2;
    }
    private Vector3 CalculateTargetPos()
    {
        SetPlayerPositions();
        float targetXPos, targetZPos;
        targetXPos = (playerXPos < 0) ? -middleOfThePos : middleOfThePos;
        targetZPos = (playerZPos < 0) ? -middleOfThePos : middleOfThePos;

        return new Vector3(targetXPos, transform.position.y, targetZPos);
    }

    
}

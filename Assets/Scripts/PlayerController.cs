using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    // In this script, the game is shaped according to the movement of the player.
    private Vector3 moveDirection = Vector3.zero;
    public float moveSpeed;
    private float moveStartSpeed;
    [SerializeField] private StartGround startGroundObj;
    [SerializeField] private GroundController groundController;
    public Vector3 playerStartPosition;
    [SerializeField] private Rigidbody rb;
    private bool isIncreaseSpeed = true;

    void Start()
    {
        moveStartSpeed = moveSpeed;
        playerStartPosition = new Vector3(groundController.StartGroundPos.x + startGroundObj.StartGroundObjScale.x / 2, groundController.StartGroundPos.y, groundController.StartGroundPos.z - startGroundObj.StartGroundObjScale.z / 2);
        transform.position = playerStartPosition;
    }
    
    
   
    void Update()
    {
        switch (GameManager.instance.CurrentGameState)
        {
            case GameManager.GameState.Prepare:
                StopAllCoroutines();
                ChangePlayerDirection();
                PrepareGame();
                break;
            case GameManager.GameState.MainGame:
                if (isIncreaseSpeed)
                {
                    StartCoroutine("IncreaseSpeed");
                    isIncreaseSpeed = false;
                }
                ChangePlayerDirection();
                CheckPlayerTouchGround();
                CheckPlayerDie();
                break;
            case GameManager.GameState.FinishGame:
                StopAllCoroutines();
                isIncreaseSpeed = true;
                break;
        }
        transform.Translate(moveDirection*Time.deltaTime*moveSpeed);
    }

    // The direction changes every time the player clicks the mouse.
    private void ChangePlayerDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (moveDirection == Vector3.zero || moveDirection == Vector3.left)
            {
                moveDirection = Vector3.forward;
            }
            else
            {
                moveDirection = Vector3.left;
            }
        }
    }
    // When the player stops touching the floor, the floor begins to fall to the ground.
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.CompareTag("StartGround"))
        {
            other.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        if (other.transform.parent.CompareTag("MiniGround"))
        {
            StartCoroutine(CallGroundFall(other.transform.parent.gameObject));
        }
    }
     IEnumerator CallGroundFall(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        groundController.SpawnNewMiniGround?.Invoke();
    }
    IEnumerator IncreaseSpeed()
    {
        yield return new WaitForSeconds(5f);
        moveSpeed += 0.05f;
        StartCoroutine("IncreaseSpeed");
    }

    private void PrepareGame()
    {
        moveSpeed = moveStartSpeed;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        transform.position = playerStartPosition;
    }
    private void CheckPlayerDie()
    {
        if (transform.position.y < 0)
        {
            GameManager.instance.CurrentGameState = GameManager.GameState.FinishGame;
        }
    }
    private void CheckPlayerTouchGround()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), -transform.up, out RaycastHit hit, 20f))
        {
           
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}

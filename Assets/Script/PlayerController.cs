using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private CapsuleCollider col;
    private Vector3 dir;
    private Score score;
    [SerializeField] private int speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private int coin;
    [SerializeField] private Text coinText;
    [SerializeField] private Score scoreScript;
    [SerializeField] private GameObject scoreText;
    private int lineToMove = 1;
    public float lineDistance = 4;
    private bool isImmortal;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        col = GetComponent<CapsuleCollider>();
        Time.timeScale = 1;
        score = scoreText.GetComponent<Score>();
        isImmortal = false;

    }

    private void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (lineToMove < 2)
                lineToMove++;
        }

        if (SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
                lineToMove--;
        }

        if (SwipeController.swipeUp)
        {
            if (controller.isGrounded)
                Jump();
        }

        if (SwipeController.swipeDown)
        {
            StartCoroutine(Slide());
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private void Jump()
    {
        dir.y = jumpForce;
    }

    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    public void LoadScene(int sceneid)
    {
        SceneManager.LoadScene(sceneid);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isImmortal)
        { coin++; }
            
        else
        {
            if (hit.gameObject.tag == "obstacle")
            {
                Time.timeScale = 0;
                int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunScore);
                LoadScene(0);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            coin++;
            coinText.text = coin.ToString();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag =="bonus")
        {
            StartCoroutine(Bonus());
            //Destroy(other.gameObject);
        }
    }

    private IEnumerator Slide()
    {
        col.center = new Vector3(0, -0.4f, 0);
        col.height = 0.5f;
        yield return new WaitForSeconds(1);
        col.center = new Vector3(0, 0, 0);
        col.height = 1f;
    }

    private IEnumerator Bonus()
    {
        isImmortal = true;
        yield return new WaitForSeconds(5);
        isImmortal = false;
    }
}
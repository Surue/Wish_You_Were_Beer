using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    Rigidbody body;
    [SerializeField]
    float speed;
    float horizontal;
    Vector2 movement;
    bool isturnedRight;

    [SerializeField]
    GameObject beerPrefab;

    [Header("UI")]
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    CustomerManager customerManager;

    private Animator animatorController;
    private QTEController qteController;

    private GameManager gameManager;

    bool inTrigger = false;

    bool isInQTE = false;

    const int failMax = 3;
    int fails = 0;

    private float score;

    enum State
    {
        IDLE,
        MOVING,
        DRINK,
        QTE
    }
    State state = State.MOVING;

	void Start ()
    {
        gameManager = FindObjectOfType<GameManager>();
        beerPrefab.SetActive(false);
        scoreText.text = "";
        animatorController = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        qteController = GetComponent<QTEController>();
	}

    void Update ()
    {
        horizontal = Input.GetAxis("Horizontal");       
        movement = new Vector2(horizontal, 0.0f);
        switch (state)
        {
            case State.IDLE:
                animatorController.speed = 0;

                if (Mathf.Abs(horizontal) > 0.0f)
                {
                    state = State.MOVING;
                }

                if (Input.GetKeyDown(KeyCode.Space) && inTrigger) {
                    state = State.DRINK;
                }
                break;

            case State.MOVING:
                animatorController.SetFloat("horizontal", Mathf.Abs(horizontal));
                animatorController.speed = 1;

                if (horizontal > 0 && !isturnedRight)
                {
                    transform.eulerAngles = new Vector3(0,90,0);
                    isturnedRight = true;                  
                }
                if (horizontal < 0 && isturnedRight)
                {
                    transform.eulerAngles = new Vector3(0, -90, 0);
                    isturnedRight = false;
                }
                if(Mathf.Abs(horizontal) == 0)
                {
                    state = State.IDLE;
                }

                if (Input.GetKeyDown(KeyCode.Space) && inTrigger) {
                    state = State.DRINK;
                }
                break;

            case State.DRINK:
                animatorController.speed = 1;
                animatorController.SetBool("Drinking", true);
                beerPrefab.SetActive(true);
                state = State.QTE;
                break;

            case State.QTE:
                
                if (!isInQTE) {
                    qteController.StartQTE();
                    isInQTE = true;
                }

                if (customerManager.IsEveryoneDrinking()) {
                    qteController.ForceStop();
                    isInQTE = false;
                }

                if (qteController.state == QTEController.State.LOSE) {
                    Debug.Log("Lose");
                    fails++;
                    isInQTE = false;
                    animatorController.SetBool("Drinking", false);
                    beerPrefab.SetActive(false);
                    state = State.MOVING;
                }

                if(qteController.state == QTEController.State.WIN) {
                    Debug.Log("Win");
                    customerManager.HasDrunk();
                    score++;
                    scoreText.text = score.ToString();
                    isInQTE = false;
                    animatorController.SetBool("Drinking", false);
                    beerPrefab.SetActive(false);
                    state = State.MOVING;
                }
                break;
        }

        if(fails == failMax) {
            gameManager.Defeate();
        }
	}

    void FixedUpdate()
    {
        if(state == State.MOVING)
        {
            body.velocity = movement * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == customerManager.GetTriggerWC()) {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other == customerManager.GetTriggerWC()) {
            inTrigger = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody body;
    [SerializeField]
    float speed;
    float horizontal;
    Vector2 movement;
    bool isturnedRight;

    private Animator animatorController;
    private QTEController qteController;

    bool isInQTE = false;

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

                if (Input.GetKeyDown(KeyCode.Space)) {
                    state = State.QTE;
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

                if (Input.GetKeyDown(KeyCode.Space)) {
                    state = State.QTE;
                }
                break;

            case State.DRINK:
                break;

            case State.QTE:
                animatorController.speed = 0;
                if (!isInQTE) {
                    qteController.StartQTE();
                    isInQTE = true;
                }

                if(qteController.state == QTEController.State.LOSE) {
                    Debug.Log("Lose");
                    isInQTE = false;
                    state = State.MOVING;
                }

                if(qteController.state == QTEController.State.WIN) {
                    Debug.Log("Win");
                    isInQTE = false;
                    state = State.MOVING;
                }
                
                break;
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

    }
}

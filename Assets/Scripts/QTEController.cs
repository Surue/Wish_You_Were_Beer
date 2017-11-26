using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class QTEController : MonoBehaviour {

    [SerializeField]
    private GameObject keyPrefab;
    private SkeletonAnimation skeletonAnimation;
    
    
    private float timerCurrent = 0.0f;
    private float timeToPress = 2.0f;

    private enum KeyToPress {
        Q,
        W,
        E,
        R,
        T,
        Z,
        LENGTH
    }
    private KeyToPress keyToPress;

    private bool keyFound;
    private int keyToFound = 5;
    private int keyFailed = 0;
    private const int keyPossibleTofail = 3;

    private GameObject currentKeySprite; 

    public enum State {
        IDLE,
        CHOOSING,
        WAITING,
        WIN,
        LOSE
    }
    public State state;

    // Use this for initialization
    void Start () {
        skeletonAnimation = keyPrefab.GetComponent<SkeletonAnimation>();
        state = State.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
        switch (state) {
            case State.CHOOSING:
                if (keyFailed == keyPossibleTofail) {
                    state = State.LOSE;
                }
                else {
                    keyFound = false;
                    ChooseRandomKey();
                    timerCurrent = 0.0f;
                    state = State.WAITING;
                    StartCoroutine(WaitForPressRightTouch());
                }
                break;

            case State.WAITING:
                if (keyFound && keyToFound > 0) {
                    state = State.CHOOSING;
                    keyToFound--;
                }

                if(keyToFound == 0) {
                    state =  State.WIN;
                }

                if(keyFailed == keyPossibleTofail) {
                    state = State.LOSE;
                }

                timerCurrent += Time.deltaTime;
                if(timerCurrent > timeToPress) {
                    keyFailed++;
                    state = State.CHOOSING;
                }
                break;

            case State.LOSE:
            case State.WIN:
                keyPrefab.gameObject.SetActive(false);
                break;
        }
    }

    public void StartQTE() {
        keyFailed = 0;
        keyToFound = 5;
        state = State.CHOOSING;    
    }

    IEnumerator WaitForPressRightTouch() {
        bool hasFailed = false;
        while(Time.deltaTime < timeToPress && !keyFound && !hasFailed) {
            if (Input.GetKey(KeyToPressInString())) {
                keyFound = true;
            }else if (Input.anyKeyDown) {
                keyFailed++;
                hasFailed = true;
            }
            yield return new WaitForFixedUpdate();
        }

        if (hasFailed) {
            state = State.CHOOSING;
        }

        Destroy(currentKeySprite);
    }

    void ChooseRandomKey() {
        for(int i = 0; i < 1; i++)
        keyToPress = (KeyToPress)Random.Range(0, (float)KeyToPress.LENGTH);

        switch (keyToPress) {
            case KeyToPress.Q:
                skeletonAnimation.AnimationName = "Q";
                break;

            case KeyToPress.W:
                skeletonAnimation.AnimationName = "W";
                break;

            case KeyToPress.E:
                skeletonAnimation.AnimationName = "E";
                break;

            case KeyToPress.R:
                skeletonAnimation.AnimationName = "R";
                break;

            case KeyToPress.T:
                skeletonAnimation.AnimationName = "T";
                break;

            case KeyToPress.Z:
                skeletonAnimation.AnimationName = "Z";
                break;
        }

        keyPrefab.gameObject.SetActive(true);
    }
    
    KeyCode KeyToPressInString() {
        switch (keyToPress) {
            case KeyToPress.Q:
                return KeyCode.Q;

            case KeyToPress.W:
                return KeyCode.W;

            case KeyToPress.E:
                return KeyCode.E;

            case KeyToPress.R:
                return KeyCode.R;

            case KeyToPress.T:
                return KeyCode.T;

            case KeyToPress.Z:
                return KeyCode.Z;

            default:
                return KeyCode.A;
        }
        
    }
}

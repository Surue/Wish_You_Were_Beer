using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEController : MonoBehaviour {

    private GameObject[] spawnsPoints;

    private float timerTotal = 0.0f;
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
    float totalLength = (float)KeyToPress.LENGTH;

    private bool keyFound;

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
        spawnsPoints = GameObject.FindGameObjectsWithTag("qteSpawnPoint");
        state = State.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
        switch (state) {
            case State.CHOOSING:
                keyFound = false;
                //ChooseRandomKey();
                keyToPress = KeyToPress.E;
                Debug.Log(keyToPress);
                state = State.WAITING;
                StartCoroutine(WaitForPressRightTouch());
                break;

            case State.WAITING:
                if (keyFound) {
                    Debug.Log("Touche trouvée");
                    state = State.WIN;
                }
                break;
        }
    }

    public void StartQTE() {
        state = State.CHOOSING;
    }

    IEnumerator WaitForPressRightTouch() {
        while(Time.deltaTime < timeToPress && !keyFound) {
            //if (Input.GetKeyDown(KeyToPressInString())) {
            if (Input.GetKeyDown(KeyCode.E)) {
                Debug.Log("Bonne touche");
                keyFound = true;
            }
            Debug.Log("ICI");
            yield return new WaitForFixedUpdate();
        }
    }

    void ChooseRandomKey() {
        for(int i = 0; i < 1; i++)
        keyToPress = (KeyToPress)Random.Range(0, (float)KeyToPress.LENGTH - 1);
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

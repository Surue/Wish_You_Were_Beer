using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CustomerManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] customers;
    [SerializeField]
    GameObject dotPrefab;
    SkeletonAnimation skeletonAnimation;

    enum State {
        DRINKING,
        WC
    }

    State[] states = new State[5];
    int choosenOne;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < customers.Length; i++) {
            states[i] = State.DRINKING;
        }

        skeletonAnimation = dotPrefab.GetComponent<SkeletonAnimation>();
        dotPrefab.SetActive(false);
        StartCoroutine(GoesInWC());
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < customers.Length; i++) {
            if(states[i] == State.WC) {
                customers[i].SetActive(false);
            }
        }
	}

    IEnumerator GoesInWC() {
        while (true) {
            yield return new WaitForSeconds(10);
            //IN WC
            states[choosenOne] = State.DRINKING;
            customers[choosenOne].SetActive(true);
            dotPrefab.SetActive(false);
            yield return new WaitForSeconds(2);
            //Wait for next one
            ChooseOneCustomerThatGoesInWC();
            skeletonAnimation.Initialize(true);
            dotPrefab.SetActive(true);
        }
    }

    void ChooseOneCustomerThatGoesInWC() {
        choosenOne = Random.Range(0, customers.Length);

        states[choosenOne] = State.WC;
    }
}

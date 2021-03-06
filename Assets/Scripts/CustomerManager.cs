﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CustomerManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] customers;
    [SerializeField]
    GameObject dotPrefab;
    SkeletonAnimation skeletonAnimation;

    QTEController qteController;

    enum State {
        DRINKING,
        WC
    }

    State[] states = new State[5];
    int choosenOne;
    bool beerDrunk = false;

	// Use this for initialization
	void Start () {
        qteController = FindObjectOfType<QTEController>();
        for(int i = 0; i < customers.Length; i++) {
            states[i] = State.DRINKING;
        }

        skeletonAnimation = dotPrefab.GetComponent<SkeletonAnimation>();
        dotPrefab.SetActive(false);
        StartCoroutine(GoesInWC());
	}
	
	// Update is called once per frame
	void Update () {
        GameObject choosenBeer = null;
        foreach (Transform child in customers[choosenOne].transform) {
            if (child.CompareTag("Beer")) {
                choosenBeer = child.gameObject;
            }
        }
        

        for(int i = 0; i < customers.Length; i++) {
            if (states[i] == State.WC) {
                customers[i].GetComponentInChildren<MeshRenderer>().enabled = false;

                foreach (Transform child in customers[i].transform) {
                    if (child.CompareTag("Halo")) {
                        child.gameObject.SetActive(true);
                    }
                }

            }
            else {
                customers[i].GetComponentInChildren<MeshRenderer>().enabled = true;

                foreach (Transform child in customers[i].transform) {
                    if (child.CompareTag("Halo")) {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    IEnumerator GoesInWC() {
        while (true) {
            
            
            yield return new WaitForSeconds(2);
            //Wait for next one
            ChooseOneCustomerThatGoesInWC();
            beerDrunk = false;
            skeletonAnimation.Initialize(true);
            dotPrefab.SetActive(true);
            yield return new WaitForSeconds(8);
            //IN WC
            states[choosenOne] = State.DRINKING;
            customers[choosenOne].GetComponentInChildren<MeshRenderer>().enabled = false;
            dotPrefab.SetActive(false);
            yield return new WaitForSeconds(0);
            Debug.Log(qteController.state);
            if (!beerDrunk) {
                Debug.Log("On s'est fait prendre");
                GameObject bulle = null;
                foreach (Transform child in customers[choosenOne].transform) {
                    if (child.CompareTag("Bulle")) {
                        bulle = child.gameObject;
                    }
                }
                bulle.GetComponent<SkeletonAnimation>().AnimationName = "Exclamation";
                Debug.Log(bulle.GetComponent<SkeletonAnimation>());
                bulle.SetActive(true);
                yield return new WaitForSeconds(2);
                bulle.SetActive(false);
            }
            if (beerDrunk) {
                GameObject bulle = null;
                foreach (Transform child in customers[choosenOne].transform) {
                    if (child.CompareTag("Bulle")) {
                        bulle = child.gameObject;
                    }
                }
                bulle.GetComponent<SkeletonAnimation>().AnimationName = "Interogation";
                bulle.SetActive(true);
                yield return new WaitForSeconds(2);
                bulle.SetActive(false);
            }
        }
    }

    void ChooseOneCustomerThatGoesInWC() {
        choosenOne = Random.Range(0, customers.Length);

        states[choosenOne] = State.WC;
    }

    public bool IsEveryoneDrinking() {
        foreach(State state in states) {
            if(state == State.WC) {
                return false;
            }
        }

        return true;
    }

    public Collider GetTriggerWC() {
        for(int i = 0; i < customers.Length; i++) {
            if(states[i] == State.WC) {
                return customers[i].gameObject.GetComponentInChildren<BoxCollider>();
            }
        }

        return null;
    }

    public void HasDrunk() {
        beerDrunk = true;
    }
}

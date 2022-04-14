using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour {


    Animator spikeTrapAnim;

    void Awake()
    {
        spikeTrapAnim = GetComponent<Animator>();
        StartCoroutine(OpenCloseTrap());
    }

    IEnumerator OpenCloseTrap()
    {
        spikeTrapAnim.SetTrigger("open");
        yield return new WaitForSeconds((float)1.5);

        spikeTrapAnim.SetTrigger("close");
        yield return new WaitForSeconds((float)1.5);

        StartCoroutine(OpenCloseTrap());
    }
}
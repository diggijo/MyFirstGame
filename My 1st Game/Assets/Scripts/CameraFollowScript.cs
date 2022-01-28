using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform mainCharacter;
    public Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = transform.position - mainCharacter.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = mainCharacter.transform.position + cameraPosition;
        transform.position = newPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Vector3 startPosition;
    public Vector3 targetPosition = Vector3.zero;
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if(FinishPoint.isOpenGate == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}

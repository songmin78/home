using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMainCamera : MonoBehaviour
{
    [SerializeField] GameObject objPlayer;
    [SerializeField] float pod = -1.0f;

    void Update()
    {
        if (objPlayer == null) return;

        Vector3 pos = objPlayer.transform.position;
        pos.z = pod;
        transform.position = pos;
    }
}

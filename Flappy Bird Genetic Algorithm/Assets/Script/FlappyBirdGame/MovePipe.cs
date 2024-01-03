using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePipe : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        transform.position += Vector3.left * PipeSpawner.Instance.pipeMoveSpeed * Time.deltaTime;
    }
}

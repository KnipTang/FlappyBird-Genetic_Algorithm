using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestPipe : MonoBehaviour
{
    public static ClosestPipe Instance { get; private set; }
    // Start is called before the first frame update
    private GameObject closestPipe;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public GameObject GetClosestPipe()
    {
        UpdateClosestPipe();
        return closestPipe;
    }
    // Update is called once per frame
    void UpdateClosestPipe()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        List<GameObject> notHitPipes = new List<GameObject>();
        if (notHitPipes != null)
        {
            foreach (GameObject pipe in pipes)
            {
                SpriteRenderer resetColor = pipe.GetComponent<SpriteRenderer>();
                resetColor.color = Color.gray;

                PipeIncreaseScore pipeScore = pipe.GetComponent<PipeIncreaseScore>();
                if (!pipeScore.GetHit())
                    notHitPipes.Add(pipe);
            }
            if (notHitPipes.Count > 0)
            {
                SpriteRenderer pipeRenderer = notHitPipes[0].GetComponent<SpriteRenderer>();
                pipeRenderer.color = Color.red;
                closestPipe = notHitPipes[0];
            }
        }
    }
}

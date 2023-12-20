using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeIncreaseScore : MonoBehaviour
{
    private bool wasHit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject bird = collision.gameObject;

        Score birdScore = bird.GetComponent<Score>();
        if (birdScore != null)
        {
            birdScore.IncreaseScore();
        }

        Invoke("SetHit", 2f);

        Destroy(gameObject, 10);
    }

    void SetHit()
    {
        wasHit = true;
    }

    public bool GetHit()
    {
        return wasHit;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeIncreaseScore : MonoBehaviour
{
    private bool wasHit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject bird = collision.gameObject;
        GameObject HUD = GameObject.FindGameObjectWithTag("HUD");
        Score birdScore = bird.GetComponent<Score>();
        if (birdScore != null)
        {
            birdScore.IncreaseScore();
            if(HUD != null)
            {
                if(birdScore.GetScore() > HUD.GetComponent<HUD>().score)
                    HUD.GetComponent<HUD>().score = birdScore.GetScore();
            }
        }
    }
    private void FixedUpdate()
    {
        if (gameObject.transform.position.x < -1f && wasHit != true)
        {
            SetHit();
        }
        if (gameObject.transform.position.x < -5f)
        {
            Destroy(gameObject);
        }
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

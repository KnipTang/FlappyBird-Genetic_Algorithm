using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Score : MonoBehaviour
{
    private int _score = 0;
    private float startTime;
    public float fitnessScore = 0;
    // Start is called before the first frame update

    void Start()
    {
        startTime = Time.time;
    }
    public int GetScore()
    {
        return _score;
    }

    public void IncreaseScore()
    {
        _score++;
    }
    private void Update()
    {
        FlyBehavior flyBehavior = GetComponent<FlyBehavior>();
        if(!flyBehavior.wasCollided)
        fitnessScore = Time.time - startTime;
        //Debug.Log(_score);
    }
}

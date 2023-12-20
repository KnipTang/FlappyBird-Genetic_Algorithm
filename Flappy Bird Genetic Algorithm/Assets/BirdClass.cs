using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdClass : MonoBehaviour
{
    public float fitnessScore { get; set; }
    public GameObject bird { get; set; }
    public DNA birdDNA { get; set; }

    public BirdClass(float score, GameObject currentBird)
    {
        fitnessScore = score;
        bird = currentBird;

        GameObject controller = GameObject.FindGameObjectWithTag("PopulationController");
        PopulationController controllerScript = controller.GetComponent<PopulationController>();
        int currentGen = controllerScript.gen;
    }
}

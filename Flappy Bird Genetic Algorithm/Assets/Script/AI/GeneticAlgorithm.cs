using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneticAlgorithm : MonoBehaviour
{
    private NeuralNetwork neuralNetwork;
    private float distanceX;
    private float distanceY;
    private float weight = 0;

    private void Awake()
    {
        //Get neuralNetwork linked to the bird gameobject
        neuralNetwork = gameObject.GetComponent<NeuralNetwork>();
    }
    private void Update()
    {
        //Get the distanceX/Y from the gameobject bird to the center of the pipe.
        SetInputLayers();

        //Pass these distance values to the neuralNetwork FeedForward function to get the weight.
        float[] inputs = { distanceX, distanceY };
        weight = neuralNetwork.FeedForward(inputs);
        //If weight is higher than 0.5f -> Flap
        if (weight >= 0.5f)
            Flap();  
    }

    private void SetInputLayers()
    {
        GameObject closestPipe = ClosestPipe.Instance.GetClosestPipe();
        if (closestPipe != null)
        {
            Transform pipeCenterTransform = closestPipe.transform.Find("Center");

            if(pipeCenterTransform != null)
            {
                distanceX = pipeCenterTransform.position.x;
                distanceY = pipeCenterTransform.position.y - gameObject.transform.position.y;
            }
        }
    }
   
    private void Flap()
    {
        FlyBehavior birdJump = gameObject.GetComponent<FlyBehavior>();
        birdJump.Jump();
    }
}
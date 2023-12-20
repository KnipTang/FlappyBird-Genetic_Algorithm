using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneticAlgorithm : MonoBehaviour
{
    //[SerializeField] GameObject _pipe;

    public NeuralNetwork neuralNetwork;
    float distanceX;
    float distanceY;
    public float weight = 0;
    float fitness;

    private void Awake()
    {
        neuralNetwork = gameObject.GetComponent<NeuralNetwork>();
    }
    private void Update()
    {
        SetInputLayers();

        float[] inputs = { distanceX, distanceY };
        weight = neuralNetwork.FeedForward(inputs);

        if (weight >= 0.5f)
        {
            Flap();  
        }
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
                distanceY = Mathf.Abs(gameObject.transform.position.y - pipeCenterTransform.position.y);

               //Debug.Log("x: " + distanceX);
               //Debug.Log("y: " + distanceY);
            }
        }
    }
   
    void Flap()
    {
        FlyBehavior birdJump = gameObject.GetComponent<FlyBehavior>();
        birdJump.Jump();
    }
}

   // public float creatureSpeed;
   // public float pathMultiplier;
   // int pathIndex = 0;
   // public DNA dna;
   // public bool hasFinished = false;
   // bool hasBeenInitialized = false;
   // Vector2 target;
   // Vector2 nextPoint;
   //
   // public void InitCreature(DNA newDna, Vector2 _target)
   // {
   //     dna = newDna;
   //     target = _target;
   //     nextPoint = transform.position;
   //     hasBeenInitialized = true;
   // }
   // private void Update()
   // {
   //     if (hasBeenInitialized && !hasFinished)
   //     {
   //         if (pathIndex == dna.genes.Count || Vector2.Distance(transform.position, target) < 0.5f)
   //         {
   //             hasFinished = true;
   //         }
   //         if ((Vector2)transform.position == nextPoint)
   //         {
   //             nextPoint = (Vector2)transform.position + dna.genes[pathIndex];
   //             pathIndex++;
   //         }
   //         else
   //         {
   //             transform.position = Vector2.MoveTowards(transform.position, nextPoint, creatureSpeed * Time.deltaTime);
   //         }
   //     }
   // }
   //
   // public float fitness
   // {
   //     get
   //     {
   //         float dist = Vector2.Distance(transform.position, target);
   //         if (dist == 0)
   //         {
   //             dist = 0.0001f;
   //         }
   //         return 60 / dist;
   //     }
   // }
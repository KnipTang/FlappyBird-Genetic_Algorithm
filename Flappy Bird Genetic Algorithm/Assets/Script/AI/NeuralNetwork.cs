using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class NeuralNetwork : MonoBehaviour
{
    public float fitnessScore { get; set; }
    public GameObject bird { get; set; }
    //Weight that gets added from an input node to an hidden node
    public float[] weightsInputToHidden;
    //Weight that gets added from an hidden node to an output node
    public float[] weightsHiddenToOutput;
    //Amount of input nodes
    public int inputLayerSize = 2;
    //Amount of hidden nodes
    public int hiddenLayerSize = 6;

    //Current Gen
    public int gen;
    public NeuralNetwork(float score, GameObject currentBird)
    {
        fitnessScore = score;
        bird = currentBird;

    }
    private void Awake()
    {
        //Idea Get gen. if gen is 0 get random weights. But if gen is 1 or more. Get weights 

        //Get current gen
        PopulationController populationController = FindAnyObjectByType<PopulationController>();
        gen = populationController.gen;

        //Total linkes between input nodes and hidden nodes -> 12
        weightsInputToHidden = new float[inputLayerSize * hiddenLayerSize];
        //Total linkes between hidden nodes and output nodes -> 6
        weightsHiddenToOutput = new float[hiddenLayerSize];

        //Initialize how much each link is gonna add to the total weight
        InitializeWeights();
    }
    public void NewInitializeWeights(float[] oldWeightsInputToHidden, float[] oldWeightsHiddenToOutput)
    {
        //Base every weight on the weights of the last gen best
        for (int i = 0; i < weightsInputToHidden.Length; i++)
        {
            weightsInputToHidden[i] = oldWeightsInputToHidden[i];
        }

        for (int i = 0; i < weightsHiddenToOutput.Length; i++)
        {
            weightsHiddenToOutput[i] = oldWeightsHiddenToOutput[i];
        }
    }
    private void InitializeWeights()
    {
        //Give every link a random weight
        for (int i = 0; i < weightsInputToHidden.Length; i++)
        {
            weightsInputToHidden[i] = Random.Range(-1f, 1f);
        }

        for (int i = 0; i < weightsHiddenToOutput.Length; i++)
        {
            weightsHiddenToOutput[i] = Random.Range(-1f, 1f);
        }
    }

    public float FeedForward(float[] inputs)
    {
        // Check input size
        //Debug.Log(inputs.Length + " " + inputLayerSize);
        if (inputs.Length != inputLayerSize)
        {
            Debug.LogError("Input size does not match the neural network input size.");
            return 0f; // or another suitable default value
        }

        //float representing how much each hidden node will cost -> 6 hidden nodes -> the cost of each node gets calculated using the 6 input parameters distance X/Y
        float[] hiddenLayerValues = new float[hiddenLayerSize];
        //Loop over all the hidden nodes (6)
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            //Set the weight of every hidden node to 0
            hiddenLayerValues[i] = 0;

            //Loop over every input node(2) for every hidden node
            for (int j = 0; j < inputLayerSize; j++)
            {
                //Add weight to the hidden node. Weights gets added to the hidden node 2 times because of the 2 input nodes.
                //The first for loop uses the distanceX * the weightInputToHidden float var thats linked to this link that goes from this input to this hidden node.
                hiddenLayerValues[i] += inputs[j] * weightsInputToHidden[i * inputLayerSize + j];
            }

            //Set the hidden node value to 0 if its lower than 0 for visual purposes
            hiddenLayerValues[i] = Mathf.Max(0, hiddenLayerValues[i]);
        }

        // Calculate output layer value
        //There is only 1 output node 
        float output = 0;
        //Loop over all the hidden nodes (6)
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            //Add the weight of every hidden node * by the weight of the link assosited with this hidden node to output node.
            output += hiddenLayerValues[i] * weightsHiddenToOutput[i];
        }

        // Apply sigmoid activation function to output
        //Keep output value between 0-1 for visual purposes
        output = 1 / (1 + Mathf.Exp(-output));

        return output;
    }
}

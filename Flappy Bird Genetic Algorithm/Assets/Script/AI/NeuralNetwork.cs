using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    public float[] weightsInputToHidden;
    public float[] weightsHiddenToOutput;

    private int inputLayerSize = 2;
    public int hiddenLayerSize = 6;
    private void Awake()
    {
        weightsInputToHidden = new float[inputLayerSize * hiddenLayerSize];
        weightsHiddenToOutput = new float[hiddenLayerSize];
        InitializeWeights();
    }
    private void InitializeWeights()
    {
        // Initialize weights randomly or using some other method
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
        if (inputs.Length != inputLayerSize)
        {
            Debug.LogError("Input size does not match the neural network input size.");
            return 0f; // or another suitable default value
        }

        // Calculate hidden layer values
        float[] hiddenLayerValues = new float[hiddenLayerSize];
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            hiddenLayerValues[i] = 0;
            for (int j = 0; j < inputLayerSize; j++)
            {
                hiddenLayerValues[i] += inputs[j] * weightsInputToHidden[i * inputLayerSize + j];
            }
            hiddenLayerValues[i] = Mathf.Max(0, hiddenLayerValues[i]); // ReLU activation function
        }

        // Calculate output layer value
        float output = 0;
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            output += hiddenLayerValues[i] * weightsHiddenToOutput[i];
        }

        // Apply sigmoid activation function to output
        output = 1 / (1 + Mathf.Exp(-output));

        return output;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PopulationController : MonoBehaviour
{
    List<GameObject> newBirdPopulation = new List<GameObject>();
    public List<GameObject> bestPopulation = new List<GameObject>();
    List<GameObject> allBirdPopulation = new List<GameObject>();
    public GameObject birdPrefab; //Includes a neuralNetwork/Genetic algo
    public int populationSize = 10;
    public int gen = 0;
    bool isPlaying = false;
    public Transform spawnPoint;

    //Mutation
    float mutationRate = 0.5f;
    float mutationAmount = 0.1f;

    public int deadBirds = 0;

    List<float[]> wieghtInputToHidden = new List<float[]>();
    List<float[]> weightHiddenToOutput = new List<float[]>();
    //0
    private void Awake()
    {
        InitPopulation();
    }

    //1
    void InitPopulation()
    {
        float minY = -4f;
        float maxY = 4f;
        isPlaying = false;
        for (int i = 0; i < populationSize; i++)
        {
            float yOffset = Random.Range(minY, maxY);
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + yOffset, spawnPoint.position.z);

            GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
            allBirdPopulation.Add(bird);
            Debug.Log("AllPopuCount: " + allBirdPopulation.Count);
        }
    }

    //2
    //If a birds dies take its fitness score/NeuralNetwork and add it to the birdPopulation
    public void AddBird(GameObject bird)
    {
        wieghtInputToHidden.Add(bird.GetComponent<NeuralNetwork>().weightsInputToHidden);
        weightHiddenToOutput.Add(bird.GetComponent<NeuralNetwork>().weightsHiddenToOutput);
        deadBirds++;
        CheckAllBirdsDead();
    }

    //3
    //Check if all birds if the gen are dead. -> If they are all dead call NextGen() and destroy the old gen 
    private void CheckAllBirdsDead()
    {
        if (deadBirds == populationSize)
            GetBestBirds();
    }

    //4
    //Get the 4 birds with the highest fitness score -> Put these birds into bestPopulation
    private void GetBestBirds()
    {
        // Sort the birdPopulation list by fitness score in descending order
        allBirdPopulation.Sort((a, b) => b.GetComponent<Score>().fitnessScore.CompareTo(a.GetComponent<Score>().fitnessScore));
        // Clear the bestPopulation list before adding new birds
        bestPopulation.Clear();

        // Select the top 4 birds (assuming birdPopulation.Count is at least 4)
        for (int i = 0; i < Mathf.Min(4, allBirdPopulation.Count); i++)
        {
            bestPopulation.Add(allBirdPopulation[i]);
        }

        if (!isPlaying)
        {
            if (bestPopulation[0].GetComponent<Score>().fitnessScore > 2f)
                InitNewPopulation();
            else
            {
                newBirdPopulation.Clear();
                deadBirds = 0;
                isPlaying = false;
                gen++;
                PipeSpawner.Instance.ResetLevel();
                InitPopulation();
            }
        }
    }

    //5
    //Create a new gen based on the 4 best birds of the old gen
    private void CreateNewGen()
    {
        deadBirds = 0;
        isPlaying = false;
        gen++;
        PipeSpawner.Instance.ResetLevel();

        //List<GameObject> newGeneration = new List<GameObject>();
        if (bestPopulation.Count > 1)
        {

            //10 bird crossover with the 2 best birds
            for (int i = 0; i < 2; i++)
            {
                NeuralNetwork network = CrossOver(bestPopulation[0].GetComponent<NeuralNetwork>(), bestPopulation[1].GetComponent<NeuralNetwork>());
                newBirdPopulation[i].GetComponent<NeuralNetwork>().NewInitializeWeights(network.weightsInputToHidden, network.weightsHiddenToOutput);
                newBirdPopulation[i].GetComponent<Renderer>().material.color = Color.red;
            }

            //25 bird crossover with the 2 best birds
            for (int i = 2; i < 7; i++)
            {
                int randomBestA = Random.Range(0, 3);
                int randomBestB = Random.Range(0, 3);
                NeuralNetwork network = CrossOver(bestPopulation[randomBestA].GetComponent<NeuralNetwork>(), bestPopulation[randomBestB].GetComponent<NeuralNetwork>());
                newBirdPopulation[i].GetComponent<NeuralNetwork>().NewInitializeWeights(network.weightsInputToHidden, network.weightsHiddenToOutput);
                newBirdPopulation[i].GetComponent<Renderer>().material.color = Color.blue;
            }

            //8 bird copy of the best bird
            for (int i = 7; i < 9; i++)
            {
                newBirdPopulation[i].GetComponent<NeuralNetwork>().NewInitializeWeights(bestPopulation[0].GetComponent<NeuralNetwork>().weightsInputToHidden, bestPopulation[0].GetComponent<NeuralNetwork>().weightsHiddenToOutput);
                newBirdPopulation[i].GetComponent<Renderer>().material.color = Color.black;
            }

            //7 bird copy of the second best bird
            for (int i = 9; i < 10; i++)
            {
                newBirdPopulation[i].GetComponent<NeuralNetwork>().NewInitializeWeights(bestPopulation[1].GetComponent<NeuralNetwork>().weightsInputToHidden, bestPopulation[1].GetComponent<NeuralNetwork>().weightsHiddenToOutput);
                newBirdPopulation[i].GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    //6
    private void InitNewPopulation()
    {
        newBirdPopulation.Clear();
        isPlaying = true;
        for (int i = 0; i < populationSize; i++)
        {
            GameObject newBird = Instantiate(birdPrefab, spawnPoint.position, Quaternion.identity);
            newBird.GetComponent<FlyBehavior>().wasCollided = false;
            allBirdPopulation.Add(newBird);
            newBirdPopulation.Add(newBird);
        }
        CreateNewGen();
    }

    private NeuralNetwork CrossOver(NeuralNetwork parentA, NeuralNetwork parentB)
    {
        NeuralNetwork childNN = new NeuralNetwork();

        // Perform crossover for input to hidden layer weights
        for (int i = 0; i < childNN.weightsInputToHidden.Length; i++)
        {
            // Crossover point (you can use a random point or a fixed point)
            float crossoverPoint = Random.Range(0f, 1f);

            // Use weights from parentA if crossoverPoint is less than 0.5, else use weights from parentB
            childNN.weightsInputToHidden[i] = crossoverPoint < 0.5f ? parentA.weightsInputToHidden[i] : parentB.weightsInputToHidden[i];
        }

        // Perform crossover for hidden to output layer weights
        for (int i = 0; i < childNN.weightsHiddenToOutput.Length; i++)
        {
            // Crossover point (you can use a random point or a fixed point)
            float crossoverPoint = Random.Range(0f, 1f);

            // Use weights from parentA if crossoverPoint is less than 0.5, else use weights from parentB
            childNN.weightsHiddenToOutput[i] = crossoverPoint < 0.5f ? parentA.weightsHiddenToOutput[i] : parentB.weightsHiddenToOutput[i];
        }

        ApplyMutation(childNN);

        return childNN;
    }

    private void ApplyMutation(NeuralNetwork nn)
    {
        // Apply mutation to input to hidden layer weights
        for (int i = 0; i < nn.weightsInputToHidden.Length; i++)
        {
            if (Random.Range(0f, 1f) < mutationRate)
            {
                // Mutate the weight by adding a random value within the mutationAmount range
                nn.weightsInputToHidden[i] += Random.Range(-mutationAmount, mutationAmount);
            }
        }

        // Apply mutation to hidden to output layer weights
        for (int i = 0; i < nn.weightsHiddenToOutput.Length; i++)
        {
            if (Random.Range(0f, 1f) < mutationRate)
            {
                // Mutate the weight by adding a random value within the mutationAmount range
                nn.weightsHiddenToOutput[i] += Random.Range(-mutationAmount, mutationAmount);
            }
        }
    }
}
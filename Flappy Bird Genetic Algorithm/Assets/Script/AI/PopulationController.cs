using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class PopulationController : MonoBehaviour
{
    List<BirdClass> birdPopulation = new List<BirdClass>();
    public List<BirdClass> bestPopulation = new List<BirdClass>();
    public GameObject birdPrefab;
    public int populationSize = 20;
    public int gen = 0;
    bool isPlaying = false;
    public Transform spawnPoint;

    //Mutation
    float mutationRate = 0.01f;
    float mutationAmount = 0.1f;
    void InitPopulation()
    {
        float minY = -4f;
        float maxY = 4f;
        isPlaying = false;
        for (int i = 0; i < populationSize; i++)
        {
           float yOffset = Random.Range(minY, maxY);
           Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + yOffset, spawnPoint.position.z);

           Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private void InitNewPopulation(List<BirdClass> generation)
    {
        isPlaying = false;

        float minY = -4f;
        float maxY = 4f;
        
        for (int i = 0; i < generation.Count; i++)
        {
            float yOffset = Random.Range(minY, maxY);
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + yOffset, spawnPoint.position.z);
        
            // Instantiate the bird prefab and set its neural network weights
            GameObject newBird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
            BirdClass birdClass = new BirdClass(0, newBird);
        
            // Copy the neural network weights from the new generation
            NeuralNetwork newNN = newBird.GetComponent<GeneticAlgorithm>().neuralNetwork;
            NeuralNetwork oldNN = generation[i].bird.GetComponent<GeneticAlgorithm>().neuralNetwork;
        
            // Copy input to hidden layer weights
            for (int j = 0; j < newNN.weightsInputToHidden.Length; j++)
            {
                newNN.weightsInputToHidden[j] = oldNN.weightsInputToHidden[j];
            }
        
            // Copy hidden to output layer weights
            for (int j = 0; j < newNN.weightsHiddenToOutput.Length; j++)
            {
                newNN.weightsHiddenToOutput[j] = oldNN.weightsHiddenToOutput[j];
            }
        
            // Add the new bird to the bird population
            birdPopulation.Add(birdClass);
        }
    }
    private void Awake()
    {
         InitPopulation();
    }
    private void NextGen()
    {
        // Sort the birdPopulation list by fitness score in descending order
        birdPopulation.Sort((a, b) => b.fitnessScore.CompareTo(a.fitnessScore));

        // Clear the bestPopulation list before adding new birds
        bestPopulation.Clear();

        // Select the top 4 birds (assuming birdPopulation.Count is at least 4)
        for (int i = 0; i < Mathf.Min(4, birdPopulation.Count); i++)
        {
            bestPopulation.Add(birdPopulation[i]);
        }
        //float totalFitnessScore = 0;
        //foreach (BirdClass bird in birdPopulation)
        //{
        //    totalFitnessScore += bird.fitnessScore;
        //}
        //
        //float avgFitnessScore = totalFitnessScore / birdPopulation.Count;
        //foreach (BirdClass bird in birdPopulation)
        //{
        //    //Debug.Log("BirdPopu: " + bird.bird);
        //    if (bird.fitnessScore > avgFitnessScore)
        //        bestPopulation.Add(bird);
        //}
        if (!isPlaying)
            Invoke("CreateNewGen", 0.1f);
    }
    public void AddBird(GameObject bird)
    {
        Score fitness = bird.GetComponent<Score>();
        BirdClass currentBird = new BirdClass(fitness.fitnessScore, bird);
        birdPopulation.Add(currentBird);
        //Debug.Log("BirdAdded");
        Invoke("CheckAllBirdsDead", 0.1f);
    }
    private void CheckAllBirdsDead()
    {
        int birdsAlife = 0;
        GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
        foreach (GameObject bird in birds)
        {
            FlyBehavior flyBehavior = bird.GetComponent<FlyBehavior>();
            if (flyBehavior.wasCollided == false)
                birdsAlife += 1;
            Debug.Log("Bird Alife");
        }
        if (birdsAlife == 0)
        {
            Debug.Log("All Dead");
            Invoke("DestroyOldGen", 3);
            NextGen();
        }
    }
    private void CreateNewGen()
    {
        isPlaying = true;
        gen++;
        PipeSpawner.Instance.ResetLevel();

        //// Create the next generation using crossover and mutation
       // List<BirdClass> newGeneration = new List<BirdClass>();
       // 
       // for (int i = 0; i < populationSize; i++)
       // {
       //     // Choose two parents randomly from the bestPopulation
       //     BirdClass parentA = bestPopulation[Random.Range(0, bestPopulation.Count)];
       //     BirdClass parentB = bestPopulation[Random.Range(0, bestPopulation.Count)];
       // 
       //     // Create a new bird by crossing over the parents
       //     BirdClass child = CrossOver(parentA, parentB);
       // 
       //     // Apply mutation to the child
       //     Mutate(child);
       // 
       //     // Add the child to the new generation
       //     newGeneration.Add(child);
       // }
       // 
       // // Clear the old bird population and add the new generation
       // //birdPopulation.Clear();
       // //birdPopulation.AddRange(newGeneration);

        //InitNewPopulation(newGeneration);
    }
   // private BirdClass CrossOver(BirdClass parentA, BirdClass parentB)
   // {
   //     // Create a new bird and copy its neural network weights from parents
   //     BirdClass child = new BirdClass(0, birdPrefab);
   //     NeuralNetwork childNN = child.bird.GetComponent<GeneticAlgorithm>().neuralNetwork;
   //     
   //     // Perform crossover for input to hidden layer weights
   //     for (int i = 0; i < childNN.weightsInputToHidden.Length; i++)
   //     {
   //         // Randomly choose a parent's weight for each gene
   //         childNN.weightsInputToHidden[i] = Random.Range(0f, 1f) < 0.5f ? parentA.bird.GetComponent<GeneticAlgorithm>().neuralNetwork.weightsInputToHidden[i] : parentB.bird.GetComponent<GeneticAlgorithm>().neuralNetwork.weightsInputToHidden[i];
   //     }
   //     
   //     // Perform crossover for hidden to output layer weights
   //     for (int i = 0; i < childNN.weightsHiddenToOutput.Length; i++)
   //     {
   //         // Randomly choose a parent's weight for each gene
   //         childNN.weightsHiddenToOutput[i] = Random.Range(0f, 1f) < 0.5f ? parentA.bird.GetComponent<GeneticAlgorithm>().neuralNetwork.weightsHiddenToOutput[i] : parentB.bird.GetComponent<GeneticAlgorithm>().neuralNetwork.weightsHiddenToOutput[i];
   //     }
   //     
   //     return child;
   // }
   //
   // private void Mutate(BirdClass bird)
   // {
   //     NeuralNetwork nn = bird.bird.GetComponent<GeneticAlgorithm>().neuralNetwork;
   //     
   //     // Mutate input to hidden layer weights
   //     for (int i = 0; i < nn.weightsInputToHidden.Length; i++)
   //     {
   //         if (Random.Range(0f, 1f) < mutationRate)
   //         {
   //             nn.weightsInputToHidden[i] += Random.Range(-mutationAmount, mutationAmount);
   //         }
   //     }
   //     
   //     // Mutate hidden to output layer weights
   //     for (int i = 0; i < nn.weightsHiddenToOutput.Length; i++)
   //     {
   //         if (Random.Range(0f, 1f) < mutationRate)
   //         {
   //             nn.weightsHiddenToOutput[i] += Random.Range(-mutationAmount, mutationAmount);
   //         }
   //     }
   // }
    private void DestroyOldGen()
    {
        birdPopulation.Clear();
        GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
        foreach (GameObject bird in birds)
        {
            FlyBehavior flyBehavior = bird.GetComponent<FlyBehavior>();
            if (flyBehavior.wasCollided == true)
                Destroy(bird);
        }
    }

    private void Update()
    {
        if(birdPopulation.Count > 0)
        {

        NeuralNetwork childNN = birdPopulation[0].bird.GetComponent<GeneticAlgorithm>().neuralNetwork;
        //Debug.Log(childNN.weightsHiddenToOutput);
        }
    }
}

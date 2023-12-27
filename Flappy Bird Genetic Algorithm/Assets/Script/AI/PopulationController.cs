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
    List<GameObject> currentBirdPopulation = new List<GameObject>();
    public List<GameObject> bestPopulation = new List<GameObject>();
    List<GameObject> allBirdPopulation = new List<GameObject>();
    [SerializeField]
    private TextMeshProUGUI _birdsAlifeLabel;
    public GameObject birdPrefab; //Includes a neuralNetwork/Genetic algo
    public int populationSize = 10;
    public int gen = 0;
    bool isPlaying = false;
    public Transform spawnPoint;

    //Mutation
    float mutationRate = 0.5f;
    float mutationAmount = 0.1f;

    float time = 0;

    int deadBirds = 0;

    List<float[]> wieghtInputToHidden = new List<float[]>();
    List<float[]> weightHiddenToOutput = new List<float[]>();
    //0
    private void Awake()
    {
        InitPopulation();
        //Invoke("CheckAllBirdsDead", 0.1f);
        _birdsAlifeLabel.text =
                        "Birds Alife: " + (populationSize - deadBirds).ToString() + '\n' + "Gen: " + gen.ToString();
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
        currentBirdPopulation.Add(bird);
        deadBirds++;
        CheckAllBirdsDead();
    }

    //3
    //Check if all birds if the gen are dead. -> If they are all dead call NextGen() and destroy the old gen 
    private void CheckAllBirdsDead()
    {

        if (bestPopulation.Count > 3)
        {
            _birdsAlifeLabel.text = "Birds Alife: " + (populationSize - deadBirds).ToString() + '\n' + "Gen: " + gen.ToString()
                + '\n' + "BestBird0: " + bestPopulation[0].GetComponent<Score>().fitnessScore
                + '\n' + "BestBird1: " + bestPopulation[1].GetComponent<Score>().fitnessScore
                + '\n' + "BestBird2: " + bestPopulation[2].GetComponent<Score>().fitnessScore
                + '\n' + "BestBird3: " + bestPopulation[3].GetComponent<Score>().fitnessScore;
        }
        //else
        //{
        //    _birdsAlifeLabel.text = "Birds Alife: " + birdsAlife.ToString() + '\n' + "Gen: " + gen.ToString();
        //}


        if (deadBirds == populationSize)
        {
            Debug.Log("All Dead");
            GetBestBirds();
            Invoke("DestroyOldGen", 3);
        }
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
        //Debug.Log(bestPopulation.Count);

        _birdsAlifeLabel.text =
                        "Birds Alife: " + (populationSize - deadBirds).ToString() + '\n' + "Gen: " + gen.ToString()
                + '\n' + "BestBird0: " + bestPopulation[0].GetComponent<Score>().fitnessScore
                + '\n' + "BestBird1: " + bestPopulation[1].GetComponent<Score>().fitnessScore
                + '\n' + "BestBird2: " + bestPopulation[2].GetComponent<Score>().fitnessScore
                + '\n' + "BestBird3: " + bestPopulation[3].GetComponent<Score>().fitnessScore;

        if (!isPlaying)
            InitNewPopulation();
    }

    //5
    //Create a new gen based on the 4 best birds of the old gen
    private void CreateNewGen()
    {
        deadBirds = 0;
        isPlaying = false;
        gen++;
        PipeSpawner.Instance.ResetLevel();

        int a = 0;
        a = populationSize * gen;


        int addedBirds = 0;
        //List<GameObject> newGeneration = new List<GameObject>();
        Debug.Log(bestPopulation.Count);
        if (bestPopulation.Count > 1)
        {

            //10 bird crossover with the 2 best birds
            for (int i = 0; i < 2; i++)
            {
                NeuralNetwork network = CrossOver(bestPopulation[0].GetComponent<NeuralNetwork>(), bestPopulation[1].GetComponent<NeuralNetwork>());
                allBirdPopulation[i + a].GetComponent<NeuralNetwork>().NewInitializeWeights(network.weightsInputToHidden, network.weightsHiddenToOutput);
                allBirdPopulation[i + a].GetComponent<Renderer>().material.color = Color.red;

                addedBirds++;
            }
           
            //25 bird crossover with the 2 best birds
            for (int i = 2; i < 7; i++)
            {
                int randomBestA = Random.Range(0, 3);
                int randomBestB = Random.Range(0, 3);
                NeuralNetwork network = CrossOver(bestPopulation[randomBestA].GetComponent<NeuralNetwork>(), bestPopulation[randomBestB].GetComponent<NeuralNetwork>());
                allBirdPopulation[i + a].GetComponent<NeuralNetwork>().NewInitializeWeights(network.weightsInputToHidden, network.weightsHiddenToOutput);
                allBirdPopulation[i + a].GetComponent<Renderer>().material.color = Color.blue;

                addedBirds++;

            }

            //8 bird copy of the best bird
            for (int i = 7; i < 9; i++)
            {
                allBirdPopulation[i + a].GetComponent<NeuralNetwork>().NewInitializeWeights(bestPopulation[0].GetComponent<NeuralNetwork>().weightsInputToHidden, bestPopulation[0].GetComponent<NeuralNetwork>().weightsHiddenToOutput);
                allBirdPopulation[i + a].GetComponent<Renderer>().material.color = Color.black;

                addedBirds++;
            }

            //7 bird copy of the second best bird
            for (int i = 9; i < 10; i++)
            {
                allBirdPopulation[i + a].GetComponent<NeuralNetwork>().NewInitializeWeights(bestPopulation[1].GetComponent<NeuralNetwork>().weightsInputToHidden, bestPopulation[1].GetComponent<NeuralNetwork>().weightsHiddenToOutput);
                allBirdPopulation[i + a].GetComponent<Renderer>().material.color = Color.white;

                addedBirds++;
            }
        }
    }

    //6
    private void InitNewPopulation()
    {
        currentBirdPopulation.Clear();
        isPlaying = true;
        for (int i = 0; i < populationSize; i++)
        {
            GameObject newBird = Instantiate(birdPrefab, spawnPoint.position, Quaternion.identity);
            newBird.GetComponent<FlyBehavior>().wasCollided = false;
            allBirdPopulation.Add(newBird);
            //allBirdPopulation.Add(newBird);
        }
        CreateNewGen();
    }
    private void DestroyOldGen()
    {
        //Debug.Log(allBirdPopulation.Count);
        GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
        foreach (GameObject bird in birds)
        {
            FlyBehavior flyBehavior = bird.GetComponent<FlyBehavior>();
            if (flyBehavior.wasCollided == true && bird.GetComponent<NeuralNetwork>().gen < gen)
            {
                 //Destroy(bird);
                 //currentBirdPopulation.Remove(bird);
            }
        }
       // Debug.Log(allBirdPopulation.Count);
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

    private void FixedUpdate()
    {
        //if (allBirdPopulation.Count > 0)
           // Debug.Log(allBirdPopulation[0].GetComponent<NeuralNetwork>().weightsInputToHidden[0]);
        //time += Time.deltaTime;
        //if (time == 0.1f)
        //{
        //    time = 0f;
        //    foreach (GameObject bird in allBirdPopulation)
        //    {
        //        try
        //        {
        //            if (bestPopulation.Count > 1)
        //            {
        //                //10 bird crossover with the 2 best birds
        //                for (int i = 0; i < 10; i++)
        //                {
        //                    NeuralNetwork network = CrossOver(bestPopulation[0].GetComponent<NeuralNetwork>(), bestPopulation[1].GetComponent<NeuralNetwork>());
        //                    bird.GetComponent<NeuralNetwork>().NewInitializeWeights(network.weightsInputToHidden, network.weightsHiddenToOutput);
        //                }
        //
        //                //25 bird crossover with the 2 best birds
        //                for (int i = 0; i < 25; i++)
        //                {
        //                    int randomBestA = Random.Range(0, 3);
        //                    int randomBestB = Random.Range(0, 3);
        //                    NeuralNetwork network = CrossOver(bestPopulation[randomBestA].GetComponent<NeuralNetwork>(), bestPopulation[randomBestB].GetComponent<NeuralNetwork>());
        //                    bird.GetComponent<NeuralNetwork>().NewInitializeWeights(network.weightsInputToHidden, network.weightsHiddenToOutput);
        //                }
        //
        //                //8 bird copy of the best bird
        //                for (int i = 0; i < 8; i++)
        //                {
        //                    bird.GetComponent<NeuralNetwork>().NewInitializeWeights(bestPopulation[0].GetComponent<NeuralNetwork>().weightsInputToHidden, bestPopulation[0].GetComponent<NeuralNetwork>().weightsHiddenToOutput);
        //                }
        //
        //                //7 bird copy of the best bird
        //                for (int i = 0; i < 7; i++)
        //                {
        //                    bird.GetComponent<NeuralNetwork>().NewInitializeWeights(bestPopulation[1].GetComponent<NeuralNetwork>().weightsInputToHidden, bestPopulation[1].GetComponent<NeuralNetwork>().weightsHiddenToOutput);
        //                }
        //            }
        //        }
        //        catch
        //        {
        //
        //        }
        //
        //    }
        //}

        // for (int i = 0; i < newGeneration.Count; i++)
        // {
        //     NeuralNetwork network = CrossOver(bestPopulation[0].GetComponent<NeuralNetwork>(), bestPopulation[1].GetComponent<NeuralNetwork>());
        //
        //     newGeneration[i].GetComponent<NeuralNetwork>().NewInitializeWeights(network.weightsInputToHidden, network.weightsHiddenToOutput);
        // }
    }


    // private void InitNewPopulation(List<NeuralNetwork> generation)
    // {
    //     isPlaying = false;
    //
    //     float minY = -4f;
    //     float maxY = 4f;
    //     
    //     for (int i = 0; i < generation.Count; i++)
    //     {
    //         float yOffset = Random.Range(minY, maxY);
    //         Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + yOffset, spawnPoint.position.z);
    //     
    //         // Instantiate the bird prefab and set its neural network weights
    //         GameObject newBird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
    //         NeuralNetwork NN = new NeuralNetwork(0, newBird);
    //     
    //         // Copy the neural network weights from the new generation
    //         NeuralNetwork newNN = newBird.GetComponent<GeneticAlgorithm>().neuralNetwork;
    //         NeuralNetwork oldNN = generation[i].bird.GetComponent<GeneticAlgorithm>().neuralNetwork;
    //     
    //         // Copy input to hidden layer weights
    //         for (int j = 0; j < newNN.weightsInputToHidden.Length; j++)
    //         {
    //             newNN.weightsInputToHidden[j] = oldNN.weightsInputToHidden[j];
    //         }
    //     
    //         // Copy hidden to output layer weights
    //         for (int j = 0; j < newNN.weightsHiddenToOutput.Length; j++)
    //         {
    //             newNN.weightsHiddenToOutput[j] = oldNN.weightsHiddenToOutput[j];
    //         }
    //     
    //         // Add the new bird to the bird population
    //         birdPopulation.Add(NN);
    //     }
    // }
    // private void CreateNewGen()
    // {
    //     isPlaying = true;
    //     gen++;
    //     PipeSpawner.Instance.ResetLevel();
    //
    //     //// Create the next generation using crossover and mutation
    //     List<BirdClass> newGeneration = new List<BirdClass>();
    //     
    //     for (int i = 0; i < populationSize; i++)
    //     {
    //         // Choose two parents randomly from the bestPopulation
    //         BirdClass parentA = bestPopulation[Random.Range(0, bestPopulation.Count)];
    //         BirdClass parentB = bestPopulation[Random.Range(0, bestPopulation.Count)];
    //         Debug.Log("pA: " + parentA + " pB: " + parentB);
    //         // Create a new bird by crossing over the parents
    //         BirdClass child = CrossOver(parentA, parentB);
    //     
    //         // Apply mutation to the child
    //         Mutate(child);
    //     
    //         // Add the child to the new generation
    //         newGeneration.Add(child);
    //     }
    //     
    //     // Clear the old bird population and add the new generation
    //     //birdPopulation.Clear();
    //     //birdPopulation.AddRange(newGeneration);
    //
    //     InitNewPopulation(newGeneration);
    // }

    //Pass 2 of the best birds to create a crossover bird out of these 2 birds.
    //
    //private void Mutate(BirdClass bird)
    //{
    //    NeuralNetwork nn = bird.bird.GetComponent<GeneticAlgorithm>().neuralNetwork;
    //    
    //    // Mutate input to hidden layer weights
    //    for (int i = 0; i < nn.weightsInputToHidden.Length; i++)
    //    {
    //        if (Random.Range(0f, 1f) < mutationRate)
    //        {
    //            nn.weightsInputToHidden[i] += Random.Range(-mutationAmount, mutationAmount);
    //        }
    //    }
    //    
    //    // Mutate hidden to output layer weights
    //    for (int i = 0; i < nn.weightsHiddenToOutput.Length; i++)
    //    {
    //        if (Random.Range(0f, 1f) < mutationRate)
    //        {
    //            nn.weightsHiddenToOutput[i] += Random.Range(-mutationAmount, mutationAmount);
    //        }
    //    }
    //}

    //  private void Update()
    //  {
    //      if(birdPopulation.Count > 0)
    //      {
    //
    //      NeuralNetwork childNN = birdPopulation[0].bird.GetComponent<GeneticAlgorithm>().neuralNetwork;
    //      //Debug.Log(childNN.weightsHiddenToOutput);
    //      }
    //  }
}

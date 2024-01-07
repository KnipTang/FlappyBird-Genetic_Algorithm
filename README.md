# Flappy Bird - Genetic Algorithm
## Introduction
For my research project, I tried to create a genetic algorithm that could perfectly play the mobile game Flappy Bird. I used Unity to recreate Flappy Bird and the implementation of the algorithm.

![](https://github.com/Howest-DAE-GD/gpp-researchtopic-KnipTang/blob/main/GIF.gif)

### Flappy Bird

Flappy Bird is a fairly simple game where the player controls a bird by flapping. Flapping results in the bird moving up and not flapping leads to the bird moving down. The bird needs to pass between holes of the incoming pipes. By passing between a pipe, the player's score will increase by one. Colliding between the bird and the ground, roof or one of the pipes will lead to the game ending.
### Genetic Algorithm
A genetic algorithm tries to mimic the evolution theory through the process of natural selection, crossover and mutation. Every new generation gets better by learning from the best individuals of the past generations. This process keeps getting repeated until a solution is found. 
## Implementation

### Neural Network
Every bird has a neural network. This neural network consists of an input layer, a hidden layer and an output layer.

The input layer has two nodes. The first node takes the length between the x value of the bird and the x value of the center of the incoming pipes. The second node does the same but for the y value.

These values get passed to the 6 hidden nodes. These nodes take the values of the input nodes and calculate/multiply them with the bird's unique "weightsInputToHidden" and "weightsHiddenToInput" values. These unique values decide how well the bird will perform. The closer these values are to the final solution the better the bird performs. These values are the weight matrices or DNA of the bird.

After the calculations, the hidden nodes pass a final value to the 1 output node. The value of the output node is the weight of the bird at that moment. This weight gets recalculated every frame. If the weight is higher than a certain threshold the bird flaps. Otherwise, the bird does nothing and falls down until the threshold is met.

![App Screenshot](https://github.com/Howest-DAE-GD/gpp-researchtopic-KnipTang/blob/main/NeuralNetwork.PNG)
### Generations
Every generation has a set population size. This size will decide how many birds will spawn at the beginning of each generation. In the first generation, all the bird's unique DNA values will be randomized between certain minimum and maximum values.

After the first generation dies out and no solution was found in that generation, a new generation will be created from the DNA of the birds with the highest fitness score of the last generation. A fitness score indicates how well a bird has performed. The fitness score in this case is the time a bird survived.

The offspring for the new generation will be created by selecting the best birds and using crossover and mutation techniques on the DNA of the birds to create variants of these birds. This process will be repeated until a bird with DNA that can solve the problem has been created.

#### Crossover
Crossover is the process of combining the DNA from two-parent birds to create offspring. by selecting random crossover points along the DNA of the parents and exchanging genetic information between those points.

![App Screenshot](https://github.com/Howest-DAE-GD/gpp-researchtopic-KnipTang/blob/main/Crossover.PNG)
#### Mutation
Mutation introduces small random changes in the DNA of an individual to maintain genetic diversity. In the context of this neural network, mutation involves randomly adjusting the DNA of the newly created offspring. A mutation rate determines the likelihood of offspring undergoing mutation.

![App Screenshot](https://github.com/Howest-DAE-GD/gpp-researchtopic-KnipTang/blob/main/Mutation.PNG)

## Result
After multiple generations of the genetic algorithm, the algorithm was able to produce birds that demonstrated improved performance every new generation. At an average of only 4 generations, the system can create a bird that plays the game flawlessly. The process of natural selection, crossover, and mutation contributed to the generation of birds with increasingly optimized neural network weights.

## Future
To further improve the algorithm, consider expanding the neural network's input nodes to include factors such as the distance between the bird and the upper/lower pipes instead of just keeping the distance between the center in mind. This additional information could expedite the convergence towards the desired solution.

![App Screenshot](https://github.com/Howest-DAE-GD/gpp-researchtopic-KnipTang/blob/main/Future.PNG)

## References
https://www.geeksforgeeks.org/genetic-algorithms/
https://theailearner.com/2018/11/09/snake-game-with-genetic-algorithm/
https://techs0uls.wordpress.com/2020/02/03/teaching-ai-to-play-snake-with-genetic-algorithm/
https://medium.com/@christian_deveaux/a-genetic-algorithm-solving-flappy-bird-using-data-science-87bcd981cefd
https://www.askforgametask.com/tutorial/machine-learning/machine-learning-algorithm-flappy-bird/

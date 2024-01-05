
# Flappy Bird - Genetic Algorithm

For my research project, I tried to create a genetic algorithm that could perfectly play the mobile game Flappy Bird. I used Unity to recreate Flappy Bird and the implementation of the algorithm.

![](https://github.com/Howest-DAE-GD/gpp-researchtopic-KnipTang/blob/main/GIF.gif)

## Flappy Bird

Flappy Bird is a fairly simple game where the player controls a bird by flapping resulting in the bird moving up or not flapping resulting in the bird moving down. The bird needs to pass between holes of the incoming pipes. Passing between a pipe will result in the player's score being increased by one. Colliding between the bird and the ground, roof or one of the pipes will result in ending the game.
## Genetic Algorithm
A genetic algorithm tries to mimic the evolution theory through the process of natural selection, crossover and mutation. Every new generation gets better by learning from the best individuals of the past generations. This process keeps getting repeated until a solution is found. 
## Implementation

### Neural Network
Every bird has a neural network. This neural network consists of an input layer, a hidden layer and an output layer.

The input layer has 2 nodes. The first node takes the length between the x value of the bird and the x value of the center of the incoming pipes. The second node does the same but for the y value.

These values get passed to the 6 hidden nodes. These nodes take the values of the input nodes and calculate/multiply them with the bird's unique "weightsInputToHidden" and "weightsHiddenToInput" values. These unique values decide how well the bird will perform. The closer these values are to the final solution the better the bird performs. This is the weight matrices or DNA of the bird.

After the calculations, the hidden nodes pass a final value to the 1 output node. The value of the output node is the weight of the bird at that moment. This weight gets recalculated every frame. If the weight is higher than a certain threshold the bird flaps. Otherwise, the bird does nothing and falls down until the threshold is met.

![App Screenshot](https://github.com/Howest-DAE-GD/gpp-researchtopic-KnipTang/blob/main/NeuralNETWORK.png)
### Generations
Every generation has a set population size. This size will decide how many birds will spawn at the beginning of each generation. In the first generation, all the bird's unique DNA values will be randomized between certain minimum and maximum values.

After the first generation dies out and no solution was found in that generation, a new generation will be created from the DNA of the birds with the highest fitness score of the last generation. A fitness score indicates how well a bird has performed. The fitness score in this case the time a bird survived.

The offspring for the new generation will created by selecting the best birds and using crossover and mutation techniques on the DNA of the birds to create variants of these birds. This process will be repeated until a bird with DNA that can solve the problem has been created.

#### crossover
Crossover is the process of combining the DNA from two-parent birds to create offspring. In the context of the neural network, crossover involves selecting random crossover points along the DNA of the parents and exchanging genetic information between those points.

#### mutation
Mutation introduces small random changes in the DNA of an individual to maintain genetic diversity. In the context of the neural network, mutation involves randomly adjusting the weights.
A mutation rate decides how big the chance is that an offspring will mutate.


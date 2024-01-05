
# Flappy Bird - Genetic Algorithm

For my research project I tried to create a genetic algorithm that could perfectly play the mobile game flappy bird. I used unity for the recreating of flappy bird and the implementasion of the algorithm.

## Flappy Bird

Flappy Bird is a fairly simple game were the player controls a bird by flapping resulting in the bird moving up or not flapping resulting in the bird moving down. The bird needs to pass between holes of incoming pipes. Passing a pipe will result in the players score being increased by 1. Collision between the bird and ground, roof or one of the pipes will result in the game ending.
## Genetic Algorithm
A genetic algorithm tries to mimic the evolution theory by the process of natural selection, crossover and mutation. Every new generation gets better by learning from the best individuals of the past generations. This process keeps getting repeated until a solution is found. 
## Implementation

### Neural Network
Every bird has a neural netwerk. This neural netwerk consist of a input layer, hidden layer, output layer.

The input layer has 2 nodes. The first node takes the length between the x value of the bird and the x value of the center if the incoming pipes. The second node does the same but for the y value.

These values gets passed to the 6 hidden nodes. These nodes take the values of the input nodes and calculates/multiply them with the birds unique "weightsInputToHidden" and "weightsHiddenToInput" values. These unique values decide how well the bird will preform. The closer these values are to the final solution the better the bird preforms. This is the weights matrices or DNA of the bird.

After the calculations the hidden nodes passes a final value to the 1 output node. The value of the output node is the weight of the bird at that moment. This weight gets recalculated every frame. If the weight is higher than a certain threshold the bird flaps. Otherwise the bird does nothing and falls down until the threshold is met.

### Generations
Every generations has a set population size. This size will decide how many birds will spawn at the beginning of each generation. In the first generation all the birds unique DNA values will be randomised between a certaint min and max values.

After the first generation died out and no solution was found a new generation will be created from the DNA of the birds with the highest fitness score of the last generation. A fitness score indicates how will a bird has preformed. The fitness score in this case the time a bird survived.

The offspring for the new generation will created by selecting the best birds and use crossover and mutation techniques on the DNA of the birds to create variants of these birds. This process will be repeated until a bird with DNA that can solve the problem has been created.

#### crossover
Crossover is the process of combining the DNA from two parent birds to create the offspring. In the context of the neural network, crossover involves selecting random crossover points along the DNA of the parents and exchanging genetic information between those points.

#### mutation
Mutation introduces small random changes in the DNA of an individual to maintain genetic diversity. In the context of the neural network, mutation involves randomly adjusting the weights.
A mutationrate decides how big the chance is that a offspring will mutate.
## Screenshots

![App Screenshot](https://via.placeholder.com/468x300?text=App+Screenshot+Here)


# IteratedPrisonerDilemma

Visual simulation of iterated prisoner dilemma.

developed and tested on Mac Os X:

Xamarin Studio Community version 6.01
Mono C# compiler version 4.4.1.0
Monogame assembly version: 3.3.0.2293


"animals" move randomly across the window and when an animal meets another one an Iterated Prisoner Dilemma game starts. Each player use the strategy of the type it belong. 

Five strategies are currently implemented: Cooperator, Defeater, Random, Tit for Tat, Tif for Two Tats.
Other strategies can be added using delegates in the AnimalCreator.cs class.

The payoff matrix is code as a hashtable in the Constants.cs class which contains other parameters.

Few unit tests are limited to test single strategies "out of the box". 

In the right panel of the main window the cumulative score for each animal type is displayed.

From time to time the animal with less score mutates into another type with probabilities given by the replicator dynamic equation (i.e. product of score and number of member of each type determine the probability of mutating to that type)


Planning to add features like:

- adding a control panel to select which strategies (AnimalTypes) to use before starting any simulation 
- making the animals with lower score "die" or "mutate" into other "stronger" types
- generating text files with the result of the simulation that can be used to draw graphs using external tools (done)
- diplay the average score per animal type. (done)



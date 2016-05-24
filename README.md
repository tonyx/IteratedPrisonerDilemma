# IteratedPrisonerDilemma

Visual simulation of iterated prisoner dilemma.
"pets" with different color have different strategies.
They move randomly across the window and when they meet another pet they start playing each other the prisoner dilemma according
to the strategy of the "type" they belong.

Four strategies are currently implemented: Cooperator, Defeater, Random, Tit for Tat.
Other strategies can be added directly in the AnimalCreator class.
(the source code should be easy to follow: just note that AnimalType needs a "MoveStrategy" which is a delegate, and an image. 
Any animal instance just need to know which AnimalType it belongs to).
The payoff matrix is code as a hashtable in the Constants class which contains many useful parameters.

Few unit tests are limited to test single strategies "out of the box". 

In the right panel of the main window the cumulative score for each animal type is displayed.

Evolution:
From time to time some animals change type with probabilities given by the replicator dynamic equation (i.e. product of score and number of member of any type to determine the probability of becoming of that type)


Planning to add features like:

- adding new strategies
- adding a control panel to select which strategies (AnimalTypes) to use before starting any simulation 
- making the animals with lower score "die" or "mutate" into other "stronger" types
- generating text files with the result of the simulation that can be used to draw graphs using external tools (done)
- diplay the average score per animal type. (done)



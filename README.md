# Evolutionary Container Packing

Evolutionary Container Packing is an application for solving 3D container packing problems using evolutionary algorithms. The project provides both a graphical user interface (WinForms) and a configuration-based command line mode.

The goal of the application is to **minimize the number of containers required to pack a given set of objects**, while respecting spatial and weight constraints. The solver combines heuristic packing rules with evolutionary search strategies to improve packing quality over multiple generations.

---

# Overview

Container packing is a complex combinatorial optimization problem where objects must be placed into containers while respecting spatial constraints.

In this project, the objective is to **pack all objects using as few containers as possible**. Since the problem is NP-hard, heuristic and evolutionary approaches are used to search for high-quality packing configurations.

The system works by evolving **packing strategies and heuristic combinations**, evaluating their performance using a fitness function based on container utilization and container count, and iteratively improving them through evolutionary operators such as mutation and selection.

The application supports:

* heuristic-based packing
* evolutionary optimization
* optional rotation of packed objects
* configurable packing order heuristics
* export of evolution statistics

The system can be executed in two ways:

1. Using a **graphical configuration interface (WinForms)**
2. Using a **JSON configuration file**

---


# Implemented Algorithms

The project currently includes two evolutionary optimization algorithms.

## Elitist Genetic Algorithm

The Elitist Genetic Algorithm maintains a population of candidate packing strategies and evolves them across multiple generations. The algorithm combines **elitism, crossover, mutation, and random population injection**, and optionally applies a **memetic (local search) improvement** step.

Key features:

* **elitism** – the best individuals are preserved between generations
* **memetic improvement** – elite individuals can be locally improved using hill climbing
* **crossover between elite and non-elite individuals**
* **mutation-based variation**
* **random individuals introduced each generation to maintain diversity**

### Evolutionary Iteration

Each generation is produced using the following steps:

1. **Elite selection**
   The best individuals in the population are selected and preserved as *elites*.

2. **Non-elite selection**
   The remaining individuals form a non-elite pool.

3. **Memetic improvement**
   Elite individuals may be locally improved using a hill climbing operator.

4. **Parent selection**
   Individuals are randomly selected from both the elite and non-elite groups.

5. **Crossover**
   Selected elite and non-elite individuals are combined using a crossover operator to produce offspring.

6. **Mutation**
   The generated offspring are mutated to introduce additional variation.

7. **Random population injection**
   A number of completely new random individuals are generated to preserve diversity.

8. **Fitness evaluation and population update**
   Newly generated individuals are evaluated and combined with the preserved elites to form the next generation.

This approach balances **exploitation of high-quality solutions (elitism and memetic improvement)** with **exploration of the search space (mutation and random individuals)**.


---

## Probabilistic Hill Climbing

The Probabilistic Hill Climbing algorithm is a population-based local search method that iteratively improves candidate solutions through mutation.

Although this method is **not a genetic algorithm**, similar terminology such as *population* and *generation* is used for consistency with the rest of the system architecture. Conceptually, the algorithm performs **parallel hill climbing**, where each individual in the population independently explores its neighborhood through mutation.

Unlike traditional hill climbing, the algorithm may **accept worse solutions with a certain probability**, which helps avoid getting trapped in local optima. This acceptance probability gradually decreases during the search, creating an effect similar to **simulated annealing**.

Key features:

* population-based local search
* mutation-driven neighborhood exploration
* probabilistic acceptance of worse solutions
* gradually decreasing acceptance probability (annealing effect)
* lower computational overhead compared to genetic algorithms

### Iteration Process

Each generation is produced as follows:

1. **Mutation**
   Every individual in the population is mutated to generate a candidate neighbor solution.

2. **Fitness evaluation**
   The mutated individuals are evaluated using the fitness function.

3. **Probabilistic acceptance**
   For each individual, the mutated candidate replaces the current one if:

   * it has **better fitness**, or
   * it is **accepted with a certain probability** even if it is worse.

4. **Cooling schedule**
   The probability of accepting worse solutions gradually decreases over time according to a decay factor, until a minimal probability is reached.

This strategy allows the algorithm to **explore the search space early in the evolution** while gradually shifting toward **greedy improvement** as the acceptance probability decreases.

---

# Configuration Parameters

The program is configured using a `ProgramSetting` structure.

## Input / Output

| Parameter                 | Description                                                |
| ------------------------- | ---------------------------------------------------------- |
| `SourceFile`              | Path to the input JSON file containing the packing problem |
| `OutputFile`              | Path where the resulting packing solution will be saved    |
| `EvolutionStatisticsFile` | Optional CSV file where evolution statistics are exported  |

---

## Packing Settings

These parameters define the packing constraints.

| Parameter                     | Description                                                       |
| ----------------------------- | ----------------------------------------------------------------- |
| `SelectedPlacementHeuristics` | List of heuristics used to determine where a box should be placed |
| `AllowRotations`              | Allows boxes to be rotated when searching for a valid placement   |
| `PackingOrder`                | Optional heuristic defining the order in which boxes are packed   |

If `PackingOrder` is **not specified**, the order of boxes is **determined by the evolutionary algorithm**. In this case, the algorithm evolves different packing orders as part of the search process.

---

## Placement Heuristics

Placement heuristics determine **which empty region inside containers should be used for placing the next box**.
Each heuristic evaluates candidate empty regions and selects the one that best satisfies its strategy.

The following heuristics are implemented:

| Heuristic          | Description                                                                                                     |
| ------------------ | --------------------------------------------------------------------------------------------------------------- |
| `Best Volume Fit`  | Chooses the region with the **smallest volume** that can still fit the box, minimizing leftover space.          |
| `Max Distance`     | Chooses the placement **farthest from the container's end corner**, spreading boxes across the container.       |
| `Min Distance`     | Chooses the placement **closest to the container origin**, filling the container from the origin outward.       |
| `Min X`            | Chooses the region with the **smallest X coordinate**, filling the container from one side along the X axis.    |
| `Min Y`            | Chooses the region with the **smallest Y coordinate**, prioritizing placements along the Y axis.                |
| `Gravity`          | Chooses the placement with the **lowest Z coordinate**, simulating gravity by placing boxes as low as possible. |
| `Anti-Gravity`     | Chooses the placement with the **highest Z coordinate**, filling containers from the top downward.              |
| `Bottom Left Back` | A classical packing rule selecting the position with **minimal Z, then minimal Y, then minimal X**.             |

Multiple heuristics can be selected simultaneously. During packing, the system applies the available heuristics when evaluating candidate placements.

---


## Evolutionary Algorithm Settings

Common parameters for evolutionary algorithms:

| Parameter             | Description                       |
| --------------------- | --------------------------------- |
| `TargetFitness`       | Optional early stopping condition |
| `NumberOfIndividuals` | Population size                   |
| `NumberOfGenerations` | Number of evolutionary iterations |

Additional parameters for the **Elitist Genetic Algorithm**:

| Parameter                                 | Description                                |
| ----------------------------------------- | ------------------------------------------ |
| `PercentageOfEliteIndividuals`            | Portion of population preserved as elite   |
| `PercentageOfElementsFromElite`           | Portion of individual inherited from elite |
| `PercentageOfElementsMutated`             | Mutation rate                              |
| `PercentageOfRandomIndividuals`           | Portion of population generated randomly   |
| `HillClimbingIterations`                  | Number of local search iterations          |
| `HillClimbingPercentageOfElementsMutated` | Mutation rate used during hill climbing    |

Additional parameters for **Probabilistic Hill Climbing**:

| Parameter                     | Description                                         |
| ----------------------------- | --------------------------------------------------- |
| `PercentageOfElementsChanged` | Percentage of elements modified to create neighbors |
| `StartAcceptanceProbability`  | Initial probability of accepting worse solutions    |
| `EndAcceptanceProbability`    | Final probability of accepting worse solutions      |
| `AcceptanceDecayFactor`       | Rate at which acceptance probability decreases      |

Each iteration the current acceptance probability is multiplied by `AcceptanceDecayFactor`, until it reaches `EndAcceptanceProbability`. 

---

# Running the Application

The application can be executed either with a graphical configuration interface or using a configuration file.

---

# Option 1 — Run with GUI

The graphical interface allows users to configure all parameters interactively.

Steps:

1. Start the application
2. Select the input JSON file
3. Configure packing settings
4. Select the evolutionary algorithm
5. Configure algorithm parameters
6. Run the solver

---

# Option 2 — Run with JSON Configuration

The program can also be executed using a configuration file.

Example command:

```bash
dotnet run --project src/App config.json
```

---

# Example Configuration File

Example `config.json`:

```json
{
  "SourceFile": "input.json",
  "OutputFile": "output.json",
  "EvolutionStatisticsFile": "statistics.csv",

  "PackingSetting": {
    "SelectedPlacementHeuristics": ["Max Distance"],
    "AllowRotations": true,
    "PackingOrder": null
  },

  "EvolutionAlgorithmSetting": {
    "AlgorithmName": "Elitist Genetic",
    "TargetFitness": null,
    "NumberOfIndividuals": 100,
    "NumberOfGenerations": 200,
    "PercentageOfEliteIndividuals": 0.1,
    "PercentageOfElementsFromElite": 0.7,
    "PercentageOfElementsMutated": 0.05,
    "PercentageOfRandomIndividuals": 0.15,
    "HillClimbingIterations": 0,
    "HillClimbingPercentageOfElementsMutated": 0.05
  }
}
```

---

# Installing and Running

## Download as ZIP

1. Go to the repository page
2. Click **Code → Download ZIP**
3. Extract the archive
4. Open the solution in **Visual Studio**
5. Build and run the project

---

## Clone from GitHub

```bash
git clone https://github.com/JanBurda03/EvolutionaryContainerPacking.git
cd EvolutionaryContainerPacking
```

Build the project:

```bash
dotnet build
```

Run the application:

```bash
dotnet run --project src/App
```

---

# Project Structure

```
src/
 ├─ App/                 Application entry point and GUI
 ├─ Evolution/           Evolutionary algorithms and architecture
 ├─ Packing/             Packing rules and problem representation
scripts/                 Dataset generation utilities
data/                    Generated datasets and experiment inputs
examples/                Examples of input and output files
tests/                   Unit tests
```

---

# Dataset and Generation Scripts

The repository also contains scripts for generating datasets used in experiments.

## Dataset Location

Generated datasets are stored in the `/data` directory, typically under:

```
data/mpv/
```

This directory contains packing problem instances organized by benchmark class and instance size.

---

## Dataset Generation Scripts

The `/scripts` directory contains Python scripts used to automatically generate packing datasets.

These scripts include:

* `generate_mpv_benchmark.py` — main script that generates the full MPV-style benchmark dataset
* `mpv_generator.py` — core implementation responsible for creating a packing instances

The scripts generate randomized packing problems with different container sizes, box dimensions, and weight distributions.

---

## Running the Dataset Generator

From the repository root run:

```bash
python3 scripts/generate_mpv_benchmark.py --output data/mpv
```

---

## Generated Dataset Structure

After generation the dataset will have a structure similar to:

```
data/mpv/
  class1/
    n50/
      instance_01.json
      instance_02.json
    n100/
      ...
  class2/
  ...
```

Each file represents a packing instance containing container properties and a list of box properties.

---

## Example Generated Instance

```json
{
  "ContainerProperties": {
    "Sizes": { "X": 100, "Y": 100, "Z": 100 },
    "MaxWeight": 500000
  },
  "BoxPropertiesList": [
    { "ID": 0, "Sizes": { "X": 20, "Y": 10, "Z": 5 }, "Weight": 123 },
    { "ID": 1, "Sizes": { "X": 5, "Y": 5, "Z": 5 }, "Weight": 10 }
  ]
}
```

This format matches the structure expected by the application's packing input loader.

---

# Output

The program produces:

1. **Packing solution (JSON)**
   Contains the resulting container packing configuration.

2. **Evolution statistics (CSV)**
   Optional statistics describing the evolution process.

These statistics can be used for analyzing algorithm performance or plotting convergence graphs.

---

# License

This project is licensed under the MIT License.

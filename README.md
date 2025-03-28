# Introduction

logicGP is a generalization of the [Genetic Programming for Association Studies (GPAS)](https://doi.org/10.1093/bioinformatics/btm522) algorithm. It is currently unpublished, first results can be seen on published [slides](https://isd-nunkesser.github.io/slides/ISDBlackHighlyInterpretablePredictionModels.html#/logicgp).
It is intended to be used for all classification tasks. Currently, it is restricted to categorical features. 

The implementation mainly uses the following NuGet packages:

- [Italbytz.Ports.Algorithms.AI](https://www.nuget.org/packages/Italbytz.Ports.Algorithms.AI) (Source: [nuget-ports-algorithms-ai](https://github.com/Italbytz/nuget-ports-algorithms-ai))
- [Italbytz.Adapters.Algorithms.AI](https://www.nuget.org/packages/Italbytz.Adapters.Algorithms.AI) (Source: [nuget-adapters-algorithms-ai](https://github.com/Italbytz/nuget-adapters-algorithms-ai))

The algorithm is intended to be compatible with Microsoft's [ML.NET](https://dotnet.microsoft.com/en-us/apps/ai/ml-dotnet).

# Getting started

The project features unit tests as a starting point. The most important unit tests show the application of the algorithm to example data sets.

## Simulated data sets

|Data set| Code|
| ------ | :---|
|Simulation from Section 3.3 of [Detecting high-order interactions of single nucleotide polymorphisms using genetic programming](https://doi.org/10.1093/bioinformatics/btm522)||
|Simulation from Equation (2) of [Evaluation of tree‑based statistical learning methods for constructing genetic risk scores](https://doi.org/10.1186/s12859-022-04634-w)||
|Simulation from Equation (3) of [Evaluation of tree‑based statistical learning methods for constructing genetic risk scores](https://doi.org/10.1186/s12859-022-04634-w)||

## Real data sets

Chosen data sets from the UC Irvine Machine Learning Repositorz

|Data set| Code|
| ------ | :---|
|[National Poll on Healthy Aging (NPHA)](https://archive.ics.uci.edu/dataset/936/national+poll+on+healthy+aging+(npha))||
|[Car Evaluation](https://archive.ics.uci.edu/dataset/19/car+evaluation)||
|[Balance Scale](https://archive.ics.uci.edu/dataset/12/balance+scale)||
|[Solar Flare](https://archive.ics.uci.edu/dataset/89/solar+flare)||
|[Lenses](https://archive.ics.uci.edu/dataset/58/lenses)||


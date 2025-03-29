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
|Simulation from Section 3.3 of [Detecting high-order interactions of single nucleotide polymorphisms using genetic programming](https://doi.org/10.1093/bioinformatics/btm522)|[SNPSimulationTests](/logicGP/logicGP.Tests/Unit/Data/Simulated/SNPSimulationTests.cs)|
|Simulation from Equation (2) of [Evaluation of tree‑based statistical learning methods for constructing genetic risk scores](https://doi.org/10.1186/s12859-022-04634-w)|[SNPSimulationTests](/logicGP/logicGP.Tests/Unit/Data/Simulated/SNPSimulationTests.cs)|
|Simulation from Equation (3) of [Evaluation of tree‑based statistical learning methods for constructing genetic risk scores](https://doi.org/10.1186/s12859-022-04634-w)|[SNPSimulationTests](/logicGP/logicGP.Tests/Unit/Data/Simulated/SNPSimulationTests.cs)|

## Real data sets

Chosen data sets from the UC Irvine Machine Learning Repository

|Data set| Code|
| ------ | :---|
|[National Poll on Healthy Aging (NPHA)](https://archive.ics.uci.edu/dataset/936/national+poll+on+healthy+aging+(npha))|[NationalPollTests](/logicGP/logicGP.Tests/Unit/Data/Real/NationalPollTests.cs)|
|[Car Evaluation](https://archive.ics.uci.edu/dataset/19/car+evaluation)|[CarEvaluationTests](/logicGP/logicGP.Tests/Unit/Data/Real/CarEvaluationTests.cs)|
|[Balance Scale](https://archive.ics.uci.edu/dataset/12/balance+scale)|[BalanceScaleTests](/logicGP/logicGP.Tests/Unit/Data/Real/BalanceScaleTests.cs)|
|[Solar Flare](https://archive.ics.uci.edu/dataset/89/solar+flare)|[SolarflareTests](/logicGP/logicGP.Tests/Unit/Data/Real/SolarflareTests.cs)|
|[Lenses](https://archive.ics.uci.edu/dataset/58/lenses)|[LensesTests](/logicGP/logicGP.Tests/Unit/Data/Real/LensesTests.cs)|

### National Poll on Healthy Aging (NPHA)

Example Model found by logicGP-FLCW-Macro on the NPHA data set with $47.33\%$ MacroAccuracy.

| $w_{0-1}$  | $w_{2-3}$  | $w_{4+}$  | Condition                                                                                     |
|--------|--------|-------|----------------------------------------------------------------------------------------------|
| $1.86$ | $0.84$ | $0.86$ | Employment $\notin \{\text{Refused},\text{Retired}\}$                                        |
| $0.36$ | $0.86$ | $1.63$ | Sleep Medication $\in \{\text{Refused},\text{Use regularly}\}$                              |
| $1.49$ | $1.02$ | $0.70$ | Race $\in \{\text{Hispanic}\}$                                                              |
| $1.47$ | $1.12$ | $0.54$ | Dental Health $\in \{\text{Excellent},\text{Poor}\}$                                        |
| $1.41$ | $0.95$ | $0.89$ | Dental Health $\notin \{\text{Excellent},\text{Very Good}\}$ $\wedge$ Physical Health $\in \{\text{Very Good},\text{Good},\text{Poor}\}$ |
| $0.81$ | $0.89$ | $1.39$ | Physical Health $\notin \{\text{Refused},\text{Very Good}\}$                                |
| $1.24$ | $1.01$ | $0.87$ | Mental Health $\in \{\text{Excellent}\}$                                                   |
| $1.09$ | $0.97$ | $1.00$ | Physical Health $\notin \{\text{Very Good},\text{Poor}\}$                                   |
| $0.91$ | $1.02$ | $1.02$ | Dental Health $\notin \{\text{Excellent},\text{Good}\}$ $\wedge$ Mental Health $\in \{\text{Very Good},\text{Good}\}$ |

### Lenses

Example model found by logicGP-FLCW-Macro with $93.33\%$ MacroAccuracy.

| $w_{Hard}$ | $w_{Soft}$ | $w_{None}$  | Condition                                                      |
|------|------|-------|---------------------------------------------------------------------------|
| 0.2  | 5.0  | 1.11  | age ∈ {young} ∧ spectacle prescription ∈ {myope}                         |
| 0.0  | 0.0  | 3.67  | astigmatic ∈ {no}                                                        |
| 0.0  | 3.2  | 0.96  | age ∈ {pre-presbyopic} ∧ spectacle prescription ∈ {myope}               |
| 0.67 | 0.0  | 0.33  | none of the above

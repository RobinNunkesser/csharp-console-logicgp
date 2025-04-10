# Introduction

logicGP is a generalization of the [Genetic Programming for Association Studies (GPAS)](https://doi.org/10.1093/bioinformatics/btm522) algorithm. It is currently unpublished, first results can be seen on published [slides](https://isd-nunkesser.github.io/slides/ISDBlackHighlyInterpretablePredictionModels.html#/logicgp).
It is intended to be used for all classification tasks. Currently, it is restricted to categorical features. 

The implementation mainly uses the following NuGet packages:

- [Italbytz.Ports.Algorithms.AI](https://www.nuget.org/packages/Italbytz.Ports.Algorithms.AI) (Source: [nuget-ports-algorithms-ai](https://github.com/Italbytz/nuget-ports-algorithms-ai))
- [Italbytz.Adapters.Algorithms.AI](https://www.nuget.org/packages/Italbytz.Adapters.Algorithms.AI) (Source: [nuget-adapters-algorithms-ai](https://github.com/Italbytz/nuget-adapters-algorithms-ai))

The algorithm is intended to be compatible with Microsoft's [ML.NET](https://dotnet.microsoft.com/en-us/apps/ai/ml-dotnet).

# Getting started

The project features unit tests as a starting point. The most important unit tests show the application of the algorithm to example data sets.

## Overview of Unit Tests for Data Sets

### Simulated data sets

|Data set| Code|
| ------ | :---|
|Simulation from Section 3.3 of [Detecting high-order interactions of single nucleotide polymorphisms using genetic programming](https://doi.org/10.1093/bioinformatics/btm522)|[SNPSimulationTests](/logicGP/logicGP.Tests/Unit/Data/Simulated/SNPSimulationTests.cs)|
|Simulation from Equation (2) of [Evaluation of tree‑based statistical learning methods for constructing genetic risk scores](https://doi.org/10.1186/s12859-022-04634-w)|[SNPSimulationTests](/logicGP/logicGP.Tests/Unit/Data/Simulated/SNPSimulationTests.cs)|
|Simulation from Equation (3) of [Evaluation of tree‑based statistical learning methods for constructing genetic risk scores](https://doi.org/10.1186/s12859-022-04634-w)|[SNPSimulationTests](/logicGP/logicGP.Tests/Unit/Data/Simulated/SNPSimulationTests.cs)|

### Real data sets

Chosen data sets from the UC Irvine Machine Learning Repository

|Data set| Code|
| ------ | :---|
|[Iris](https://archive.ics.uci.edu/dataset/53/iris)|[IrisTests](/logicGP/logicGP.Tests/Unit/Data/Real/IrisTests.cs)|
|[National Poll on Healthy Aging (NPHA)](https://archive.ics.uci.edu/dataset/936/national+poll+on+healthy+aging+(npha))|[NationalPollTests](/logicGP/logicGP.Tests/Unit/Data/Real/NationalPollTests.cs)|
|[Car Evaluation](https://archive.ics.uci.edu/dataset/19/car+evaluation)|[CarEvaluationTests](/logicGP/logicGP.Tests/Unit/Data/Real/CarEvaluationTests.cs)|
|[Balance Scale](https://archive.ics.uci.edu/dataset/12/balance+scale)|[BalanceScaleTests](/logicGP/logicGP.Tests/Unit/Data/Real/BalanceScaleTests.cs)|
|[Solar Flare](https://archive.ics.uci.edu/dataset/89/solar+flare)|[SolarflareTests](/logicGP/logicGP.Tests/Unit/Data/Real/SolarflareTests.cs)|
|[Lenses](https://archive.ics.uci.edu/dataset/58/lenses)|[LensesTests](/logicGP/logicGP.Tests/Unit/Data/Real/LensesTests.cs)|

#### National Poll on Healthy Aging (NPHA)

Example Model found by logicGP-FLCW-Macro on the NPHA data set with $47.33\%$ MacroAccuracy.

| $w_{0-1}$  | $w_{2-3}$  | $w_{4+}$  | Condition                                                                                     |
|--------|--------|-------|----------------------------------------------------------------------------------------------|
| $1.86$ | $0.84$ | $0.86$ | Employment $\notin$ {Refused,Retired}                                        |
| $0.36$ | $0.86$ | $1.63$ | Sleep Medication $\in$ {Refused,Use regularly}                              |
| $1.49$ | $1.02$ | $0.70$ | Race $\in$ {Hispanic}                                                              |
| $1.47$ | $1.12$ | $0.54$ | Dental Health $\in$ {Excellent,Poor}                                        |
| $1.41$ | $0.95$ | $0.89$ | Dental Health $\notin$ {Excellent,Very Good} $\wedge$ Physical Health $\in$ {Very Good,Good,Poor} |
| $0.81$ | $0.89$ | $1.39$ | Physical Health $\notin$ {Refused,Very Good}                                |
| $1.24$ | $1.01$ | $0.87$ | Mental Health $\in$ {Excellent}                                                   |
| $1.09$ | $0.97$ | $1.00$ | Physical Health $\notin$ {Very Good,Poor}                                   |
| $0.91$ | $1.02$ | $1.02$ | Dental Health $\notin$ {Excellent,Good} $\wedge$ Mental Health $\in$ {Very Good,Good} |
| 0.0 | 1.0  | 0.0  | none of the above

#### Lenses

Example model found by logicGP-FLCW-Macro with $93.33\%$ MacroAccuracy.

| $w_{Hard}$ | $w_{Soft}$ | $w_{None}$  | Condition                                                      |
|------|------|-------|---------------------------------------------------------------------------|
| 0.2  | 5.0  | 1.11  | age ∈ {young} ∧ spectacle prescription ∈ {myope}                         |
| 0.0  | 0.0  | 3.67  | astigmatic ∈ {no}                                                        |
| 0.0  | 3.2  | 0.96  | age ∈ {pre-presbyopic} ∧ spectacle prescription ∈ {myope}               |
| 0.67 | 0.0  | 0.33  | none of the above

# Analyzing Data Sets

We have tried to integrate with Microsoft's [ML.NET](https://dotnet.microsoft.com/en-us/apps/ai/ml-dotnet) as closely as possible. However, ML.NET has many internal APIs and is not always easy to integrate with, so some manual work is needed. 

A good starting point is to first use Microsoft's [AutoML](https://learn.microsoft.com/en-us/dotnet/machine-learning/reference/ml-net-cli-reference) or [Model Builder](https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/load-data-model-builder) to generate an ML.NET input model class and pipeline for your data set. 

## Remarks on data preparation

ML.NET's trainers typically operate on a two-dimensional feature table of ```float``` values and a label column with ```uint``` values describing the classes. 

Data gets prepared for training with [data transformations](https://github.com/dotnet/docs/blob/main/docs/machine-learning/resources/transforms.md). Typically, the following transformations are suggested:

- ```float``` or ```int``` values in a feature are taken as ```float``` to preserve possible ordinal values
- ```string``` values in a feature are one hot encoded
- ```string``` values in the label get a key-value-mapping to ```uint```

Unfortunately, logicGP cannot operate on one hot encoded values presently. The direct use of the key-value-mapping is also not possible due to ML.NET hiding a lot of internal APIs. 

The alternatives (examples may be found in the unit tests) are manual mappings with given key-value-mappings. 
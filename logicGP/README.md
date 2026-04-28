# logicGP Consumer

Dieses Consumer-Projekt ist jetzt der kanonische Ort fuer den Benchmark-CLI mit dem Assembly-Namen `Italbytz.AI.ML.LogicGp.Benchmark.Cli`.

## Zweck

- hostet den C#-Benchmark-CLI fuer LogicGP
- wird von den Python-Benchmark-Wrappern aus `pypi-scoredrulesets` aufgerufen
- referenziert die aktuelle lokale `nuget-ai`-Codebasis direkt per ProjectReference

## Build

```bash
cd artifacts/consumers/production/csharp-console-logicgp/logicGP
dotnet build logicGP.sln -c Release
```

Die CLI-Binaries liegen danach unter:

- `logicGP/bin/Release/net8.0/Italbytz.AI.ML.LogicGp.Benchmark.Cli`
- `logicGP/bin/Release/net9.0/Italbytz.AI.ML.LogicGp.Benchmark.Cli`

## Aufruf

```bash
./logicGP/bin/Release/net9.0/Italbytz.AI.ML.LogicGp.Benchmark.Cli run \
  --trainer flcw-macro \
  --dataset iris \
  --generations 100 \
  --population 1000 \
  --seed 42 \
  --output result.json
```

Fuer Ergebnisvergleiche:

```bash
./logicGP/bin/Release/net9.0/Italbytz.AI.ML.LogicGp.Benchmark.Cli compare \
  --python python-result.json \
  --csharp csharp-result.json
```

## Hinweise

- Die fachliche Benchmark-Dokumentation liegt weiterhin in `artifacts/packages/deprecated/dotnet/nuget-adapters-algorithms-ea/BENCHMARK_README.md`.
- Dieses Verzeichnis enthaelt nur den Consumer und die zugehoerige Solution-Struktur.
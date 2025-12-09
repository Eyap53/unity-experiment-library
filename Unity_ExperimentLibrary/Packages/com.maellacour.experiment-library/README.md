# ExperimentLibrary

[![openupm](https://img.shields.io/npm/v/com.maellacour.experiment-library?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.maellacour.experiment-library/)

ExperimentLibrary keeps human-subject experiments tidy by enforcing a predictable folder structure, CSV pipelines, and log capture straight from Unity. It ships with drop-in MonoBehaviours and ScriptableObjects so that experiment operators can focus on trial design instead of file IO.

## Philosophy

- **Single source of truth:** every build reads inputs from `StreamingAssets/Inputs` and writes outputs to `StreamingAssets/Outputs`, keeping editor and player paths identical.
- **Participant-first tooling:** all APIs accept a participant identifier so you never mix up data between sessions.
- **CSV all the way down:** CsvHelper is bundled so you can model stimuli, responses, and telemetry as plain C# records.
- **Fail loudly but stay simple:** guard clauses catch missing files or folders early while the public API stays one-line per action.

## What You Get

- **Input helpers** (`ExperimentInputs`) to read participant-specific CSV files and JSON settings.
- **Output helpers** (`ExperimentOutputs`) to append or overwrite CSV logs per participant or globally.
- **Utilities** like `DirectoryExtensions.Copy`, `ExperimentUtilities.AddCsvExtension`, and CSV converters for `Vector2`/`Vector3`.
- **Operator UI** with `ParticipantSelector` and `ParticipantIndexSO` so lab staff can change subjects in builds.
- **Run-end diagnostics** via `LogSaver`, `SimpleLogSaver`, and optional Unity Atoms powered `AtomLogSaver`.
- **Samples** demonstrating the bare-minimum output flow and frame-by-frame logging with converters.

## Installation

### Requirements

- Unity 2021.1+.
- Registries that provide `CsvHelper` (via [UnityNuGet](https://github.com/xoofx/UnityNuGet)) and `Newtonsoft.Json`.

### OpenUPM (recommended)

1. Add the UnityNuGet scoped registry (see link above).
2. Install through OpenUPM CLI:

 ```bash
 openupm add com.maellacour.experiment-library
 ```

### Git URL (manifest.json)

```json
{
 "dependencies": {
  "com.maellacour.experiment-library": "https://github.com/mael-lacour/unity-experiment-library.git#release/2.6.1"
 },
 "scopedRegistries": [
  {
   "name": "UnityNuGet",
   "url": "https://unitynuget-registry.azurewebsites.net",
   "scopes": ["org.nuget"]
  }
 ]
}
```

## Folder Conventions

- Inputs live in `StreamingAssets/Inputs/{participantId}/`, plus optional `StreamingAssets/Inputs/*.csv` shared by all participants.
- Outputs are written to `StreamingAssets/Outputs/` and `StreamingAssets/Outputs/{participantId}/`.
- The library creates missing folders automatically, so distributing experiments is mostly copying the StreamingAssets tree.

## Quick Start

1. Create a `ParticipantIndexSO` asset (`Assets/Create/ExperimentLibrary/Participant Index`). Reference it from a `ParticipantSelector` in your scene so operators can enter the participant ID.
2. Drop CSV input files under `StreamingAssets/Inputs/{participantId}/Stimuli.csv` (or any name you prefer).
3. Use the helpers inside your gameplay code:

```csharp
using CsvHelper.Configuration;
using ExperimentLibrary;

var trials = ExperimentInputs.ReadParticipantInput<TrialRow>(participantId, "Stimuli", new TrialRowMap());

foreach (var trial in trials)
{
    // run the trial, gather responses
    results.Add(new TrialResult { Trial = trial.Id, ReactionTime = timer.ElapsedMilliseconds });
}

ExperimentOutputs.WriteParticipantOutputs(results, participantId, "Results");
ExperimentOutputs.WriteCommonOutputs(new[] { sessionSettings }, "SessionSettings", append: true);
```

1. Optionally add `SimpleLogSaver` to persist Unity console output next to the participant CSV files when the player quits.

## Runtime Building Blocks

- **ExperimentInputs**
 	- `ReadParticipantInput<T>` and `ReadCommonInput<T>` load CSV rows into strongly typed lists with optional `ClassMap` overrides.
 	- `ReadParticipantSettings<T>` deserializes JSON payloads per participant (e.g., calibration values).
- **ExperimentOutputs**
 	- `WriteParticipantOutputs`, `WriteCommonOutputs`, and `WriteOutputs` cover per-subject, shared, or arbitrary absolute paths.
 	- `AppendOutput` handles streaming telemetry one record at a time while keeping CSV headers correct.
 	- `ReadParticipantOutputs` gives you round-tripping during validation or review.
- **ExperimentUtilities.AddCsvExtension** ensures file names end with `.csv`, so inspectors can accept bare names.
- **DirectoryExtensions.Copy** mirrors reference folders (stimuli, config) into build locations while skipping extensions you specify.
- **ParticipantSelector + ParticipantIndexSO** synchronize UI (TMP input + label) with a ScriptableObject that other systems can read or listen to through `OnValueChanged`.
- **LogSaver family** copies Unity`s player log into the participant folder, deduplicated with a timestamp;`SimpleLogSaver` uses a serialized int, while `AtomLogSaver` reads a Unity Atoms `IntReference` when that package is present.
- **Vector2/Vector3 converters** allow CsvHelper to write and read Unity vectors inside CSV files without auxiliary DTOs.

## Integrations

- **Unity Atoms** (optional dependency): when `UNITY_ATOMS` is defined, `AtomLogSaver` is available, letting you bind the participant ID to ScriptableObject references/event-driven flows instead of manual serialization.

## Samples

- `Samples~/OutputBase`: minimal scene that saves a list of records to CSV.
- `Samples~/OutputEveryFrame`: frame-level logger showcasing listeners plus the `Vector3Converter`.

## License

Released under the Mozilla Public License 2.0. See [LICENSE.md](./LICENSE.md).

# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.3.0] - 2024-02-08
### Added
- Samples: Add OutputBase, to showcase the way to write output data easily.
- Samples: Add OutputEveryFrame, to showcase the way to write output data every frame.


## [2.2.0] - 2024-02-07
### Added
- Add converter for Vector3.
- Add converter for Vector2.
- Add append option in WriteOutputs.
- Allows use of IEnumerable instead of list in all write outputs methods.

### Changed
- Add Append option in Common and Participant write output methods.
- Make AppendParticipentOutputs obsolete


## [2.1.0] - 2023-08-02
### Added
- Added AppendOutput to replace WriteOutput.
- AppendOutput Add data at the end of the file.
- AppendOutput add the header if file missing or empty.
- Tests for methods of ExperimentOutputs.

### Changed
- WriteOutput is now marked obsolete (replaced by AppendOutput).


## [2.0.1] - 2023-07-28
### Added
- Add WriteOutput method.

### Changed
- BreakingChange: Changed namespace name.
- BreakingChange: Changed output method names.


## [1.3.0] - 2023-05-24
### Changed
- Now use the CsvHelper from UnityNuget.


## [1.2.1] - 2023-03-09
### Changed
- Made the methods virtual in ParticipantSelector

### Fixed
- Properly add .csv if missing from input name


## [1.2.0] - 2023-03-09
### Added
- Allows .csv extension in the filename field in Experiments Input/Output classes


## [1.1.1] - 2022-11-08
### Added
- Unit tests

### Fixed
- Fix ignored extensions not working


## [1.1.0] - 2022-07-08
# Added
- Prevent .meta files from being copied to Build Inputs directory.


## [1.0.0] - 2021-10-12
First release of the package

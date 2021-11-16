# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
- Back projected Direction selector algorithm for continuous range of output directions.
- Cubic (three-dimensional) controller/behaviours/masks
- External scheduler for controllers (aims to smooth out frame rate dips)
- Context combinator option that masks a broader range of directions than just the highest mask direction.

## [0.0.1] - 2021-11-16
### Added
- Package Core.
- Behaviours: DotToTag, DotToTransform, DotToLayer, DotToNavMesh.
- Masks:  DotToTagMask, DotToTransformMask, DotToLayerMask.
- Demo scene, demo prefabs, and demo scene code.
- Editor tests of behaviours.
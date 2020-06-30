# Unity Preset & Asset Bundle Build

This repository contains a very rough & ready Proof of Concept of using [Presets](https://docs.unity3d.com/2018.4/Documentation/Manual/Presets.html) (2018.4) and a custom Editor to script to:

* Downsize textures via predefined Presets, to support different asset settings for different device performance profiles.
* Build Asset Bundles for different platforms

## Running

Load the Sample Scene + Run

## Assets

A simple 1024x1024 sprite atlas is provided. This contains a set of 64x64 files. These can be found under `Assets/Sprites/` 

## Presets

[Presets](https://docs.unity3d.com/2018.4/Documentation/Manual/Presets.html) (2018.4) are used to define Asset types and settings. Separating the asset configuration from the assets themselves. These can be found under `Assets/Presets/`

## Asset Bundles

An Asset Bundle is build per platform and per Resolution. They are output to `Assets/StreamingAssets/` but could easily sit on a CDN.

Asset Bundles are output to a conventionally named folder:

`[Platform]-[LowDefintion|StandardDefintion]/`

i.e.

```
OSX-StandardDefinition/
iOS-LowDefinition/
```

Asset Bundles are built via the Builder menu. You can build for iOS or OSX. Switch to the appropriate platform and then use the Builder menu to build all Resolution 

## Loading Asset Bundles

Simple Async Asset Bundle loading is demonstrated. You can switch Definition in the Inspector in the script attached to the  `Asset Bundle Async Loader` object

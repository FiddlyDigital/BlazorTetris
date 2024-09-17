# BlazorTetris!

It's Tetris, as a Blazor WASM application.

An example application to test writing/deploying Blazor WASM apps, and how HTML Canvas interaction works between WASM Space and HTML elements.

## Controls
- a/A - Move left
- s/S - Drop
- d/D - Move Right
- q/Q - rotate counter-clockwise
- e/E - rotate clockwise

## Features

- .Net 8
- 2-Way JSInterop
  - WASM to JS
    - Starting the rendering loop
  - JS to WASM
    - Call to render
    - Keyboard input
- HTML Canvas Interaction
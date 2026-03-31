# Logos.Input

## Source Code

The entire source code can be found in this GitHub repository. To build the source code:

1. Clone the repository: `git clone https://github.com/COMP-4120-Logos-Input/Logos.Input.git`
2. Open the solution in your favorite IDE or code editor (make sure that you have .NET 10 and C# build tools installed).
3. Build the solution using either the Debug or Release configuration.

Alternatively, you may build the solution from the command line using the `dotnet build` command. See Microsoft's [documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build) for more information.

## Folder Layout

* **Libraries:** Stores precompiled native libraries that Logos.Input depends on, but cannot be built using C# build tools.
* **Source:** Stores implementation of input processing components provided by Logos.Input, including extensions for SDL3.
* **Tests:** Stores unit tests that verify the functionality of input processing components provided by Logos.Input.

## Dependencies

Logos.Input depends on the LTS version of [Simple DirectMedia Layer](https://github.com/libsdl-org/SDL/releases) to implement input processing components within the Logos.Input.Sdl namespace and simulate inputs in unit tests. Currently, this repository comes with precompiled SDL3 binaries for Windows x64 and Mac OS X arm64 platforms, which can be found within the Libraries folder. Platform-specific binaries included in this repository will be automatically copied to the build output folder when the solution is built, so no additional configuration is required. For all other platforms, you will have to compile SDL3 from its source and copy the binaries to the build output folder. If you have SDL3 binaries for a platform we've missed, let us know! We'll be happy to include it in the solution build process.

Logos.Input depends on the LTS version of [NUnit](https://nunit.org/download/) to implement its unit tests. NUnit can be downloaded as a NuGet package within popular IDEs like Visual Studio and JetBrains Rider. Alternatively, NUnit can be downloaded from its website and built from its source.

## Unit Tests

You can build and run the unit tests within your IDE or code editor of choice using their respective testing features. Alternatively, you can use the `dotnet test` command within the command line to build and run the unit tests. See Microsoft's [documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test) for more information.

Automated tests operate on simulated inputs generated using SDL3, which test the functionality of input processing components in a controlled setting. Manual tests (annotated with the `[Explicit]` attribute in the source code) do not run as part of the automated test suite, but can be ran individually to test how components react to inputs from physical devices. Currently, we are resolving issues with the manual tests that prevent them from working properly, but the automated tests should be able to pass or fail in a consistent manner.

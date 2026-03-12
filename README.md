# Logos.Input

## Testing

Run the fast unit tests with:

```bash
dotnet test Tests/Logos.Input.Tests.csproj
```

These tests use a fake SDL runtime to validate `SdlInputProvider` behavior without requiring real hardware.

Manual SDL integration tests are defined in `SdlInputProviderIntegrationTestFixture` and are marked `Explicit`, so they do not run as part of the normal test suite. Use them when you want to verify the real native SDL path and external keyboard hot-plug behavior on a supported machine.

## Native SDL binaries

The repository vendors the following native SDL3 binaries:

- `Libraries/win-x64/SDL3.dll`
- `Libraries/osx-arm64/libSDL3.dylib`

The source and test projects copy the platform-appropriate native library into the build output automatically.

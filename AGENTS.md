# NHibernate.Extensions.Logging Developer Guidelines

Use Microsoft.Extensions.Logging as an NHibernate logging provider.

## Commands
- Build: `dotnet build -c Release`
- Test: `dotnet test --coverage --coverage-output-format cobertura`
- Restore only: `dotnet restore`
- Check for vulnerable packages: `dotnet list <project> package --vulnerable --include-transitive`

## Pointers
- Package management & versions: @Directory.Packages.props
- Core library implementation: @src/NHibernate.Extensions.Logging/MicrosoftLogger.cs
- Test suite: @test/NHibernate.Extensions.Logging.Tests/MicrosoftLoggerTests.cs
- Sample usage: @samples/SampleApp/Program.cs

## Architecture Overview
- `src/NHibernate.Extensions.Logging`: Published library (`netstandard2.0;net8.0`).
- `test/NHibernate.Extensions.Logging.Tests`: xUnit v3 tests (`net10.0;net8.0`). References `src` via `ProjectReference` (no version override), exercising the floor version.
- `samples/SampleApp`, `samples/SampleWebApp`, `samples/SampleShared`: Usage examples (`net10.0`). Uses `FluentNHibernate`, validating the latest NHibernate version.
- `Directory.Packages.props`: Central package management (`ManagePackageVersionsCentrally=true`, `CentralPackageTransitivePinningEnabled=true`).

## Dependency Versioning Policy — read before "fixing" a version

Do not bump `NHibernate` or `Microsoft.Extensions.Logging.Abstractions` in `src/NHibernate.Extensions.Logging/NHibernate.Extensions.Logging.csproj` or the `netstandard2.0` block of `Directory.Packages.props` just because a newer version exists on NuGet or looks out of sync with the central version:

- `src/NHibernate.Extensions.Logging.csproj` pins `NHibernate` via `VersionOverride` (currently `5.4.10`). This is the package's declared **minimum-compatible version (floor)** for consumers — deliberately kept lower than the central `Directory.Packages.props` version (currently `5.7.0`).
- The floor is exercised in tests: `test/NHibernate.Extensions.Logging.Tests` references `src` via plain `ProjectReference` with no override, so `dotnet test` builds and runs against the floor version.
- The **latest** NHibernate version is validated separately: `samples/SampleShared` depends on `FluentNHibernate`, pulling in the central (latest) `NHibernate` version via `CentralPackageTransitivePinningEnabled=true`.
- Net effect: CI covers both ends (oldest supported NHibernate via tests, newest via samples) without an explicit test matrix.
- Only raise the floor when there is a concrete driver — e.g., a CVE affecting the floor or a new API requirement. Verify first with `dotnet list <project> package --vulnerable`.
- `Microsoft.Extensions.Logging.Abstractions 8.0.3` is pinned only for the `netstandard2.0` target in `Directory.Packages.props` (floor). The `net8.0` target uses `FrameworkReference Include="Microsoft.AspNetCore.App"` and resolves to the shared runtime.

## TargetFramework Policy

- `src` intentionally targets only `netstandard2.0` and `net8.0` — there is no `#if`-conditional code between them, so no additional TFMs are needed.
- `net8.0` is an LTS release. Because .NET class libraries are forward-compatible, `net8.0` assets resolve for `net9.0`/`net10.0` consumers as well.
- `test`/`samples` track the newest SDK (`net10.0`) to catch forward-compatibility issues early.
- Revisit `net8.0` only when it approaches EOL or when a feature requires an API not present in `net8.0`.

## Claude Code Compatibility

`CLAUDE.md` is a symbolic link pointing to `AGENTS.md`. Edit `AGENTS.md` directly.

## Self-Reflection
- **Candidate**: Distill a non-obvious gotcha into ≤ 2 context-tagged bullets. Propose it before writing.
- **Promote**: On confirmation, write it to a dedicated file — merge an existing topic doc, else `docs/<topic>.md`, else `docs/lessons-learned.md`. Add or update one `@path` line under Pointers.
- **Prune**: Drop entries once stale (obsolete version, now enforced, duplicated, or a transcript) — not by a fixed count.

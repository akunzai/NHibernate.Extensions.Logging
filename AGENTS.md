# NHibernate.Extensions.Logging Developer Guidelines

Use Microsoft.Extensions.Logging as an NHibernate logging provider.

## Commands
- Build: `dotnet build -c Release`
- Test: `dotnet test --coverage --coverage-output-format cobertura`
- Check for vulnerable packages: `dotnet list <project> package --vulnerable --include-transitive`

## Pointers
- Package management: @Directory.Packages.props
- Core library implementation: @src/NHibernate.Extensions.Logging/MicrosoftLogger.cs
- Test suite: @test/NHibernate.Extensions.Logging.Tests/MicrosoftLoggerTests.cs
- Sample usage: @samples/SampleApp/Program.cs

## Dependency Versioning & TargetFramework Policy
- **Minimum-compatible floor**: `src/` pins `NHibernate` via `VersionOverride` to declare the minimum supported version for consumers. Tests reference `src` without override, exercising this floor in CI.
- **Latest version ceiling**: Samples reference `FluentNHibernate`, pulling the latest central `NHibernate` via `CentralPackageTransitivePinningEnabled`.
- **Raising the floor**: Only bump the floor for concrete CVEs or required APIs (verify via `dotnet list <project> package --vulnerable`).
- **Framework references**: In `Directory.Packages.props`, `Microsoft.Extensions.Logging.Abstractions` is pinned only for `netstandard2.0`; `net8.0` uses `FrameworkReference Include="Microsoft.AspNetCore.App"`.
- **TFMs**: `src` targets LTS `netstandard2.0;net8.0` without `#if` branching; `test` and `samples` track the latest .NET SDK to catch forward-compatibility issues early.

## Claude Code Compatibility

`CLAUDE.md` is a symbolic link pointing to `AGENTS.md`. Edit `AGENTS.md` directly.

## Prevent Recurrence
- **Candidate**: Name who hits this again, in which file, on what change. No such scenario, nothing to propose.
- **Promote**: Offer the first tier that reaches them and only that one, pending confirmation — enforce it (assert/type/test) with its size quoted, else a comment at that site, else an agent-facing doc (`docs/agents/<topic>.md`, else `docs/agents/lessons-learned.md`) with one `@path` line under Pointers and one sentence on why the tiers above cannot hold it.
- **Prune**: When adding to a file, audit the rest of it in the same pass. Drop entries once stale (obsolete version, now enforced, duplicated, or a transcript) — not by a fixed count.

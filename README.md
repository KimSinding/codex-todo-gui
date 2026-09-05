# Codex Todo GUI

En simpel todo-liste app bygget i **C# WPF**, lavet som en del af Øvelse 4.1
("Codex lavede rod i mit C#-repo"). Formålet med opgaven var todelt: bruge
Codex til at generere GUI-kode, og træne Git – herunder gendannelse af både
ikke-committede og committede/pushede ændringer.

## Funktioner

- Tilføj en ny opgave med tekst
- Vælg prioritet (Low / Medium / High) ved oprettelse
- Opgaver sorteres automatisk efter prioritet (High → Medium → Low)
- Vis alle opgaver i en liste med tekst og done-status
- Markér en opgave som færdig via checkbox
- Rediger en opgaves tekst ved dobbeltklik
- Data gemmes og indlæses automatisk

## Data

En opgave består af:

| Felt       | Type   | Beskrivelse                        |
|------------|--------|-------------------------------------|
| `Id`       | int    | Unikt id                            |
| `Text`     | string | Opgavetekst                         |
| `IsDone`   | bool   | Om opgaven er markeret som færdig   |
| `Priority` | enum   | Low / Medium / High                 |

Data gemmes i **`data/tasks.json`** i projektets rodmappe. Filen indlæses
automatisk ved opstart og gemmes igen, hver gang der tilføjes, redigeres
eller markeres en opgave som færdig.

## Kør appen lokalt

Kræver .NET SDK (testet med .NET 10) og Windows (WPF er Windows-only).

```bash
git clone <repo-url>
cd codex-todo-gui
dotnet run
```

## Teknologi

- **C# / WPF** (.NET)
- Ingen eksterne NuGet-pakker udover .NET's indbyggede JSON-serialisering

## Git-historik og øvelsens forløb

Repoet er bevidst bygget op til at demonstrere Git-recovery i praksis:

1. **Del 0** – Repo initialiseret, `.gitignore` for .NET tilføjet
   (`dotnet new gitignore`), første commit og push.
2. **Del 1** – Baseline-app genereret med Codex: Add / List / Done /
   persistence. Testet grundigt (inkl. en rettet `StartupUri`-fejl) før
   commit.
3. **Del 2** – Prioritet + Edit-funktion tilføjet, medførte en reel
   compile-fejl (`CS1513`). Ændringerne blev **ikke** committet, og blev
   gendannet med:
   ```bash
   git restore .
   ```
4. **Del 3** – Appen refaktoreret til MVVM (ViewModel + Commands). Byggede
   uden fejl, men Edit-funktionen (dobbeltklik) holdt op med at virke.
   Ændringen blev alligevel committet og pushet, og efterfølgende
   gendannet **uden at omskrive historikken**:
   ```bash
   git log --oneline
   git revert <commit-hash>
   git push
   ```

Historikken viser derfor tydeligt både den fejlbehæftede ændring og
reverten af den, i stedet for at den er slettet eller skjult.

```
Revert MVVM refactor - broke double-click edit functionality
update  (MVVM-refaktorering – reverteret)
Add basic todo GUI + persistence
Add .gitignore for .NET
Nyt projekt
```

## Kendte begrænsninger

- Ingen validering af tom opgavetekst udover UI'ets `CanExecute`-styring
- Ingen automatiske tests
- MVVM-forsøget fra Del 3 er reverteret og indgår derfor ikke i den
  nuværende kodebase — koden på `main` svarer til Del 1-arkitekturen
  (code-behind), udvidet med prioritet og edit fra Del 2.

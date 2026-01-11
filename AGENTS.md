# AGENTS.md

## Scopo del repository
Utility package per Unity 2022.3 (`com.gedemy.gameutils`). Gli script di runtime vivono in `Runtime/`, gli strumenti editor in `Editor/`, e le feature sperimentali in `WIP/`.

## Struttura principale
- `Runtime/` — codice utilizzabile in gioco (namespace `GameUtils`).
- `Editor/` — strumenti e finestre editor (namespace `UnityEditor.GameUtils`).
- `WIP/` — feature in sviluppo; mantenere separato dal runtime stabile.
- `*.asmdef` — rispettare la separazione Runtime/Editor.

## Convenzioni di codice
- **Namespace**:  
  - Runtime: `GameUtils`  
  - Editor: `UnityEditor.GameUtils`
- **Naming convention interna**: rispettare sempre le convenzioni di questo progetto (namespace, nomi file/classe, campi serializzati, stile).
- **Nomi file/classe**: 1 classe pubblica principale per file, nome file = nome classe.
- **Campi serializzati**: `private` con prefisso `_` e `[SerializeField]`.
- **API Unity**: usare `CreateAssetMenu`/`MenuItem` con `GameUtilsMenuConstants` per coerenza delle voci di menu.
- **Stile**: PascalCase per classi/metodi/proprietà, camelCase per parametri, underscore per campi privati.
- **ScriptableObject**: preferire SO per dati/configurazioni condivise; mantenere i path di menu consistenti.
## Requisiti per l'agente
- Includi sempre i `<summary>` nelle risposte finali.
- Commenta sempre il codice che scrivi.

## Asset e meta
- Non rimuovere o rinominare file `.meta` manualmente; Unity li gestisce.
- Ogni nuovo asset/script deve avere un relativo `.meta`.

## Note su WIP
- Le feature in `WIP/` sono sperimentali: evitare dipendenze inverse da `Runtime/` stabile.

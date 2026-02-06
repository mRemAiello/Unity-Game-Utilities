## 3.4.0

- 3.4.0 - Added modal window button cleanup with warnings for missing button prefab/root.
- Translated AGENTS.md to English and clarified the CHANGELOG version requirement.
- Added ClickInputManager and IClickable for Input System raycast click handling.
- Added README documentation for the Runtime/Input utilities and interfaces.
- Translated the Runtime/Input README documentation to English.

## Others

- Clarified that ResetData clears debug data but does not remove active event listeners.
- Updated and translated the Runtime/Events README to match the current event API.
- Added AutoBundles documentation and linked it from the root README.
- Implementata in AutoBundles2 la sincronizzazione dei group Addressables con asset, label e pulizia per estensioni escluse.
- Normalizzati i separatori dei percorsi in AutoBundles2 per generare asset path validi su Windows.
- Implementata la popolazione automatica di AutoBundles2 con cartelle di Assets (profondità 1), esclusioni precise e ordinamento alfabetico dei bundle.
- Aggiunto il monitor di performance UI con TextMeshPro per FPS e statistiche di rendering.
- Allineata la pulizia degli Addressables alle estensioni escluse con match per suffisso (es. .tar.gz).
- Aggiunta la rimozione automatica dagli Addressables degli asset con estensioni escluse in AutoBundles.
- Added English README documentation for the Task folder usage.
- Rifattorizzato AutoBundles per suddividere il flusso in funzioni riutilizzabili.
- Tradotto in inglese il README con dettagli aggiornati sulla cartella Events e sui suoi asset evento.
- Aggiornate le tuple evento per distinguere GameObject e ScriptableObject con visibilità condizionale in inspector.
- Resa virtuale l'invocazione in GameEventAsset e override nei relativi asset parametrizzati.
- Aggiunta l'invocazione con pulsante agli asset evento scriptable con parametro.
- Aggiornate le linee guida dell'agente con note su commenti alle variabili di classe e aggiornamento del CHANGELOG.
- Aggiornati i calcoli degli attributi per includere il contesto della classe nei valori correnti.
- Aggiunta la gestione opzionale di OnTriggerEnter e OnCollisionEnter per i Collectable.
- Tradotta in inglese la documentazione della cartella Currency.
- Aggiunta la gestione opzionale del click (Input System e legacy) per i Collectable.
- Aggiornata la registrazione dei listener degli eventi per mostrare l'oggetto reale e lo script/metodo di origine.
- Aggiornati i dati dei listener per salvare il riferimento Unity serializzabile del caller e il relativo fallback testuale.
- Reso esplicito e serializzato il riferimento Unity del caller nelle tuple evento con pulizia dei dati legacy.
- Tradotta in inglese la documentazione della cartella DataStructures.

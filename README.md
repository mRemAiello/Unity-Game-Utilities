# Unity Game Utilities

Questo pacchetto contiene una serie di script di utility per progetti che utilizzano l'engine Unity.

## Requisiti

Per utilizzare questo pacchetto hai bisogno dei seguenti assets:

### Unity Packages:

* [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@2.1/manual/index.html)

* [New Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/index.html)

### Terze parti:

* [Tri-Inspector](https://github.com/codewriter-packages/Tri-Inspector)

* [Quick Save](https://www.claytoninds.com/quick-save)

* [DOTween](http://dotween.demigiant.com/)

## Pacchetti

In questa sezione andiamo a vedere i contenuti del pacchetto.

### [Runtime](./Runtime)
Racchiude gli script da utilizzare direttamente nel gioco.
Principali sottocartelle:
- [Achievements](./Runtime/Achievements): gestione degli achievement.
- [Attributes](./Runtime/Attributes): sistema di attributi e classi da RPG.
- [Audio](./Runtime/Audio): riproduzione di clip audio.
- [Commands](./Runtime/Commands): coda di comandi eseguibili.
- [Constant](./Runtime/Constant): valori e costanti globali.
- [Currency](./Runtime/Currency): logica delle valute di gioco.
- [DataStructures](./Runtime/DataStructures): Scriptable Object comuni.
- [Debug](./Runtime/Debug): utility di debug e logging.
- [Dictionary](./Runtime/Dictionary): dizionari serializzabili.
- [Events](./Runtime/Events): implementazione di game events.
- [Extensions](./Runtime/Extensions): metodi di estensione vari.
- [Input](./Runtime/Input): componenti per il sistema input.
- [Math](./Runtime/Math): funzioni matematiche d'aiuto.
- [Misc](./Runtime/Misc): utilità generiche.
- [Movement](./Runtime/Movement): script di movimento.
- [Placement](./Runtime/Placement): piazzamento oggetti.
- [Projectiles](./Runtime/Projectiles): gestione di proiettili 2D.
- [Random](./Runtime/Random): strumenti per la randomizzazione.
- [Renderer](./Runtime/Renderer): renderer personalizzati.
- [Save](./Runtime/Save): gestione salvataggi e caricamenti.
- [Singleton](./Runtime/Singleton): classi base per singleton e manager.
- [States](./Runtime/States): semplici macchine a stati.
- [StatusEffects](./Runtime/StatusEffects): sistema di effetti di stato.
- [TagManager](./Runtime/TagManager): tag personalizzate.
- [Task](./Runtime/Task): gestione di task asincroni.
- [Tracker](./Runtime/Tracker): tracciamento di variabili.
- [UI](./Runtime/UI): componenti per l'interfaccia utente.
- [Utils](./Runtime/Utils): altre utilità.

### [Editor](./Editor)
Strumenti e finestre per l'editor di Unity.
- [AutoBundles](./Editor/AutoBundles): generazione automatica di bundle.
- [Dictionary](./Editor/Dictionary): drawer per SerializableDictionary.
- [Utils](./Editor/Utils): piccoli tool di supporto.
- [Windows](./Editor/Windows): finestre personalizzate dell'editor.

### [WIP](./WIP)
Funzionalità sperimentali in sviluppo.
- [Health](./WIP/Health): sistema salute e danno.
- [Lights](./WIP/Lights): gestione delle luci e ciclo giorno/notte.
- [Pool](./WIP/Pool): object pooling.
- [Reedem](./WIP/Reedem): codici premio.

Altre risorse utili:
- [CHANGELOG.md](./CHANGELOG.md)
- [LICENSE.md](./LICENSE.md)
- [package.json](./package.json)

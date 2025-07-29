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

## Struttura delle cartelle

### [Runtime](./Runtime)
Racchiude gli script da utilizzare direttamente nel gioco.
Principali sottocartelle:
- [Achievements](./Runtime/Achievements): gestione degli achievement.
- [Attributes](./Runtime/Attributes): sistema di attributi e classi da RPG.
- [Audio](./Runtime/Audio): riproduzione di clip audio.
- [Commands](./Runtime/Commands): coda di comandi eseguibili.
- [Constant](./Runtime/Constant): valori e costanti globali.
- [Currency](./Runtime/Currency): logica delle valute di gioco.
- [Data Structures](./Runtime/Data%20Structures): Scriptable Object comuni.
- [Debug](./Runtime/Debug): utility di debug e logging.
- [Dictionary](./Runtime/Dictionary): dizionari serializzabili.
- [Events](./Runtime/Events): implementazione di game events.
- [Extensions](./Runtime/Extensions): metodi di estensione vari.
- [Input](./Runtime/Input): componenti per il sistema input.
- [Math](./Runtime/Math): funzioni matematiche d'aiuto.
- [Misc](./Runtime/Misc): utilità generiche.
- [Movement](./Runtime/Movement): script di movimento.
- [Placement](./Runtime/Placement): piazzamento oggetti.
- [Random](./Runtime/Random): strumenti per la randomizzazione.
- [Renderer](./Runtime/Renderer): renderer personalizzati.
- [Save](./Runtime/Save): gestione salvataggi e caricamenti.
- [Singleton](./Runtime/Singleton): classi base per singleton e manager.
- [Status Effect](./Runtime/Status%20Effect): sistema di effetti di stato.
- [Tag Manager](./Runtime/Tag%20Manager): tag personalizzate.
- [Task](./Runtime/Task): gestione di task asincroni.
- [Tracker](./Runtime/Tracker): tracciamento di variabili.
- [UI](./Runtime/UI): componenti per l'interfaccia utente.
- [Utils](./Runtime/Utils): altre utilità.

### [Editor](./Editor)
Strumenti e finestre per l'editor di Unity.
- [Auto Bundles](./Editor/Auto%20Bundles): generazione automatica di bundle.
- [Dictionary](./Editor/Dictionary): drawer per SerializableDictionary.
- [Utils](./Editor/Utils): piccoli tool di supporto.
- [Windows](./Editor/Windows): finestre personalizzate dell'editor.

### [WIP](./WIP)
Funzionalità sperimentali in sviluppo.
- [Health](./WIP/Health): sistema salute e danno.
- [Lights](./WIP/Lights): gestione delle luci e ciclo giorno/notte.
- [Pool](./WIP/Pool): object pooling.
- [Reedem](./WIP/Reedem): codici premio.
- [State Machines](./WIP/State%20Machines): implementazione di FSM.

Altre risorse utili:
- [CHANGELOG.md](./CHANGELOG.md)
- [LICENSE.md](./LICENSE.md)
- [package.json](./package.json)

## Pacchetti

In questa sezione andiamo a vedere i contenuti del pacchetto.

### Runtime

* [Sistema di Attributi e classi da RPG](https://github.com/mRemAiello/Unity-Game-Utilities/tree/master/Runtime/Attributes) (Puoi gestire classi, modificatori, statistiche)

* [Salvataggio / Caricamento](https://github.com/mRemAiello/Unity-Game-Utilities/tree/master/Runtime/Save) (Sistema strutturato per gestire qualunque tipo di salvataggio / caricamento)

* [Strutture Dati](https://github.com/mRemAiello/Unity-Game-Utilities/tree/master/Runtime/Data%20Structures)

* [Tracker System](https://github.com/mRemAiello/Unity-Game-Utilities/tree/master/Runtime/Tracker) (Con questo sistema)












## Gestione di un Database di Assets comuni

Ti basta creare una sottoclasse di GenericDataManager<T1, T2>, dove T1 è il Manager e T2 è la classe di Scriptable Objects da caricare.

```cs
public class CurrencyManager : GenericDataManager<CurrencyManager, CurrencyData>
```

Inserisci poi lo script in un Prefab e carica gli assets per gestirne la lista.

## Debug

```cs
public class GameSaveManager : Singleton<GameSaveManager>, ILoggable
{
    public void SetActiveSaveSlot(int slot)
    {
        if (slot < _minSaveSlot || slot > _maxSaveSlot)
        {
            this.LogError($"Invalid save slot: {slot}. Must be between {_minSaveSlot} and {_maxSaveSlot}.");
            return;
        }
    }
}
```

## Auto Bundles


## Task



## Commands

Create a subclass of Command, then implements Execute method.

```cs
public class DrawCardAction : Command
{
    public override void Execute(CommandInput input)
    {
        // Add codes here
    }
}
```

### Generic Asset List Viewer

Make a subclass, then enjoy!

```cs
public class MaterialEditorWindow : GenericAssetEditorWindow<Material>
{
    [MenuItem("Window/Material Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<MaterialEditorWindow>();
        window.titleContent = new GUIContent("Material Editor");
        window.Show();
    }
}
```

## Loading Manager

```cs
void Start()
{
    LoadingScreenData data = new LoadingScreenData
    {
        EnableLoadingAnimation = true,
        Text = "Test Progress",
        Type = LoadingType.Fullscreen
    };

    LoadingManager.Instance.StartLoading(CustomLoadAction, data);
}

private void CustomLoadAction(Action<float> updateProgress)
{
    // Custom loading logic goes here
    StartCoroutine(SimulateLoading(updateProgress));
}

private IEnumerator SimulateLoading(Action<float> updateProgress)
{
    // Simulated loading process
    float progress = 0f;
    while (progress < 1f)
    {
        progress += Time.deltaTime / 5f; // Simulate a loading operation taking 5 seconds
        updateProgress(Mathf.Clamp01(progress));
        yield return null;
    }
}
```

## Asset Loader

```cs
public class AddressableTest : MonoBehaviour
{
    public AssetReferenceT<Sprite> assetReference;

    void Start()
    {
        AssetLoader.LoadAssetAsync<Sprite>(assetReference, OnLoadComplete);
    }

    private void OnLoadComplete(AsyncOperationHandle<Sprite> handle)
    {
        Debug.Log(handle.Result);
    }
}
```

## Element Tuple

```cs
public class TestTuple : ElementTuple<string, string>
{
    public TestTuple(string firstData, string secondData) : base(firstData, secondData)
    {
    }
}
```




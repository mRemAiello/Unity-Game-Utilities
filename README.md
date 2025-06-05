# Game Utils

Una lista di utility script per Unity. Includono:
- Un sistema di Singleton / Persistent Singleton e 
- Un sistema di gestione delle valute (currency)
- Un sistema di salvataggio / caricamento


## Requisiti

### Unity Packages:

* Addressables (https://docs.unity3d.com/Packages/com.unity.addressables@2.1/manual/index.html)

* New Input System (https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/index.html)

### Terze parti:

* Tri-Inspector (https://github.com/codewriter-packages/Tri-Inspector)
* Quick Save (https://www.claytoninds.com/quick-save)
* DOTween (http://dotween.demigiant.com/)

## Gestione di un Database di Assets comuni

Ti basta creare una sottoclasse di GenericDataManager<T1, T2>, dove T1 è il Manager e T2 è la classe di Scriptable Objects da caricare.

```cs
public class CurrencyManager : GenericDataManager<CurrencyManager, CurrencyData>
```

Inserisci poi lo script in un Prefab e carica gli assets per gestirne la lista.

## Salvataggio e caricamento

```cs
// Semplice salvataggio implementando l'interfaccia ISave
GameSaveManager.Instance.Save(this, "Money", 100);

// Salvataggio più strutturato
GameSaveManager.Instance.Save("Deck", "Card1", "123x1123");
GameSaveManager.Instance.Save("Deck", "Card2", "123x1123");
GameSaveManager.Instance.Save("Deck", "Card3", "123x1123");

// Caricamento
int money = GameSaveManager.Instance.Load<int>(this, "Money", 0);
int cardsCount = GameSaveManager.Instance.Load<int>("Deck", "CardsCount", 0);
string card1 = GameSaveManager.Instance.Load<string>("Deck", "Card1", "");
string card2 = GameSaveManager.Instance.Load<string>("Deck", "Card2", "");
string card3 = GameSaveManager.Instance.Load<string>("Deck", "Card3", "");
```

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

## Genera un seed casuale

```cs
int seed = SeedGenerator.GenerateSeed();
```

## Transform

```cs
transform.SetX(0);
transform.SetY(0);
transform.SetZ(0);
```

## Layers

```cs
// GameObject
gameObject.SetLayerMask(layerMask);
var layerMask = gameObject.GetLayerMask();
bool contains = gameObject.ContainsLayerMask(layerMask);

// Layer Mask
bool contains = layerMask.Contains(gameObject);
```
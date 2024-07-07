# Game Utils

## Requirements

### Unity Packages:

* Addressables (https://docs.unity3d.com/Packages/com.unity.addressables@2.1/manual/index.html)

### Third parts:

* Quantum Console (https://assetstore.unity.com/packages/tools/utilities/quantum-console-211046)
* VInspector 2 (https://assetstore.unity.com/packages/tools/utilities/vinspector-2-252297)
* Quick Save (https://www.claytoninds.com/quick-save)
* DOTween (http://dotween.demigiant.com/)

## Generate a Seed

```cs
int seed = SeedGenerator.GenerateSeed();
```

## Auto Bundles



## Scriptable Object Collections

Simply create a subclass:

```cs
[CreateAssetMenu(menuName = "Assets/Card Collection")]
public class CardDataCollection : ScriptableObjectCollection<CardData>
{
    [Button]
    public override void LoadAssets()
    {
        base.LoadAssets();
    }
}
```

Then make a new Scriptable Object and click "Load Assets".

## UI Animations

Simply add one (or more) of this script to a GameObject:

* TransformScaler
* CanvasFader
* Spinner

Then you can show / hide

```cs
component.Show();
component.Hide();
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

## Element Tuple

```cs
public class TestTuple : ElementTuple<string, string>
{
    public TestTuple(string firstData, string secondData) : base(firstData, secondData)
    {
    }
}
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


## Transform

```cs
transform.SetX(0);
transform.SetY(0);
transform.SetZ(0);
```
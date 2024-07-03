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

## Fade in / out

```cs
FadeToBlack.Instance.StartFade();
```

## Loading Manager


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

## Misc
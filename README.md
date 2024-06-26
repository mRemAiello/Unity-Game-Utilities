# Game Utils

## Requirements

* Quantum Console (https://assetstore.unity.com/packages/tools/utilities/quantum-console-211046)
* VInspector 2 (https://assetstore.unity.com/packages/tools/utilities/vinspector-2-252297)
* Quick Save (https://www.claytoninds.com/quick-save)
* DOTween (http://dotween.demigiant.com/)

## Generate a Seed

```cs
int seed = SeedGenerator.GenerateSeed();
```

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

## Misc
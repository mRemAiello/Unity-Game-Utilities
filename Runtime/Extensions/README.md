# Extension Methods

## List

Metodi disponibili:
- `RandomItem()`: restituisce un elemento casuale.
- `RandomItemRemove()`: rimuove e restituisce un elemento casuale.
- `Pop(index)`: estrae l'elemento in posizione `index` e lo rimuove.
- `Get(index)`: recupera l'elemento in posizione `index` se presente.
- `TryGet(index, out item)`: prova a recuperare l'elemento in `index`.
- `Append(newItem)`: aggiunge un nuovo elemento e lo restituisce.
- `FromEnd(index)`: estrae l'elemento a partire dalla fine.
- `InsertBefore(item, newItem)`: inserisce `newItem` prima di `item`.
- `InsertAfter(item, newItem)`: inserisce `newItem` dopo `item`.
- `Shuffle()`: mescola gli elementi.
- `Print(log)`: scrive il contenuto nel log.

```csharp
// Estrae l'elemento in posizione index e lo restituisce
T item = list.Pop(1);

// Aggiunge un nuovo elemento e lo restituisce
T item2 = list.Append(item);

// Estrae l'elemento in posizione index, a partire dalla fine
T item3 = list.FromEnd(1);
```

## Array

Metodi disponibili:
- `ForEach(action)`: esegue l'azione specificata per ogni elemento.

```csharp
int[] numbers = {1, 2, 3};
numbers.ForEach(n => Debug.Log(n));
```

## Camera

Metodi disponibili per modificare il colore di sfondo:
- `SetBackgroundColorR/G/B/A`
- `SetBackgroundColorRG/RB/RA/GB/GA/BA`
- `SetBackgroundColorRGB/RGA/RBA/GBA`

```csharp
camera.SetBackgroundColorRGB(1f, 0.5f, 0f);
```

## Color

Metodi disponibili:
- `WithR/G/B/A`
- `WithRG/RB/RA/GB/GA/BA`
- `WithRGB/RGA/RBA/GBA`
- `ToGray()`, `ToGrayColor()`

```csharp
Color c = Color.white.WithA(0.5f);
float gray = c.ToGray();
```

## Color32

Metodi disponibili (analoghi a `Color`):
- `WithR/G/B/A`
- `WithRG/RB/RA/GB/GA/BA`
- `WithRGB/RGA/RBA/GBA`
- `ToGray()`, `ToGrayColor()`

```csharp
Color32 c = new Color32(255, 0, 0, 255).WithG(128);
```

## Dictionary

Metodi disponibili:
- `MakeDictionary(keys[], values[])`
- `MakeDictionary(IList<TKey>, IList<TValue>)`

```csharp
var map = new[] { "a", "b" }.MakeDictionary(new[] {1, 2});
```

## GameObject

Metodi disponibili:
- `GetComponentForced<T>()`: recupera un componente cercando anche tra i figli.

```csharp
var renderer = gameObject.GetComponentForced<Renderer>();
```

## Layer

Metodi disponibili:
- `SetLayerMask(layerMask)`
- `GetLayerMask()`
- `ContainsLayerMask(layerMask)`
- `LayerMask.Add(ref mask, layer)`
- `LayerMask.Remove(ref mask, layer)`
- `LayerMask.Contains(layer)`
- `LayerMask.Contains(GameObject)`
- `LayerMask.Contains(Component)`

```csharp
gameObject.SetLayerMask(layerMask);
bool hasLayer = layerMask.Contains(gameObject);
```

## NullOrEmpty

Metodi disponibili:
- `IsNullOrEmpty(List<T>)`
- `IsNullOrEmpty(T[])`
- `IsNullOrEmpty(Dictionary<TKey, TValue>)`

```csharp
if (myList.IsNullOrEmpty()) {
    // ...
}
```

## Object

Metodi disponibili per il logging:
- `Log`, `LogWarning`, `LogError`, `LogException`
- `LogFormat`, `LogWarningFormat`, `LogErrorFormat`
- `LogIf`, `LogIfFormat`, `LogWarningIf`, `LogWarningIfFormat`, `LogErrorIf`, `LogErrorIfFormat`, `LogExceptionIf`
- `LogErrorAndThrow`, `LogErrorAndThrowFormat`, `LogErrorAndThrowException`
- `LogErrorAndThrowIf`, `LogErrorAndThrowIfFormat`, `LogErrorAndThrowExceptionIf`

```csharp
gameObject.Log("Hello world");
gameObject.LogWarningIf(condition, "Warn!");
```

## Shuffle

Metodi disponibili:
- `Shuffle(T[])`, `Shuffle(List<T>)`, `Shuffle(IDictionary<T1, T2>)`
- `PickRandom(T[])`, `PickRandom(List<T>)`
- `PickRandomAndIndex(T[])`, `PickRandomWithIndex(List<T>)`
- `GetRandomElements(List<T>, count)`
- `First(IList<T>)`, `Last(IList<T>)`

```csharp
var items = new List<int> {1, 2, 3};
items.Shuffle();
int random = items.PickRandom();
```

## Transform

Metodi disponibili:
- `SetPositionRotationScale(position, rotation, scale)` (con `Quaternion` o `Vector3` per la rotazione)
- `AddChildren(GameObject[])`, `AddChildren(Component[])`
- `ResetChildPositions(recursive)`
- `SetChildLayers(layerName, recursive)`
- `SetX(x)`, `SetY(y)`, `SetZ(z)`
- `CloserEdge(camera, width, height)`

```csharp
transform.SetX(0f);
transform.AddChildren(new[] {child1, child2});
```

## Vector

Metodi disponibili:
- `GetClosest(IEnumerable<Vector3>)`
- `XY()`
- `WithX/WithY/WithZ` e `WithAddX/WithAddY/WithAddZ`
- `NearestPointOnAxis(axisDirection, point, isNormalized)`
- `NearestPointOnLine(lineDirection, point, pointOnLine, isNormalized)`

```csharp
Vector3 pos = new(1, 2, 3);
Vector3 other = pos.WithZ(0);
Vector3 closest = pos.GetClosest(points);
```

## Common

Metodi disponibili:
- `IsInteger(float)`
- `GetNumberInString(string)`

```csharp
bool isInt = 3f.IsInteger();
int num = "level42".GetNumberInString();
```

## StringMerger

Metodi disponibili:
- `AppendPrefix(message, prefix)`
- `AppendTimeAndPrefix(message, prefix)`

```csharp
string msg = StringMerger.AppendPrefix("Ready", "INFO");
```


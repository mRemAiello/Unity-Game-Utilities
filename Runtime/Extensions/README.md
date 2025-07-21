## Lista

```cs
// Estrae l'elemento in posizione index e lo restituisce
T item = list.Pop(1);

// Aggiunge un nuovo elemento e lo restituisce
T item2 = list.Place(item);

// Estra l'elemento in posizione index, a partire dalla fine
T item = list.FromEnd(1);
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
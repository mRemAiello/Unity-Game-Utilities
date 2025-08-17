# Utilità matematiche

Questa cartella raccoglie alcune classi di supporto per operazioni matematiche e per la generazione di valori casuali.

## Classi

### `FloatMinMax`
Classe serializzabile che rappresenta un intervallo di valori in virgola mobile.
Espone le proprietà `Min` e `Max` e il metodo `GetValue()` che restituisce un numero casuale compreso tra i due estremi.

```cs
var range = new FloatMinMax { Min = 0f, Max = 10f };
float valore = range.GetValue();
```

### `IntMinMax`
Versione intera di `FloatMinMax`. Il metodo `GetValue()` restituisce un intero casuale compreso tra `Min` e `Max` inclusi.

```cs
var range = new IntMinMax { Min = 1, Max = 6 };
int lancio = range.GetValue(); // 1 - 6
```

### `MathUtility`
Classe statica che fornisce alcuni metodi di utilità:
- `Normalize(float value, float min, float max)` normalizza `value` nell'intervallo [`min`, `max`].
- `SmoothTime(float t)` applica una funzione di smoothing a `t` (atteso in [0,1]).

```cs
float percentuale = MathUtility.Normalize(valoreCorrente, minimo, massimo);
float tempoMorbidito = MathUtility.SmoothTime(t);
```

### `Point`
Semplice classe che rappresenta una coordinata bidimensionale di interi (`X`, `Y`).
Implementa `IEquatable<Point>` e ridefinisce `ToString()` restituendo la forma `(X, Y)`.

```cs
Point p = new Point(2, 3);
Debug.Log(p); // "(2, 3)"
```

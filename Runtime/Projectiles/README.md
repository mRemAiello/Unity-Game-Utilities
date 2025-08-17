# Proiettili 2D

Sistema per creare proiettili con traiettoria curva.

```cs
// Creazione manuale di un proiettile
GameObject projGameObject = Instantiate(_projPrefab, transform.position, Quaternion.identity);
if (projGameObject.TryGetComponent(out IProjectile projectile))
{
    // Imposta bersaglio, velocità massima e altezza relativa
    projectile.InitProjectile(_target.position, _projMaxMoveSpeed, _projMaxHeight, onHit: OnProjectileHit);
    // Applica le curve della traiettoria, correzione asse e velocità
    projectile.InitAnimationCurves(_trajectoryCurve, _axisCorrectionCurve, _projSpeedCurve);
}

void OnProjectileHit(Projectile2D proj)
{
    // Restituisci al pool o gestisci l'impatto
}
```

Il componente `Projectile2D` calcola la traiettoria leggendo tre `AnimationCurve`:

* **TrajectoryCurve** – descrive l'arco principale.
* **AxisCorrectionCurve** – corregge la posizione sull'asse secondario.
* **ProjSpeedCurve** – definisce l'andamento della velocità.

Quando raggiunge il bersaglio il proiettile invoca l'evento `OnHit`. Tramite questo evento è possibile restituire l'oggetto a un pool implementando `IPoolable` o attivare altri effetti.

Per un feedback visivo aggiungere `ProjectileVisual2D`, che ruota il modello seguendo la direzione e sposta l'ombra.

```cs
_projectileVisual.SetTarget(_target.position);
```

Per un uso rapido è disponibile `Shooter`, che istanzia il prefab ogni `_shootRate` secondi e inizializza automaticamente l'`IProjectile` con i parametri e le curve specificate.


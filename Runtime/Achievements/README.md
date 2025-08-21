# Sistema di Achievement

## Introduzione
Questo modulo fornisce una struttura riutilizzabile per definire, monitorare e notificare i traguardi raggiunti dal giocatore. Gli achievement sono descritti tramite asset e gestiti a runtime da un manager centrale che ne aggiorna lo stato in risposta agli eventi del gioco.

## Classi ed Enumerazioni
- **AchievementCondition**: enum che descrive la relazione logica usata per valutare un traguardo semplice (uguale, maggiore, minore, ecc.).
- **AchievementType**: enum che distingue tra achievement "Simple" (verifica immediata) e "Progress" (richiede il raggiungimento graduale di un valore).
- **AchievementData**: `ScriptableObject` che contiene le informazioni statiche di un achievement, come l'evento associato, gli identificativi delle piattaforme e le icone da mostrare. Definisce inoltre tipo, condizione e valore bersaglio.
- **AchievementManager**: gestore principale che inizializza le istanze a runtime (`RuntimeAchievement`), riceve gli eventi dal gioco e notifica il completamento o l'annullamento dei traguardi tramite eventi pubblici.
- **RuntimeAchievement**: rappresentazione dinamica di un achievement. Tiene traccia del valore corrente, verifica le condizioni di completamento e invoca il manager quando cambia stato.

## Componenti UI
`AchievementVisualizer` ascolta gli eventi di `AchievementManager` e istanzia un `AchievementNotification` quando un traguardo viene sbloccato. `AchievementNotification` si occupa di mostrare icona, titolo e descrizione dell'achievement, fornendo il collegamento tra la logica del sistema e il messaggio visivo presentato al giocatore.


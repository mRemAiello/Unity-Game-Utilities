# Salvataggio e caricamento

```cs
public class GameHud : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Tracker _tracker;

    private void Awake()
    {
        // Tracciamo i cambiamenti alla vita e monete
        _tracker = new Tracker();
        _tracker.AddNodeForValueType(() => _character.Health, UpdateHealthBar);
        _tracker.AddNodeForValueType(() => _character.Money, UpdateMoneyView);
        _tracker.ForceInvoke();
    }

    private void LateUpdate()
    {
        // Dobbiamo fare un refresh per tenere traccia dei cambiamenti
        _tracker.Refresh();
    }

    private void UpdateHealthBar(int health)
    {
        // Aggiorna...
    }

    private void UpdateMoneyView(int money)
    {
        // Aggiorna...
    }
}
```
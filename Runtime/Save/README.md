# Salvataggio e caricamento

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
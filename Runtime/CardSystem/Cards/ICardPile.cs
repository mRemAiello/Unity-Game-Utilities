using System;

namespace GameUtils
{
    public interface ICardPile
    {
        Action<ICard[]> OnPileChanged { get; set; }
        void AddCard(ICard card);
        void RemoveCard(ICard card);
    }
}
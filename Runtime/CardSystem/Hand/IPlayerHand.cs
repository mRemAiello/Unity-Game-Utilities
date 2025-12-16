using System;
using System.Collections.Generic;

namespace GameUtils
{
    public interface IPlayerHand : ICardPile
    {
        IReadOnlyList<ICard> Cards { get; }

        //
        Action<ICard> OnCardPlayed { get; set; }
        Action<ICard> OnCardSelected { get; set; }

        //
        void PlaySelected();
        void Unselect();
        void PlayCard(ICard card);
        void SelectCard(ICard card);
        void UnselectCard(ICard card);
    }
}
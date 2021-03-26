using System.Collections.Generic;
using src.model;

namespace src.card.model
{
    public static class CardRepository
    {
        public static List<Card> GetAll
            => new List<Card>
            {
                TabularCard
            };

        static Card TabularCard
            => Card.Builder
                   .Title("Tabular")
                   .Description("This card is tabular")
                   .Type(CardType.Tabular)
                   .Build();
    }
}
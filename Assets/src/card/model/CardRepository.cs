namespace src.card.model
{
    public static class CardRepository
    {
        /************** Prototypes *************/
        static Card TabularCard
            => Card.Builder
                   .Title("Tabular")
                   .Description("This card is tabular")
                   .Type(CardType.Tabular)
                   .Build();
        
        
        
        /*************** Batches ***************/
        /***************** All *****************/
        public static Card[][] AllBatches => new[]
        {
            BatchOne,
        };
        
        
        
        /**************** Waves ****************/
        public static Card[] BatchOne => new[]
        {
            Card.Builder
                .Title("Eye")
                .Description("What will you behold?")
                .Type(CardType.Eye)
                .GuaranteedDrop(true)
                .DropWeight(2)
                .Build()
        };
    }
}
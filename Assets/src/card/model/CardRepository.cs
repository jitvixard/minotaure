namespace src.card.model
{
    public static class CardRepository
    {
        /************** Prototypes *************/
        static Card BeaconCard(bool guarantee, int weight)
            => Card.Builder
                .Title("Be a true nuisance.")
                .Description("Something deep in your smooth brain is stirred by the chimes." +
                             "Attraction and deception, this could be used.")
                .Type(CardType.Beacon)
                .GuaranteedDrop(guarantee)
                .DropWeight(weight)
                .Build();
        
        static Card EyeCard(bool guarantee, int weight)
            => Card.Builder
                .Title("Take a peek.")
                .Description("A lens here and wire there." +
                             "What will you be able to behold?")
                .Type(CardType.Eye)
                .GuaranteedDrop(guarantee)
                .DropWeight(weight)
                .Build();



        /*************** Batches ***************/
        /***************** All *****************/
        public static Card[][] AllBatches => new[]
        {
            BatchOne,
            BatchTwo
        };
        
        
        
        /**************** Waves ****************/
        public static Card[] BatchOne => new[] 
        {
            BeaconCard(true, 1),
        };

        public static Card[] BatchTwo => new[]
        {
            EyeCard(true, 1),
        };
    }
}
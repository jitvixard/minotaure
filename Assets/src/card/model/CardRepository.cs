namespace src.card.model
{
    public static class CardRepository
    {
        /************** Prototypes *************/
        public static Card BeaconCard(bool guarantee, int weight)
            => Card.Builder
                .Description("You stare at the small boxes on the screen. The usual suspects. " 
                             + " They've been getting more frequent as people are bored and along." +
                             " Some only want to share how well they're doing. Others a more genuine cry" 
                             + " for companionship. Not today.")
                .Type(CardType.Beacon)
                .GuaranteedDrop(guarantee)
                .DropWeight(weight)
                .Build();
        
        public static Card EyeCard(bool guarantee, int weight)
            => Card.Builder
                .Description("In the robotic tone. [THE FIRDAY'S ARE BACK IN FORCE TONIGHT BLASEBALL FANS"
                             + " WITH THE BLESSING OF THE 404 AMONGST THEM]. Somewhere a fantasy league ticks over." 
                             + " It's rulebook has gone down. Bringing up with it two new ones and more history than"
                             + " before."
                )
                .Type(CardType.Eye)
                .GuaranteedDrop(guarantee)
                .DropWeight(weight)
                .Build();
        
        public static Card ExplosiveCard(bool guarantee, int weight)
            => Card.Builder
                .Description("It was only ventilation they'd asked for. The dust was too much to breathe in " +
                             "everyday. But you didn't think what could happen with just one small spark," +
                             " how could you? That's why you pay the supervisor after all.")
                .Type(CardType.Explosive)
                .GuaranteedDrop(guarantee)
                .DropWeight(weight)
                .Build();
        
        public static Card LureCard(bool guarantee, int weight)
            => Card.Builder
                .Description("Fuck Tonight in Mountain Ash. Genuine.")
                .Type(CardType.Lure)
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
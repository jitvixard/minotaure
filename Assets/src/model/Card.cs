namespace src.model
{
    public class Card
    {
        CardType type;

        public Card(CardType type)
        {
            this.type = type;
        }

        public static Card BlankCard()
        {
            return new Card(CardType.Blank);
        }
    }
}
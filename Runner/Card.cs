using System;
using System.Collections.Generic;

namespace Runner
{
    public class Card
    {
        public string Name { get; }
        public Action<Player> PlayerAction { get; }

        public Card(string name, Action<Player> playerAction)
        {
            Name = name;
            PlayerAction = playerAction;
        }

        public static IEnumerable<Card> CreateCommunityChestCards(Action<int> addToFreeParking, Func<int> claimFreeParking)
        {
            yield return new Card("Bank error in your favor. Collect $200.", p => p.AddMoney(200));
            yield return new Card("Doctor's fees. Pay $50.", p =>
            {
                p.DeductMoney(50);
                addToFreeParking(50);
            });
        }

        public static IEnumerable<Card> CreateChanceCards(Action<int> addToFreeParking, Func<int> claimFreeParking)
        {
            yield return new Card("Advance to \"Go\". (Collect $200)", p =>
            {
                p.MovePlayer(0); 
                p.AddMoney(200);
            });
            yield return new Card("Advance to St. Charles Place. If you pass Go, collect $200. ", p =>
            {
                if (p.BoardIndex >= 11)
                    p.AddMoney(200);
                p.MovePlayer(11);
            });
        }
    }
}
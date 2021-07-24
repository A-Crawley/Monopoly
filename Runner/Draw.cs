using System;
using System.Collections.Generic;
using System.Linq;

namespace Runner
{
    public class Draw : BoardTile
    {
        public Func<List<Card>, Card> DrawAction { get; }
        public List<Card> Cards { get; }
        
        public Draw(int boardIndex, string name, TileType tileType, Func<List<Card>, Card> drawAction, List<Card> cards) : base(boardIndex, name, tileType)
        {
            DrawAction = drawAction;
            Cards = cards;
        }

        public Card DrawCard()
            => DrawAction(Cards);

        public static IEnumerable<Draw> CreateDrawTiles(
            List<Card> communityChestCards, 
            List<Card> chanceCards)
        {
            //todo: Get out of jail free
            Func<List<Card>, Card> drawAction = cards =>
            {
                var card = cards.First();
                cards.Remove(card);
                cards.Add(card);
                return card;
            };
            
            yield return new Draw(2, "Community Chest", TileType.CommunityChest, drawAction, communityChestCards);
            yield return new Draw(17, "Community Chest", TileType.CommunityChest, drawAction, communityChestCards);
            yield return new Draw(33, "Community Chest", TileType.CommunityChest, drawAction, communityChestCards);
            
            yield return new Draw(7, "Chance", TileType.Chance, drawAction, chanceCards);
            yield return new Draw(22, "Chance", TileType.Chance, drawAction, chanceCards);
            yield return new Draw(36, "Chance", TileType.Chance, drawAction, chanceCards);
        }
    }
}
using System;
using System.Collections.Generic;

namespace Runner
{
    public class Tax : BoardTile
    {
        public Func<Player, int> TaxAction { get; }
        
        public Tax(int boardIndex, string name, TileType tileType, Func<Player, int> taxAction) : base(boardIndex, name, tileType)
        {
            TaxAction = taxAction;
        }

        public static IEnumerable<Tax> CreateTaxTiles(Action<int> freeParkingAccumulator)
        {
            yield return new Tax(4, "Income Tax", TileType.Tax, p =>
            {
                p.DeductMoney(200);
                freeParkingAccumulator(200);
                return 200;
            });
            yield return new Tax(38, "Luxury Tax", TileType.Tax, p =>
            {
                p.DeductMoney(100);
                freeParkingAccumulator(100);
                return 100;
            });
        }
    }
}
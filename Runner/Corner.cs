using System;
using System.Collections.Generic;

namespace Runner
{
    public class Corner : BoardTile
    {
        public Action<Player> PlayerAction { get; }
        
        public Corner(int boardIndex, string name, TileType tileType, Action<Player> playerAction) : base(boardIndex, name, tileType)
        {
            PlayerAction = playerAction;
        }

        public static IEnumerable<Corner> CreateCornerTiles(Func<int> claimFreeParking)
        {
            //todo: Jail func
            yield return new Corner(0, "Go", TileType.Special, p =>
            {
            });
            yield return new Corner(10, "Jail", TileType.Special, p =>
            {
                p.AddLog($"JAI | {p.Name} has landed on jail");
            });
            yield return new Corner(20, "Free Parking", TileType.Special, p =>
            {
                var money = claimFreeParking();
                p.AddMoney(money);
                p.AddLog($"FRE | {p.Name} has landed on free parking and received {money:C0}");
            });
            yield return new Corner(30, "Go To Jail", TileType.Special, p =>
            {
                p.MovePlayer(10);
                p.AddLog($"JAI | {p.Name} has been sent to jail");
            });
        }
    }
}
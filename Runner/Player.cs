using System.Collections.Generic;
using System.Linq;

namespace Runner
{
    /// <summary>
    /// The players
    /// </summary>
    public class Player
    {
        public string Name { get; }
        public int Money { get; private set; }
        public int BoardIndex { get; private set; }
        public Token Token { get; }
        private readonly List<Property> _properties = new();
        public IEnumerable<Property> Properties => _properties.OrderBy(p => p.TileType);

        public IEnumerable<TileType> Monopolies => _properties
            .Where(p => p.TileType != TileType.Special && p.TileType != TileType.Station && p.TileType != TileType.Tax && p.TileType != TileType.Utility)
            .Where(p =>
            {
                var count = _properties.Count(pr => pr.TileType == p.TileType);
                if (p.TileType is TileType.Brown or TileType.DarkBlue)
                    return count == 2;
                return count == 3;
            })
            .Select(p => p.TileType)
            .Distinct();
        public List<string> ActionLog { get; } = new();
        public Player(string name, int money, Token token)
        {
            Name = name;
            Money = money;
            Token = token;
        }

        public void AddProperty(Property property)
            => _properties.Add(property);

        public void AddLog(string log)
        {
            Logger.Log($"{Money:C0} | {log}");
            ActionLog.Add($"{Money:C0} | {log}");
        }

        public void AddMoney(int money)
            => Money += money;

        public void DeductMoney(int money)
            => Money -= money;

        public void MovePlayer(int boardIndex)
        {
            AddLog($"{BoardIndex} => {boardIndex}");
            BoardIndex = boardIndex;
        }

        public static Player CreatePlayer(int number, Token token)
        {
            return new($"Player{number}", 1500, token);
        }
    }
}
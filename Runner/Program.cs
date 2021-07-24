using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineArguments = CollectCommandLineArgs(args);
            commandLineArguments.ForEach(d => Console.WriteLine($"{{{d.Key}: {d.Value}}}"));
            Game.CreatePlayers(int.Parse(commandLineArguments[ArgumentEnum.Players]));
            Game.CreateBoardTiles();
            foreach (var i in Enumerable.Range(0, 40))
            {
                if (Round(i))
                    break;
            }

            //Game.Players.ForEach(Logger.Log);
            Logger.WriteToFile("Game",Game.Players);
        }

        static bool Round(int roundNumber)
        {
            var i = 0;
            var rolls = 1;
            while (i < Game.Players.Count)
            {
                var dice1 = Dice.Roll();
                var dice2 = Dice.Roll();
                
                Movement(dice1, dice2);
                Action(dice1 + dice2);
                Development();
                
                if (Game.Players[i].Money < 0)
                {
                    Game.Players[i].AddLog($"{roundNumber:00} | LOS | {Game.Players[i].Name} has lost");
                    return true;
                }
                
                if (dice1 != dice2)
                    i++;
            }

            return false;

            void Movement(int dice1, int dice2)
            {
                Game.Players[i].AddLog($"{roundNumber:00} | ROL | Player {Game.Players[i].Name} has rolled a {dice1+dice2} ({dice1},{dice2})");

                var moveTo = dice1 + dice2 + Game.Players[i].BoardIndex;
                
                if (moveTo == 30)
                {
                    Game.Players[i].MovePlayer(10);
                    rolls = 1;
                    return;
                }

                if (moveTo > 39)
                {
                    Game.Players[i].AddMoney(200);
                    Game.Players[i].AddLog($"{roundNumber:00} | GO  | {Game.Players[i].Name} has passed Go");
                }

                Game.Players[i].MovePlayer(moveTo % 40);
                var tile = Game.BoardTile(Game.Players[i].BoardIndex);
                //Game.Players[i].AddLog(tile.Name);
                rolls++;

                if (dice1 != dice2)
                {
                    rolls = 1;
                }
            }

            void Action(int dieNumber)
            {
                var tile = Game.BoardTile(Game.Players[i].BoardIndex);
                switch (tile.TileType)
                {
                    case TileType.Tax:
                        var tax = (Tax) tile;
                        Game.Players[i].AddLog($"{roundNumber:00} | TAX | {Game.Players[i].Name} has payed {tax.TaxAction(Game.Players[i]):C0} in tax");
                        break;
                    case TileType.Chance:
                        break;
                    case TileType.CommunityChest:
                        break;
                    case TileType.Special:
                        var special = (Corner) tile;
                        special.PlayerAction(Game.Players[i]);
                        break;
                    default:
                        var prop = (Property) tile;
                        if (prop.Owned)
                        {
                            if (prop.OwnerName() == Game.Players[i].Name)
                                break;
                            var rent = prop.TileType == TileType.Utility ? prop.RentUtility(Game.Players[i], dieNumber) : prop.Rent(Game.Players[i]);
                            Game.Players[i].AddLog($"{roundNumber:00} | REN | {prop.Name} is already owned, {Game.Players[i].Name} payed {rent:C0} rent to {prop.OwnerName()} ({prop.TileType})");
                        }
                        else
                        {
                            Game.Players[i].AddLog(prop.BuyProperty(Game.Players[i]) 
                                ? $"{roundNumber:00} | BUY | {Game.Players[i].Name} bought {prop.Name}"
                                : $"{roundNumber:00} | NBY | {Game.Players[i].Name} cannot buy {prop.Name}");
                        }
                        break;
                }
            }

            void Development()
            {
                if (!Game.Players[i].Monopolies.Any())
                    return;

                while (Game.Players[i].Money > 200)
                {
                    foreach (var property in Game.Players[i].Properties.Where(p => Game.Players[i].Monopolies.Contains(p.TileType)))
                    {
                        var bought = false;
                        if (property.Houses != 4)
                            bought = property.BuyHouse(Game.Players[i]);
                        else
                            bought = property.BuyHotel(Game.Players[i]);

                        if (!bought)
                            return;
                    }
                }
            }

        }

        static Dictionary<ArgumentEnum,string> CollectCommandLineArgs(string[] args)
        {
            if (args is null || args.Length == 0)
            {
                Console.WriteLine("Cannot run with no command line arguments...");
                Environment.Exit(1);
            }

            var arguments = new Dictionary<ArgumentEnum, string>();

            for (var i = 0; i < args.Length; i++)
            {
                if (!args[i].IsArgument(out var argument))
                    continue;
                arguments.Add(argument, args[i+1]);
                i++;
            }

            return arguments;
        }
    }

    public static class Game
    {
        public static int FreeParkingMoney { get; private set;  }
        public static List<Player> Players { get; } = new ();
        public static List<BoardTile> BoardTiles { get; } = new();
        private static List<Token> Tokens { get; } = new()
        {
            new("Hat"),
            new("Dog"),
            new("Car"),
            new ("Cat"),
            new ("Boat"),
            new ("boot")
        };

        public static void CreatePlayers(int playerCount)
        {
            if (playerCount > Tokens.Count)
                playerCount = Tokens.Count;
            
            Players.AddRange(
                Enumerable.Range(0, playerCount)
                                  .ForEach(i => Player.CreatePlayer(i, Tokens[i]))
                );
        }

        private static void AddToFreeParking(int taxes)
            => FreeParkingMoney += taxes;

        private static int ClaimFreeParking()
        {
            var temp = FreeParkingMoney;
            FreeParkingMoney = 0;
            return temp;
        }

        public static BoardTile BoardTile(int index)
            => BoardTiles.Where(b => b.BoardIndex == index).Single();

        public static void CreateBoardTiles()
        {
            BoardTiles.AddRange(Property.CreateAllProperties());
            BoardTiles.AddRange(Property.CreateAllUtilities());
            BoardTiles.AddRange(Property.CreateRailRoads());
            BoardTiles.AddRange(Tax.CreateTaxTiles(AddToFreeParking));
            BoardTiles.AddRange(Draw.CreateDrawTiles(
                Card.CreateCommunityChestCards(AddToFreeParking, ClaimFreeParking).ToList(),
                Card.CreateChanceCards(AddToFreeParking, ClaimFreeParking).ToList()));
            BoardTiles.AddRange(Corner.CreateCornerTiles(ClaimFreeParking));
        }
    }
}
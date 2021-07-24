using System;
using System.Collections.Generic;
using System.Linq;

namespace Runner
{
    /// <summary>
    /// All board properties
    /// </summary>
    public class Property : BoardTile
    {
        private Player Owner { get; set; }
        public bool Owned => Owner is not null;
        
        public int Cost { get; }
        public bool Mortgage { get; private set; }
        public int MortgageValue { get; }
        public int BuildingCost { get; }
        public int Houses { get; private set;}
        public int Hotel { get; private set; }
        private Func<Player, Player, int, int, TileType, int> RentAction { get; }

        public Property(int boardIndex, string name, int cost, int mortgageValue, int buildingCost, TileType tileType, Func<Player, Player,int, int, TileType, int> rentAction)
            : base(boardIndex, name, tileType)
        {
            Cost = cost;
            Mortgage = false;
            MortgageValue = mortgageValue;
            BuildingCost = buildingCost;
            RentAction = rentAction;
        }

        public string OwnerName()
            => Owner.Name;

        public int Rent(Player renter)
            => RentAction(Owner, renter, Houses, Hotel, TileType);

        public int RentUtility(Player renter, int dieNumber)
            => RentAction(Owner, renter, dieNumber, 0, TileType);

        public void MortgageProperty(Player player)
        {
            Mortgage = true;
            player.AddMoney(MortgageValue);
        }

        public bool UnMortgageProperty(Player player)
        {
            if (!(player.Money >= MortgageValue))
                return false;

            Mortgage = false;
            player.DeductMoney(MortgageValue);
            return true;
        }

        public bool BuyHouse(Player player)
        {
            if (player.Money < BuildingCost || Houses == 4)
                return false;

            Houses++;
            player.DeductMoney(BuildingCost);
            player.AddLog($"{player.Name} has bought a house for {Name}");
            return true;
        }

        public bool BuyHotel(Player player)
        {
            if (player.Money < BuildingCost || Houses != 4 || Hotel == 1)
                return false;

            Hotel++;
            player.DeductMoney(BuildingCost);
            player.AddLog($"{player.Name} has bought a hotel for {Name}");
            return true;
        }

        public bool BuyProperty(Player player)
        {
            if (player.Money <= Cost || Owned)
                return false;

            Owner = player;
            player.DeductMoney(Cost);
            player.AddProperty(this);
            return true;
        }

        public static IEnumerable<Property> CreateAllProperties()
        {
            //Brown
            yield return new Property(1, "Mediterranean Avenue", 60, 30,50, TileType.Brown, 
                CreatePropertyRentAction(2,10,30,90,160,250, 2));
            yield return new Property(3, "Baltic Avenue", 60, 30, 50, TileType.Brown,
                CreatePropertyRentAction(4, 20, 60, 180, 320, 450, 2));
            
            //Light Blue
            yield return new Property(6, "Oriental Avenue", 100, 50, 50, TileType.LightBlue,
                CreatePropertyRentAction(6, 30, 90, 270, 400, 550, 3));
            yield return new Property(8, "Vermont Avenue", 100, 50, 50, TileType.LightBlue,
                CreatePropertyRentAction(6, 30, 90, 270, 400, 550, 3));
            yield return new Property(9, "Connecticut Avenue", 120, 60, 50, TileType.LightBlue,
                CreatePropertyRentAction(8, 40, 100, 300, 450, 600, 3));

            //purple
            yield return new Property(11, "St. Charles Place", 140, 70, 100, TileType.Purple,
                CreatePropertyRentAction(10, 50, 150, 450, 625, 750, 3));
            yield return new Property(13, "States Avenue", 140, 70, 100, TileType.Purple,
                CreatePropertyRentAction(10, 50, 150, 450, 625, 750, 3));
            yield return new Property(14, "Virginia Avenue", 160, 80, 100, TileType.Purple,
                CreatePropertyRentAction(12, 60, 180, 500, 700, 900, 3));
            
            //Orange
            yield return new Property(16, "St. James Place", 180, 90, 100, TileType.Orange,
                CreatePropertyRentAction(14, 70, 200, 550, 750, 950, 3));
            yield return new Property(18, "Tennessee Avenue", 180, 90, 100, TileType.Orange,
                CreatePropertyRentAction(14, 70, 200, 550, 750, 950, 3));
            yield return new Property(19, "New York Avenue", 200, 100, 100, TileType.Orange,
                CreatePropertyRentAction(16, 80, 220, 600, 800, 1000, 3));
            
            //Red
            yield return new Property(21, "Kentucky Avenue", 220, 110, 150, TileType.Red,
                CreatePropertyRentAction(18, 90, 250, 700, 875, 1050, 3));
            yield return new Property(23, "Indiana Avenue", 220, 110, 150, TileType.Red,
                CreatePropertyRentAction(18, 90, 250, 700, 875, 1050, 3));
            yield return new Property(24, "Illinois Avenue", 240, 120, 150, TileType.Red,
                CreatePropertyRentAction(20, 100, 300, 750, 925, 1100, 3));
            
            //Yellow
            yield return new Property(26, "Atlantic Avenue", 260, 130, 150, TileType.Yellow,
                CreatePropertyRentAction(22, 110, 330, 800, 975, 1150, 3));
            yield return new Property(27, "Ventnor Avenue", 260, 130, 150, TileType.Yellow,
                CreatePropertyRentAction(22, 110, 330, 800, 975, 1150, 3));
            yield return new Property(29, "Marvin Gardens", 280, 140, 150, TileType.Yellow,
                CreatePropertyRentAction(24, 120, 360, 850, 1025, 1200, 3));
            
            //Green
            yield return new Property(31, "Pacific Avenue", 300, 150, 200, TileType.Green,
                CreatePropertyRentAction(26, 130, 390, 900, 1100, 1275, 3));
            yield return new Property(32, "North Carolina Avenue", 300, 150, 200, TileType.Green,
                CreatePropertyRentAction(26, 130, 390, 900, 1100, 1275, 3));
            yield return new Property(34, "Pennsylvania Avenue", 320, 160, 200, TileType.Green,
                CreatePropertyRentAction(28, 150, 450, 1000, 1200, 1400, 3));
            
            //Dark Blue
            yield return new Property(37, "Park Place", 350, 175, 200, TileType.DarkBlue,
                CreatePropertyRentAction(35, 175, 500, 1100, 1300, 1500, 2));
            yield return new Property(39, "Boardwalk", 400, 200, 200, TileType.DarkBlue,
                CreatePropertyRentAction(50, 200, 600, 1400, 1700, 2000, 2));
        }

        public static IEnumerable<Property> CreateAllUtilities()
        {
            Func<Player, Player, int, int, TileType, int> utilityAction =
                (owner, renter, dieNumber, o, set) =>
                {
                    var properties = owner.Properties.Count(p => p.TileType == TileType.Utility);
                    if (properties == 0)
                        return 0;
                    
                    var deduct = properties == 1 ? dieNumber * 4 : dieNumber * 10;
                    renter.DeductMoney(deduct);
                    owner.AddMoney(deduct);
                    return deduct;
                };
            
            yield return new Property(12, "Electric Company", 150, 75, 0, TileType.Utility, utilityAction);
            yield return new Property(28, "Water Company", 150, 75, 0, TileType.Utility, utilityAction);
        }

        public static IEnumerable<Property> CreateRailRoads()
        {
            Func<Player, Player, int, int, TileType, int> railRoadAction =
                (owner, renter, i, o, set) =>
                {
                    var properties = owner.Properties.Count(p => p.TileType == TileType.Station);
                    var deduct = properties * 25;
                    renter.DeductMoney(deduct);
                    owner.AddMoney(deduct);
                    return deduct;
                };

            yield return new Property(5, "Reading Railroad", 200, 100, 0, TileType.Station, railRoadAction);
            yield return new Property(15, "Pennsylvania Railroad", 200, 100, 0, TileType.Station, railRoadAction);
            yield return new Property(25, "B&O Railroad", 200, 100, 0, TileType.Station, railRoadAction);
            yield return new Property(35, "Short Line", 200, 100, 0, TileType.Station, railRoadAction);
        }

        public static Func<Player, Player,int,int, TileType,int> CreatePropertyRentAction(int baseRent, int oneHouse, int twoHouse, int threeHouse, int fourHouse,
            int hotelPrice, int monopolyValue)
        {
            return (owner, renter,houses, hotel, set) =>
            {
                var properties = owner.Properties.Count(p => p.TileType == set);
                var deduct = (houses, hotel) switch
                {
                    (0,_) => baseRent,
                    (1,_) => oneHouse,
                    (2,_) => twoHouse,
                    (3,_) => threeHouse,
                    (4,0) => fourHouse,
                    (_,1) => hotelPrice,
                    (_,_) => 0
                };

                if (properties == monopolyValue && deduct == baseRent)
                    deduct *= 2;
                
                renter.DeductMoney(deduct);
                owner.AddMoney(deduct);
                return deduct;
            };
        }
    }
}
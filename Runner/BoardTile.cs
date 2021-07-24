namespace Runner
{
    public class BoardTile
    {
        public int BoardIndex { get; }
        public string Name { get; }
        public TileType TileType { get; }

        public BoardTile(int boardIndex, string name, TileType tileType)
        {
            BoardIndex = boardIndex;
            Name = name;
            TileType = tileType;
        }
    }
}
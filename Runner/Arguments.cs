namespace Runner
{
    public static class Arguments
    {
        private const string Players = "-p";
        private const string Games = "-g";

        public static bool IsArgument(this string arg, out ArgumentEnum argumentEnum)
        {
            switch (arg)
            {
                case Players:
                    argumentEnum = ArgumentEnum.Players;
                    break;
                case Games:
                    argumentEnum = ArgumentEnum.Games;
                    break;
                default:
                    argumentEnum = ArgumentEnum.None;
                    return false;
            }

            return true;
        }
    }
}
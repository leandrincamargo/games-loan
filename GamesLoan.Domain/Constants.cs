namespace GamesLoan.Domain
{
    public static class Constants
    {
        public static string Secret => "fd909b06-f3f6-4757-801a-30545bb3c6f3";
        public static string[] Audiences = { "f0d2e6a6-f9a5-45ac-a820-bcbff853ade1" };
        public static string Issuer => "LoanGames";
        public static double Minutes => 90;
        public static string Administrator => "ADMIN";
        public static string Friend => "FRIEND";
    }
}


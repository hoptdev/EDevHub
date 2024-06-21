namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string hostname = "http://0.0.0.0.0";

            Builder.Run(new string[1] { $"--urls={hostname}:8444/" }, true);
        }
    }
}
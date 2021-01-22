namespace MasterArmsDiscordBot_v2
{
	class Program
	{
		static void Main(string[] args)
		{
			var bot = new MasterArmsDiscordBot_v2();
			bot.RunAsync().GetAwaiter().GetResult();
		}
	}
}

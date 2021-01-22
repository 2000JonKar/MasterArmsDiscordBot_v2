using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using System.Threading.Tasks;

namespace MasterArmsDiscordBot_v2
{
	class MasterArmsDiscordBot_v2
	{
		public DiscordClient Client { get; private set; }
		public InteractivityExtension Interactivity { get; private set; }
		public CommandsNextExtension Commands { get; private set; }

		public async Task RunAsync()
		{
			var token = ""; // Put Discord bot token here
			var config = new DiscordConfiguration 
			{
				Token = token,
				TokenType = TokenType.Bot,
				AutoReconnect = true,
				// MinimumLogLevel = LogLevel.Debug
			};

			Client = new DiscordClient(config);

			Client.Ready += OnClientReady;

			var commandsConfig = new CommandsNextConfiguration
			{
				StringPrefixes = new string[] { "!" }, // ! is the prefix for commands
				EnableDms = false, // Cannot use commands in direct message to bot
				EnableMentionPrefix = true, // Can use bot tag as alternative to prefix
				DmHelp = true // Send !help message in direct message
			};
			Commands = Client.UseCommandsNext(commandsConfig);
			Commands.RegisterCommands<CommandsList>();

			await Client.ConnectAsync().ConfigureAwait(false);

			await Task.Delay(-1);
		}
		private Task OnClientReady(object sender, ReadyEventArgs e)
		{
			return Task.CompletedTask;
		}
	}
}

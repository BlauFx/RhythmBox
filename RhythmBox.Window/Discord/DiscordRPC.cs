using DiscordRPC;
using DiscordRPC.Logging;
using System;

namespace RhythmBox.Window.Discord
{
    public static class DiscordRichPresence
	{
        public static DiscordRpcClient client;

        public static void ctor()
        {
			client = new DiscordRpcClient("659551055138521118");

			client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

			client.OnReady += (sender, e) =>
			{
				Console.WriteLine("Received Ready from user {0}", e.User.Username);
			};

			client.OnPresenceUpdate += (sender, e) =>
			{
				Console.WriteLine("Received Update! {0}", e.Presence);
			};

			client.Initialize();
		}

		public static void UpdateRPC(RichPresence rpc)
		{
			client.SetPresence(rpc);
			client.Invoke();
		}
	}
}

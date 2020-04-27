using System;
using TShockAPI;
using TerrariaApi.Server;
using Terraria;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;
using TShockAPI.Hooks;

namespace ShakePlugin.cs
{
	[ApiVersion(2, 1)]

	public class ShakePlugin : TerrariaPlugin
	{

		public override string Name => "ShakePlugin";
		public override Version Version => new Version();
		public override string Author => "Wade";
		public override string Description => "This command will allow you to shake hands with other users.";

		public ShakePlugin(Main game) : base(game) { }

		public override void Initialize()
		{
			ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
			}
			base.Dispose(disposing);
		}

		private void OnInitialize(EventArgs args)
		{
			Commands.ChatCommands.Add(new Command("shakecommand.shake", Shake, "shake")
			{
				HelpText = "Shakes hands with another user."
			});
		}
		private void Shake(CommandArgs args)
		{
			if (args.Parameters.Count != 1 )
			{
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /shake <playerName>");
				return;
			}
			string playerName = args.Parameters[0];

			var players = TShock.Utils.FindPlayer(playerName);

			if (players.Count == 0)
			{
				args.Player.SendErrorMessage("Invalid player!");
			}

			else if (players.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			}

			else
			{
				args.Player.SendInfoMessage($"You shook hands with {players[0].Name}.");

				players[0].SendSuccessMessage($"{args.Player.Name} shook hands with you!");
			}
		}

	}
}




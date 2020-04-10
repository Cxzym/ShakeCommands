using System;
using TshockAPI;
using TerrariaAPI.Server;
using Terraria;
using Newton.Json;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;
using TshockAPI.Hooks;

namespace ShakePlugin.cs
{
    [APIVersion 4,3]

    public class ShakePlugin : //TerrariaPlugin
    {
        private readonly Dictionary<string, Player> _playerlist = new Dictionary<string, Player>();
        private Config_config;
        private Random_random;

        public override string Name => "ShakePlugin";
        public override Version Version => new Version()
        public override string Author => "Wade";
        public override string Description => "This command will allow you to shake hands with other users.";

        public ShakePlugin(main game) : base(game) { }

        public override void Intialize()
        {
            _config = Config.Create();
            _random = new Random.(DateTime.Now.Millisecond);
            ServerApi.Hooks.GameIntialize.Register(this, onInitialize);
            ServerApi.Hooks.ServerJoin.Register(this, OnInitialize);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);
            GeneralHooks.ReloadEvent += OnReload;
        }
        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameIntialize.Deregister(this, OnInitialize);
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
                ServerApi.Hooks.ServerLeave.Deregister(this, OnLeave);
                GeneralHooks.ReloadEvent -= OnReload;
        
            }
            base.Dispose(disposing);
        }

        private void OnJoin (JoinEventArgs args)
        {
            if (!_playerlist.ContainsKey(Tshock.Players[args.Who]).UUID))
                _playerlist.Add(Tshock.Players[args.Who].UUID, new Player());

        }

        private void OnLeave(LeaveEventArgs args)
        {
            if (args.Who == Tshock.Players.Count(p => p != null)) return;
            if (!_playerlist.ContainsKey(Tshock.Players[args.Who].UUID))
                _playerlist.Remove(Tshock.Players[args.Who].UUID);

        }

        private void OnReload(ReloadEventArgs args)
        {
            var newConfig = Config.Read(true);
            if (newConfig != null)
                _config = newConfig;

        }

        private void OnInitialize(EventArgs args)
        {
            Commands.ChatCommands.Add(new command("shakecommand.shake", Shake, "shake")
            {
                HelpText = "Shakes hands with another user."
            });

            private void Shake(CommandArgs args)
            {
                if (!FirstCheck(args, "shake", new []{"<player>"} _config.Shake.Cooldown, c => c != 1))
                    return;
                args.Player.SendInfoMessages($"You shook hands with {"target.Name"}.);     
                TSPlayer.All.SendMessage($"{args.Player.Name} shook hands with {target.Name}.")
                Tshock.Log.Info($"{args.Player.Name} shook hands with {target,Name}.");
                if (!args.Player.Group.HasPermission("shakeplugin.nocd"))
                    _playerlist[args.Player.UUID].SetCooldown("shake", _config.ShakeCooldown); 
            }
        
        }
    }
}




using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace TreeChat
{
    /// <summary>
    /// Entry point for the entire mod.
    /// </summary>
    public class TreeChat : Mod
    {
        private ProximityMine.ProximityChat proximityChat;

        public override void Load()
        {
            Logger.Info($"[TreeChat.Load] CALLED");

            base.Load();
            this.CreateProximityChat();

            Terraria.Player.Hooks.OnEnterWorld += this.Hooks_OnEnterWorld;
        }

        private void Hooks_OnEnterWorld(Terraria.Player player)
        {
            Logger.Info($"[TreeChat.Hooks_OnEnterWorld] CALLED");

            Logger.Info($"[TreeChat.Hooks_OnEnterWorld] All players...");
            this.PrintAllPlayerData(Terraria.Main.player);

            // Hey, here is the identifier that other Terraria clients can get my player using
            // Tell ProximityChat, our local Discord ID maps to this Terraria identifier
            this.proximityChat.SetPlayerGameId(player.name);
        }

        private void PrintAllPlayerData(IReadOnlyList<Terraria.Player> players)
        {
            foreach (var player in players)
            {
                Logger.Info($"[TreeChat.Hooks_OnEnterWorld] Name: {player.name}");
                Logger.Info($"[TreeChat.PrintAllPlayerData] WhoAmI: {player.whoAmI}");
            }
        }

        private void ProximityChat_UserConnected(long userDiscordId)
        {
            Logger.Info($"[TreeChat.ProximityChat_UserConnected] CALLED");
        }

        private void ProximityChat_UserDisconnected(long userDiscordId)
        {
            Logger.Info($"[TreeChat.ProximityChat_UserDisconnected] {userDiscordId} has left the game");
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
        }

        public override void PostUpdateEverything()
        {
            base.PostUpdateEverything();

            for (int i = 0; i < Terraria.Main.player.Length; ++i)
            {
                var player = Terraria.Main.player[i];
                if (player != null && !string.IsNullOrEmpty(player.name))
                {
                    long playerDiscordId = this.proximityChat.GetPlayerDiscordId(player.name);
                    if (playerDiscordId != 0)
                    {
                        OnLogString($"Updating player {player.name} with position {player.position.X}, {player.position.Y}");
                        this.proximityChat.SetPlayerPosition(playerDiscordId, player.position.X, player.position.Y, 0);
                    }
                    else
                    {
                        OnLogString($"Skipping player {player.name}, no discord ID mapping yet");
                    }
                }
            }

            this.proximityChat.Update();
        }

        public override void Close()
        {
            Logger.Info($"[TreeChat.Close] CALLED");
            base.Close();
        }

        public override void Unload()
        {
            Logger.Info($"[TreeChat.Unload] CALLED");
            base.Unload();

            ProximityMine.ProximityChat.LogInfo -= OnLogString;
            this.proximityChat.UserConnected -= this.ProximityChat_UserConnected;
            this.proximityChat.UserDisconnected -= this.ProximityChat_UserDisconnected;
            this.proximityChat.Dispose();
            this.proximityChat = null;

            Terraria.Player.Hooks.OnEnterWorld -= this.Hooks_OnEnterWorld;
        }

        private void CreateProximityChat()
        {
            ProximityMine.ProximityChat.LogInfo += OnLogString;

            this.proximityChat = new ProximityMine.ProximityChat();
            this.proximityChat.Initialize();

            this.proximityChat.SetLobbyCapacity(8);

            this.proximityChat.VoiceMinDistance = 10;
            this.proximityChat.VoiceMaxDistance = 100;

            this.proximityChat.UserConnected += this.ProximityChat_UserConnected;
            this.proximityChat.UserDisconnected += this.ProximityChat_UserDisconnected;
        }

        private void OnLogString(string message)
        {
            Logger.Info($"[TreeChat.OnLogString] {message}");
        }
    }
}
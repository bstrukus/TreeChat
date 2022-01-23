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
        private List<Terraria.Player> players;

        public override void Load()
        {
            Logger.Info($"(bstru)[TreeChat.Load] CALLED");

            base.Load();

            this.players = new List<Terraria.Player>();


            this.CreateProximityChat();

            Terraria.Player.Hooks.OnEnterWorld += this.Hooks_OnEnterWorld;
        }

        private void Hooks_OnEnterWorld(Terraria.Player player)
        {
            Logger.Info($"(bstru)[TreeChat.Hooks_OnEnterWorld] CALLED");
            this.players.Add(player);

            Logger.Info($"(bstru)[TreeChat.Hooks_OnEnterWorld] All players...");
            this.PrintAllPlayerData(this.players);

            //this.proximityChat.Update();

        }

        private void PrintAllPlayerData(List<Terraria.Player> players)
        {
            foreach (var player in players)
            {
                Logger.Info($"(bstru)[TreeChat.Hooks_OnEnterWorld] Name: {player.name}");
                Logger.Info($"(bstru)[TreeChat.PrintAllPlayerData] WhoAmI: {player.whoAmI}");
            }
        }

        private void ProximityChat_UserConnected(long userDiscordId)
        {
            Logger.Info($"(bstru)[TreeChat.ProximityChat_UserConnected] CALLED");
        }

        private void ProximityChat_UserDisconnected(long userDiscordId)
        {
            Logger.Info($"(bstru)[TreeChat.ProximityChat_UserDisconnected] {userDiscordId} has left the game");
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

//             if (this.proximityChat == null)
//             {
//                 this.CreateProximityChat();
//             }
// 
//             this.proximityChat.Update();

            //             foreach (var player in this.players)
            //             {
            //                 Logger.Info($"(bstru)[TreeChat.UpdateUI] {player.name}: {player.position}");
            //             }
        }

        public override void PostUpdateEverything()
        {
            base.PostUpdateEverything();

//             foreach (var player in this.players)
//             {
//                 this.proximityChat.SetPlay
//             }

            this.proximityChat.Update();
        }

        public override void Close()
        {
            Logger.Info($"(bstru)[TreeChat.Close] CALLED");
            base.Close();
        }

        public override void Unload()
        {
            Logger.Info($"(bstru)[TreeChat.Unload] CALLED");
            base.Unload();

            this.proximityChat.UserConnected -= this.ProximityChat_UserConnected;
            this.proximityChat.UserDisconnected -= this.ProximityChat_UserDisconnected;

            Terraria.Player.Hooks.OnEnterWorld -= this.Hooks_OnEnterWorld;
        }
       
        private void CreateProximityChat()
        {
            this.proximityChat = new ProximityMine.ProximityChat();
            this.proximityChat.Initialize();
            // Hey, here is the identifier that other Terraria clients can get my player using
            // Tell ProximityChat, our local Discord ID maps to this Terraria identifier
            this.proximityChat.SetLobbyCapacity(8);

            this.proximityChat.UserConnected += this.ProximityChat_UserConnected;
            this.proximityChat.UserDisconnected += this.ProximityChat_UserDisconnected;

            //this.setPL

            ProximityMine.ProximityChat.LogInfo += OnLogString;
        }

        private void OnLogString(string message)
        {
            Logger.Info($"(bstru)[TreeChat.OnLogString] {message}");
        }
    }
}
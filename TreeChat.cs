using Microsoft.Xna.Framework;
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
            base.Load();

            this.proximityChat = new ProximityMine.ProximityChat();
            this.proximityChat.Initialize();
            // Hey, here is the identifier that other Terraria clients can get my player using
            // Tell ProximityChat, our local Discord ID maps to this Terraria identifier
            this.proximityChat.SetLobbyCapacity(8);

            this.proximityChat.UserConnected += this.ProximityChat_UserConnected;
            this.proximityChat.UserDisconnected += this.ProximityChat_UserDisconnected;

            Terraria.Player.Hooks.OnEnterWorld += this.Hooks_OnEnterWorld;
        }

        private void Hooks_OnEnterWorld(Terraria.Player obj)
        {
            throw new System.NotImplementedException();
        }

        private void ProximityChat_UserConnected(long userDiscordId)
        {
            Logger.Info($"User {userDiscordId} has entered the game.");
        }

        private void ProximityChat_UserDisconnected(long userDiscordId)
        {
            Logger.Info($"User {userDiscordId} has left the game.");
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

            this.proximityChat.Update();

            // Do Discord shit
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Unload()
        {
            base.Unload();

            this.proximityChat.UserConnected -= this.ProximityChat_UserConnected;
            this.proximityChat.UserDisconnected -= this.ProximityChat_UserDisconnected;

            Terraria.Player.Hooks.OnEnterWorld -= this.Hooks_OnEnterWorld;
        }

    }

    /*
    internal class ExampleGame
    {
        private List<Player> _players = new List<Player>();
        private ProximityMine.ProximityChat _proximityChat;

        public void Initialize()
        {
            _proximityChat = new ProximityMine.ProximityChat();
            _proximityChat.Initialize();

            _proximityChat.UserConnected += OnUserConnected;
            _proximityChat.UserDisconnected += OnUserDisconnected;
        }

        public void Uninitialize()
        {
            _proximityChat.UserConnected -= OnUserConnected;
            _proximityChat.UserDisconnected -= OnUserDisconnected;
        }

        public void GameLoop()
        {
            // Pump the event look to ensure all callbacks continue to get fired.
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                while (true)
                {
                    float dt = timer.ElapsedTicks / (float)TimeSpan.TicksPerSecond;
                    timer.Restart();

                    _proximityChat.Update();
                    Thread.Sleep(1000 / 60);
                }
            }
            finally
            {
                _proximityChat.Dispose();
            }
        }

        private void OnUserConnected(long userId)
        {
            Player player = new Player();
            player.Id = userId;

            _players.Add(player);

            Console.Write($"Player connected: {userId}");
        }

        private void OnUserDisconnected(long userId)
        {
            Player player = GetPlayer(userId);
            if (player != null)
            {
                _players.Remove(player);
            }

            Console.Write($"Player disconnected: {userId}");
        }

        private Player GetPlayer(long playerId)
        {
            for (int i = 0; i < _players.Count; ++i)
            {
                if (_players[i].Id == playerId)
                    return _players[i];
            }

            return null;
        }
    }
    */
}
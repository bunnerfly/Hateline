using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Celeste.Mod.CelesteNet.Client;
using Celeste.Mod.CelesteNet.Client.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Hateline.CelesteNet
{
    public class CelesteNetHatComponent : GameComponent
    {
        protected readonly CelesteNetClientModule _clientModule;
        private Delegate _initHook;
        private Delegate _disposeHook;

        private ConcurrentQueue<Action> _updateQueue = new ConcurrentQueue<Action>();

        public CelesteNetClient Client => _clientModule.Context?.Client;

        public CelesteNetHatComponent(Game game) : base(game)
        {
            _clientModule = (CelesteNetClientModule)Everest.Modules.FirstOrDefault(m => m is CelesteNetClientModule);
            if (_clientModule == null) throw new Exception("CelesteNet not loaded???");
            
            EventInfo startEvent = typeof(CelesteNetClientContext).GetEvent("OnStart");
            if (startEvent.EventHandlerType.GenericTypeArguments.FirstOrDefault() == typeof(CelesteNetClientContext))
                startEvent.AddEventHandler(null, _initHook = (Action<CelesteNetClientContext>)(_ => clientStart()));
            else
                startEvent.AddEventHandler(null, _initHook = (Action<object>)(_ => clientStart()));

            EventInfo disposeEvent = typeof(CelesteNetClientContext).GetEvent("OnDispose");
            if (disposeEvent.EventHandlerType.GenericTypeArguments.FirstOrDefault() == typeof(CelesteNetClientContext))
                disposeEvent.AddEventHandler(null, _disposeHook = (Action<CelesteNetClientContext>)(_ => clientDisposed()));
            else
                disposeEvent.AddEventHandler(null, _disposeHook = (Action<object>)(_ => clientDisposed()));
            
        }
        
        private void clientDisposed()
        {
        }

        private void clientStart()
        {
            try
            {
                SendPlayerHat();
                Logger.Log(LogLevel.Verbose, "Hateline", $"clientStart: Called SendPlayerHat at CelesteNetClientContext.OnStart with {Client}");
            } catch
            {
                // if this threw an exception, CelesteNetClient.Start would actually fail
                Logger.Log(LogLevel.Warn, "Hateline", $"clientStart: Something went wrong while trying to SendPlayerHat at CelesteNetClientContext.OnStart");
            }
        }

        public override void Update(GameTime gameTime)
        {
            var queue = _updateQueue;
            _updateQueue = new ConcurrentQueue<Action>();
            foreach (var action in queue) action();

            base.Update(gameTime);

            if (Engine.Scene == null)
                return;

            foreach (Ghost ghost in Engine.Scene.Tracker.GetEntities<Ghost>())
            {
                if (ghost.Get<HatComponent>() == null)
                {
                    ghost.Add(new HatComponent());
                }
            }
        }

        /*
        public void Handle(CelesteNetConnection connection, DataPlayerHat data) => _updateQueue.Enqueue(() =>
        {
            var ghost = Engine.Scene?.Tracker
                .GetEntities<Ghost>()
                .FirstOrDefault(e => (e as Ghost).PlayerInfo.ID == data.Player.ID);
            if (ghost == null) return;

            ghost.Add(new HatComponent());
        });*/

        public void SendPlayerHat()
        {
            if (Client == null) return;

            Client.SendAndHandle(new DataPlayerHat
            {
                CrownX = HatelineModule.Instance.CurrentX,
                CrownY = HatelineModule.Instance.CurrentY,
                SelectedHat = HatelineModule.Instance.CurrentHat,
                Player = Client.PlayerInfo,
            });
        }
    }
}
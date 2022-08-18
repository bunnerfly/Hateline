using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeste.Mod;
using Celeste.Mod.Hateline;
using Monocle;
using System.Collections.Concurrent;
using Celeste.Mod.CelesteNet.Client;
using Celeste.Mod.CelesteNet.Client.Entities;
using System.Reflection;
using Celeste.Mod.CelesteNet;

namespace Celeste.Mod.Hateline.CelesteNet
{
    internal class CelesteNetHatComponent : GameComponent
    {
        private readonly CelesteNetClientModule _clientModule;
        private Delegate _initHook;
        private Delegate _disposeHook;

        private ConcurrentQueue<Action> _updateQueue = new ConcurrentQueue<Action>();

        public CelesteNetHatComponent(Game game) : base(game)
        {
            _clientModule = (CelesteNetClientModule)Everest.Modules.FirstOrDefault(m => m is CelesteNetClientModule);
            if (_clientModule == null) throw new Exception("CelesteNet not loaded???");

            EventInfo initEvent = typeof(CelesteNetClientContext).GetEvent("OnInit");
            if (initEvent.EventHandlerType.GenericTypeArguments.FirstOrDefault() == typeof(CelesteNetClientContext))
                initEvent.AddEventHandler(null, _initHook = (Action<CelesteNetClientContext>)(_ => clientInit(_clientModule.Context.Client)));
            else
                initEvent.AddEventHandler(null, _initHook = (Action<object>)(_ => clientInit(_clientModule.Context.Client)));

            EventInfo disposeEvent = typeof(CelesteNetClientContext).GetEvent("OnDispose");
            if (disposeEvent.EventHandlerType.GenericTypeArguments.FirstOrDefault() == typeof(CelesteNetClientContext))
                disposeEvent.AddEventHandler(null, _disposeHook = (Action<CelesteNetClientContext>)(_ => clientDisposed()));
            else
                disposeEvent.AddEventHandler(null, _disposeHook = (Action<object>)(_ => clientDisposed()));
        }

        private void clientDisposed()
        {
        }

        private void clientInit(CelesteNetClient client)
        {
            client.Data.RegisterHandlersIn(this);
        }

        public override void Update(GameTime gameTime)
        {
            var queue = _updateQueue;
            _updateQueue = new ConcurrentQueue<Action>();
            foreach (var action in queue) action();

            base.Update(gameTime);
        }

        public void Handle(CelesteNetConnection connection, DataPlayerHat data) => _updateQueue.Enqueue(() =>
        {
            var ghost = Engine.Scene?.Tracker
                .GetEntities<Ghost>()
                .FirstOrDefault(e => (e as Ghost).PlayerInfo.ID == data.Player.ID);
            if (ghost == null) return;

            ghost.Add(new HatComponent(data.SelectedHat, data.CrownX, data.CrownY));
        });

        public void SendPlayerHat(int _CrownX, int _CrownY, string _SelectedHat)
        {
            var client = _clientModule.Context?.Client;
            if (client == null) return;

            client.SendAndHandle(new DataPlayerHat
            {
                CrownX = _CrownX,
                CrownY = _CrownY,
                SelectedHat = _SelectedHat, 
                Player = client.PlayerInfo,
            });
        }
    }
}
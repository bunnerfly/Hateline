using Celeste.Mod.CelesteNet;
using Celeste.Mod.CelesteNet.DataTypes;

namespace Celeste.Mod.Hateline.CelesteNet
{
    public class DataPlayerHat : DataType<DataPlayerHat>
    {
        static DataPlayerHat()
        {
            DataID = $"Hateline_PlayerHat";
        }

        public override DataFlags DataFlags => DataFlags.CoreType;

        public DataPlayerInfo Player;

        public int CrownX = HatelineModule.Settings.CrownX;
        public int CrownY = HatelineModule.Settings.CrownY;
        public string SelectedHat = HatelineModule.Settings.SelectedHat;

        public override bool FilterHandle(DataContext ctx) => Player != null;
        public override MetaType[] GenerateMeta(DataContext ctx) => new MetaType[]
        {
            new MetaPlayerPrivateState(Player),
            new MetaBoundRef(DataType<DataPlayerInfo>.DataID, Player?.ID ?? uint.MaxValue, true),
        };

        public override void FixupMeta(DataContext ctx)
        {
            Player = Get<MetaPlayerPrivateState>(ctx);
            Get<MetaBoundRef>(ctx).ID = Player?.ID ?? uint.MaxValue;
        }

        protected override void Read(CelesteNetBinaryReader reader)
        {
            var protocolVersion = reader.ReadInt32();
            //Console.WriteLine("Read ProtocolVersion");
            CrownX = reader.ReadInt32();
            //Console.WriteLine("Read CrownX");
            CrownY = reader.ReadInt32();
            //Console.WriteLine("Read CrownY");
            SelectedHat = reader.ReadString();
            //Console.WriteLine("Read SelectedHat");
        }

        protected override void Write(CelesteNetBinaryWriter writer)
        {
            writer.Write(CelesteNetSupport.PROTOCOL_VERSION);
            //Console.WriteLine("Wrote ProtocolVersion");
            writer.Write(CrownX);
            //Console.WriteLine("Wrote CrownX");
            writer.Write(CrownY);
            //Console.WriteLine("Wrote CrownY");
            writer.Write(SelectedHat);
            //Console.WriteLine("Wrote SelectedHat");
        }
    }
}
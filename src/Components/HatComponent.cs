using Celeste.Mod.CelesteNet.Client.Entities;
using Celeste.Mod.Hateline.CelesteNet;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Hateline
{
    [Tracked(true)]
    public class HatComponent : Sprite
    {
        public string CrownSprite;

        public int CrownX, CrownY;

        public PlayerHair playerHair => Entity.Get<PlayerHair>();
        public PlayerSprite playerSprite => Entity.Get<PlayerSprite>();

        public HatComponent(string hatSprite = HatelineModule.DEFAULT, int? crownX = null, int? crownY = null) : base(null, null)
        {
            CreateHat(hatSprite, true);
            if (crownX != null && crownY != null)
                SetPosition(crownX.Value, crownY.Value);
            UpdatePosition();
        }

        public override void Update()
        {
            base.Update();

            if (CelesteNetSupport.loaded && Entity.GetType().FullName == "Celeste.Mod.CelesteNet.Client.Entities.Ghost")
            {
                UpdateForGhost();
            }
            else
            {
                Visible = HatelineModule.Instance.ShouldShowHat;

                if (!HatelineModule.Instance.ShouldShowHat)
                    return;

                SetPosition(HatelineModule.Instance.CurrentX, HatelineModule.Instance.CurrentY);
                UpdatePosition();
            }

            if (Entity == null || playerSprite == null || playerHair == null)
                return;

            FlipX = playerHair.Facing == Facings.Left;
            FlipY = GravityHelperImports.IsActorInverted?.Invoke(Entity as Actor) ?? false;
            Visible = playerSprite.CurrentAnimationID != "dreamDashIn" && playerSprite.CurrentAnimationID != "dreamDashLoop";
        }

        public void UpdateForGhost()
        {
            Ghost ghost = Entity as Ghost;
            DataPlayerHat hatData = null;

            CelesteNetSupport.CNetComponent?.Client?.Data?.TryGetBoundRef(ghost.PlayerInfo, out hatData);
            if (ghost == null || hatData == null)
                return;

            CreateHat(hatData.SelectedHat);
            SetPosition(hatData.CrownX, hatData.CrownY);
            UpdatePosition();
        }

        public void SetPosition(int x, int y)
        {
            CrownX = x;
            CrownY = y;
        }

        public void UpdatePosition()
        {
            if (Entity == null || playerSprite == null || playerHair == null)
                return;
            bool flipped = GravityHelperImports.IsActorInverted?.Invoke(Entity as Actor) ?? false;

            Position = new Vector2(CrownX, CrownY - (flipped ? Height : 0))
                + playerSprite.HairOffset * new Vector2((float)playerHair.Facing, 1)
                + new Vector2(0f, -2f);
            Position.Y *= flipped ? -playerSprite.Scale.Y : playerSprite.Scale.Y;
        }

        public void CreateHat(string hatSprite, bool forceCreate = false)
        {
            if (hatSprite != null && hatSprite == CrownSprite && !forceCreate)
                return;

            try
            {
                GFX.SpriteBank.CreateOn(this, "hateline_" + hatSprite);
                CrownSprite = hatSprite;
            }
            catch
            {
                GFX.SpriteBank.CreateOn(this, "hateline_" + HatelineModule.DEFAULT);
                CrownSprite = HatelineModule.DEFAULT;
            }
        }
    }
}

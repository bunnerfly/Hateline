using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Hateline;

[Tracked(true)]
public class HatComponent : Sprite
{
    public string CrownSprite;

    public int CrownX, CrownY;
    private int _crownOffsetX, _crownOffsetY;

    public PlayerHair playerHair => Entity.Get<PlayerHair>();
    
    public PlayerSprite playerSprite => Entity.Get<PlayerSprite>();

    public HatComponent(string hatSprite = HatelineModule.HAT_NONE, int? crownX = null, int? crownY = null) : base(null, null)
    {
        CreateHat(hatSprite, true);
        if (crownX != null && crownY != null)
            SetPosition(crownX.Value, crownY.Value);
    }
    
    public override void Render()
    {
        if (Entity == null || playerHair == null) return;
        
        int gravityFlipMult = (GravityHelperImports.IsActorInverted?.Invoke(Entity as Actor) ?? false) ? -1 : 1;
        // The new position system defaults to the hair position instead of the player position which is 7 pixels lower.
        Vector2 backwardsCompatibilityOffset = new Vector2(0, 7) * gravityFlipMult;
        
        // HairFrame 2 (bangs02.png) is used when Madeline's head (and her hair) looks backwards, while Madeline faces forwards. Since the difference is in the texture itself it's hard to detect it directly.
        // There is no guarantee that a skin's bangs02.png is the flipped hair, but it stands true for most skins. 
        int lookingBackwardsMult = (playerHair.Sprite.HairFrame == 2 && GetAttribute("flip") == "true") ? -1 : 1;
        
        // PublicGetHairScale gets the scale of the player's hair. It also flips it according to the player's facing.
        Scale = playerHair.PublicGetHairScale(0);
        Scale.Y *= gravityFlipMult;
        Scale.X *= lookingBackwardsMult;
        
        RenderPosition = playerHair.Nodes[0] + backwardsCompatibilityOffset;
        RenderPosition += new Vector2((CrownX + _crownOffsetX) * Scale.X , (CrownY + _crownOffsetY) * gravityFlipMult);
        
        if (GetAttribute("flip") == "false") Scale.X = Math.Abs(Scale.X);
        if (GetAttribute("scaling") == "false" || !HatelineModule.Settings.HatScaling) Scale = Scale.Sign();
        if (GetAttribute("tint") == "true") Color = playerHair.GetHairColor(0);
        
        base.Render();
    }
    
    public override void Update()
    {
        base.Update();
        if (Entity == null || playerSprite == null) return;
        
        Visible = playerSprite.CurrentAnimationID != "dreamDashIn" && playerSprite.CurrentAnimationID != "dreamDashLoop";
    }

    public void SetPosition(int x, int y)
        => (CrownX, CrownY) = (x, y);
    
    public void CreateHat(string hatSprite, bool forceCreate = false)
    {
        if (hatSprite != null && hatSprite == CrownSprite && !forceCreate)
            return;

        Color = Color.White;
        try
        {
            GFX.SpriteBank.CreateOn(this, "hateline_" + hatSprite);
            CrownSprite = hatSprite;
        }
        catch
        {
            GFX.SpriteBank.CreateOn(this, "hateline_" + HatelineModule.HAT_NONE);
            CrownSprite = HatelineModule.HAT_NONE;
        }
        
        string hatOffset = GetAttribute("hatOffset");
        try {
            int[] crownOffset = hatOffset.Split(',').Select(int.Parse).ToArray();
            _crownOffsetX = crownOffset[0];
            _crownOffsetY = crownOffset[1];
        }
        catch { throw new ArgumentException($"Invalid hatOffset '{hatOffset}' in hat 'hateline_{CrownSprite}'."); }
    }

    public string GetAttribute(string attribute)
        => HatelineModule.Instance.HatAttributes[CrownSprite][attribute];
}

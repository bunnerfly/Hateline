using System;
using System.Collections.Generic;
using FMOD.Studio;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;
using Celeste;
using Celeste.Mod.CelesteNet.Client.Entities;
using System.Linq;

namespace Celeste.Mod.Hateline
{
	[Tracked(true)]
	public class HatComponent : Sprite
	{
		public string crownSprite;
		// protected virtual Facings PlayerFacing => (Entity as Player)?.Facing ?? Facings.Left;

		public HatComponent(string hatSprite, int crownX, int crownY) : base(null, null)
		{
			CreateHat(hatSprite);
		}

		public override void Update()
		{
			base.Update();

			if (!HatelineModule.Settings.Enabled)
			{
				Visible = false;
			}
			else
			{
				Visible = true;
			}

			var hair = Entity.Get<PlayerHair>();
			var sprite = Entity.Get<PlayerSprite>();
			var facing = hair.Facing;

			bool flipped = GravityHelperImports.IsActorInverted?.Invoke(Entity as Actor) ?? false;

			if (HatelineModule.Session.MapForcedHat != null && HatelineModule.Settings.AllowMapChanges)
			{
				if (flipped)
				{
					Position = new Vector2(
						HatelineModule.Session.mapsetX,
						-HatelineModule.Session.mapsetY + Height * sprite.Scale.Y)
						+ sprite.HairOffset * new Vector2((float)facing, -1f) + new Vector2(0f, 2f);
				}
				else
				{
					Position = new Vector2(
						HatelineModule.Session.mapsetX,
						HatelineModule.Session.mapsetY * sprite.Scale.Y)
						+ sprite.HairOffset * new Vector2((float)facing, 1f) + new Vector2(0f, -2f);
				}
				this.FlipX = facing == Facings.Left;
				this.FlipY = flipped ? true : false;
				Visible = sprite.CurrentAnimationID != "dreamDashIn" && sprite.CurrentAnimationID != "dreamDashLoop";
			}
			else
            {
				if (HatelineModule.Settings.Enabled)
				{
					if (flipped)
					{
						Position = new Vector2(
							HatelineModule.Settings.CrownX,
							-HatelineModule.Settings.CrownY + Height * sprite.Scale.Y)
							+ sprite.HairOffset * new Vector2((float)facing, -1f) + new Vector2(0f, 2f);
					}
					else
					{
						Position = new Vector2(
							HatelineModule.Settings.CrownX,
							HatelineModule.Settings.CrownY * sprite.Scale.Y)
							+ sprite.HairOffset * new Vector2((float)facing, 1f) + new Vector2(0f, -2f);
					}
					this.FlipX = facing == Facings.Left;
					this.FlipY = flipped ? true : false;
					Visible = sprite.CurrentAnimationID != "dreamDashIn" && sprite.CurrentAnimationID != "dreamDashLoop";
				}
			}
		}

		public void CreateHat(string hatSprite)
		{
			if (HatelineModule.Session.MapForcedHat == null)
			{
				try
				{
					GFX.SpriteBank.CreateOn(this, "hateline_" + hatSprite);
					crownSprite = hatSprite;
				}
				catch
				{
					GFX.SpriteBank.CreateOn(this, "hateline_none");
				}
			}
			else
			{
				try
				{
					GFX.SpriteBank.CreateOn(this, "hateline_" + HatelineModule.Session.MapForcedHat);
					crownSprite = hatSprite;
				}
				catch
				{
					GFX.SpriteBank.CreateOn(this, "hateline_none");
				}
			}
			Position = new Vector2(0f, -13f);
		}

			// test
		public static void ReloadHat(string hat, bool inGame, int x, int y)
		{
			if (inGame && HatelineModule.Settings.Enabled)
			{
				Sprite playerHatComponent = (Sprite)Engine.Scene.Tracker.GetComponents<HatComponent>().FirstOrDefault(c => c.Entity is Player);
				Player player = Engine.Scene.Tracker.GetEntity<Player>();
				PlayerHair hair = player.Get<PlayerHair>();
				var facing = hair.Facing;

				bool flipped = GravityHelperImports.IsActorInverted?.Invoke(player) ?? false;

				if (HatelineModule.Session.MapForcedHat != null && HatelineModule.Settings.AllowMapChanges)
				{
					try
                    {
						if (flipped)
						{
							GFX.SpriteBank.CreateOn(playerHatComponent, "hateline_" + HatelineModule.Session.MapForcedHat).Position = new Vector2(
								x,
								-y + playerHatComponent.Height * player.Sprite.Scale.Y)
								+ player.Sprite.HairOffset * new Vector2((float)facing, -1f) + new Vector2(0f, 2f);
						}
						else
						{
							GFX.SpriteBank.CreateOn(playerHatComponent, "hateline_" + HatelineModule.Session.MapForcedHat).Position = new Vector2(
								x,
								y * player.Sprite.Scale.Y)
								+ player.Sprite.HairOffset * new Vector2((float)facing, 1f) + new Vector2(0f, -2f);
						}
					}
					catch
                    {
						GFX.SpriteBank.CreateOn(playerHatComponent, "hateline_none").Position = new Vector2(
							x,
							y * player.Sprite.Scale.Y)
							+ player.Sprite.HairOffset * new Vector2((float)facing, 1f) + new Vector2(0f, -2f);
					}
					HatelineModule.Session.mapsetX = x;
					HatelineModule.Session.mapsetY = y;
				}
				else
				{
					if (flipped)
					{
						GFX.SpriteBank.CreateOn(playerHatComponent, "hateline_" + hat).Position = new Vector2(
							HatelineModule.Settings.CrownX,
							-HatelineModule.Settings.CrownY + playerHatComponent.Height * player.Sprite.Scale.Y)
							+ player.Sprite.HairOffset * new Vector2((float)facing, -1f) + new Vector2(0f, 2f);
					}
					else
					{
						GFX.SpriteBank.CreateOn(playerHatComponent, "hateline_" + hat).Position = new Vector2(
							HatelineModule.Settings.CrownX,
							HatelineModule.Settings.CrownY * player.Sprite.Scale.Y)
							+ player.Sprite.HairOffset * new Vector2((float)facing, 1f) + new Vector2(0f, -2f);
					}
				}
			}

			HatelineModule.Settings.SelectedHat = hat;
			HatelineModule.currentHat = hat;
		}
	}
}

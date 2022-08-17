using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.ModInterop;

namespace Celeste.Mod.Hateline
{
	[ModImportName("GravityHelper")]
	public static class GravityHelperImports
	{
		public static Func<bool> IsPlayerInverted;

		public static Func<Actor, bool> IsActorInverted;
	}
}

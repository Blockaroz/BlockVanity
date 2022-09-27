using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Common.Metaballs
{
    public abstract class MetaballSystem : ModType
    {
        public virtual void DrawToTarget(SpriteBatch spriteBatch) { }

        public RenderTarget2D renderTarget;

        protected override void Register()
        {
            ModTypeLookup<MetaballSystem>.Register(this);
            MetaballLoader.metaball.Add(this);
        }
    }
}

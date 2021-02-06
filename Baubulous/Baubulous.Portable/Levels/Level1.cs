using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Baubulous.Portable.GameLogic;
using Baubulous.Portable.GameObjects;

namespace Baubulous.Portable.Levels
{
    public class Level1 : TowerMapLevel
    {
        BaublePlayer bauble;

        protected override string TowerTexture
        {
            get
            {
                return "brickwork";
            }
        }

        protected override IList<IGameItem> CreateLevelItems()
        {
            bauble = new BaublePlayer(GameState);

            bauble.Init(graphics, content, new BaubleInitParams()
            {
                radius = 0.3f,
                texture = "shiny",
                start = new Vector3(0.0f, -2.75f, 2.5f)
            });

            var platform1 = new Platform();
            platform1.Init(graphics, content, new PlatformDefinition()
            {
                cX = 0.0f,
                cY = 0.0f,
                topZ = 1.0f,
                height = 0.1f,
                min_radius = 2.0f,
                max_radius = 3.5f,
                start_angle = 0.0001f,
                end_angle = (float)Math.PI * 2.0f - 0.0001f,
                long_side_texture = "platform_side",
                top_texture = "platform_surface",
                short_side_texture = null,
                underside_texture = null
            });

            var platform2 = new Platform();
            platform2.Init(graphics, content, new PlatformDefinition()
            {
                cX = 0.0f,
                cY = 0.0f,
                topZ = 4.0f,
                height = 0.1f,
                min_radius = 2.0f,
                max_radius = 3.5f,
                start_angle = 1.0f,
                end_angle = 2.0f,
                long_side_texture = "platform_side",
                top_texture = "platform_surface",
                short_side_texture = null,
                underside_texture = null
            });

 
            var all_items = new List<IGameItem>()
            {
                platform1,
                //platform2,
                bauble
            };

            int collectibles = 20;
            float spacing = 0.8f;
            var dx = ((Math.PI * 2) - (spacing * 2)) / collectibles;
            for (int i = 0; i < collectibles; i++)
            {
                var x = spacing + (dx * i);

                var collectible = new BaubleCollectible(GameState);
                collectible.Init(graphics, content, new BaubleInitParams()
                {
                    radius = 0.1f,
                    texture = "shiny",
                    start = new Vector3((float)x, -2.75f, 1.5f)
                });

                all_items.Add(collectible);
            }

            Focus = bauble;
            player = bauble;

            return all_items;
        }

    }
}

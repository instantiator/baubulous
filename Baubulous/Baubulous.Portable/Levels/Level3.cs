using Baubulous.Portable.GameLogic;
using Baubulous.Portable.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.Levels
{
    public class Level3 : TowerMapLevel
    {
        BaublePlayer bauble;

        protected override string TowerTexture
        {
            get
            {
                return "bluebrick";
            }
        }

        protected override IList<IGameItem> CreateLevelItems()
        {
            bauble = new BaublePlayer(GameState);

            bauble.Init(graphics, content, new BaubleInitParams()
            {
                radius = 0.3f,
                texture = "shiny",
                start = new Vector3(0.0f, -2.75f, 1.8f)
            });

            var platform0 = new Platform();
            platform0.Init(graphics, content, new PlatformDefinition()
            {
                cX = 0.0f,
                cY = 0.0f,
                topZ = 1.0f,
                height = 0.1f,
                min_radius = 2.0f,
                max_radius = 3.5f,
                start_angle = 5.0f,
                end_angle = ((float)Math.PI * 2f) - 0.0001f,
                long_side_texture = "platform_side",
                top_texture = "platform_surface",
                short_side_texture = null,
                underside_texture = null
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
                end_angle = 4.0f,
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
                start_angle = 2.5f,
                end_angle = 3.5f,
                long_side_texture = "platform_side",
                top_texture = "platform_surface",
                short_side_texture = null,
                underside_texture = null
            });

            var platform3 = new Platform();
            platform3.Init(graphics, content, new PlatformDefinition()
            {
                cX = 0.0f,
                cY = 0.0f,
                topZ = 3.0f,
                height = 0.1f,
                min_radius = 2.0f,
                max_radius = 3.5f,
                start_angle = 0.0f,
                end_angle = 2.0f,
                long_side_texture = "platform_side",
                top_texture = "platform_surface",
                short_side_texture = null,
                underside_texture = null
            });

            var all_items = new List<IGameItem>()
            {
                platform0,
                platform1,
                platform2,
                platform3,
                bauble
            };

            int collectibles = 5;
            var dx = 1.0f / collectibles;
            for (int i = 0; i < collectibles; i++)
            {
                var x = platform2.init.start_angle + (dx * i) + (dx / 2);

                var collectible = new BaubleCollectible(GameState);
                collectible.Init(graphics, content, new BaubleInitParams()
                {
                    radius = 0.1f,
                    texture = "shiny",
                    start = new Vector3((float)x, -2.75f, 4.5f)
                });

                all_items.Add(collectible);
            }

            Focus = bauble;
            player = bauble;

            return all_items;
        }

    }
}

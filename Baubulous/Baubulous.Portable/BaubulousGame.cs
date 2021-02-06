using Baubulous.Portable.GameLogic;
using Baubulous.Portable.GameObjects;
using Baubulous.Portable.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Baubulous.Portable
{
    public class BaubulousGame : Game
    {
        GraphicsDeviceManager graphics;
        BasicEffect effect;

        /*
        Waves floor;
        Tower level;
        Platform platform;
        Bauble bauble;
        */

        int level_index = 0;
        int last_level = 3;
        TowerMapLevel level;

        double fps_previous = 0.0D;

        public BaubulousGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize(); // ensures LoadContent is done before utilising textures
            /*
            floor = new Waves();
            level = new Tower();
            platform = new Platform();
            bauble = new Bauble();

            floor.Init(graphics, Content, new WavesDefinition());
            level.Init(graphics, Content, new TowerDefinition() { cX = 0.0f, cY = 0.0f, baseZ = -0.2f, height = 8.0f, radius = 2.0f } );
            platform.Init(graphics, Content, new PlatformDefinition()
            {
                cX = 0.0f,
                cY = 0.0f,
                topZ = 1.0f,
                height = 0.1f,
                min_radius = 2.0f,
                max_radius = 3.5f,
                start_angle = 0.0f,
                end_angle = (float)Math.PI * 2.0f,
                long_side_texture = "platform_side",
                top_texture = "platform_surface",
                short_side_texture = null,
                underside_texture = null
            });

            bauble.Init(graphics, Content, new BaubleInitParams()
            {
                radius = 0.5f,
                texture = "shiny",
                start = new Vector3(0.0f, -2.75f, 1.5f)
            });
            */

            AdvanceLevel();
            effect = new BasicEffect(graphics.GraphicsDevice);
        }

        protected void AdvanceLevel()
        {
            level_index += 1;

            if (level_index <= last_level)
                InitLevel();
        }

        protected void InitLevel()
        {
            switch (level_index)
            {
                case 1:
                    level = new Level1();
                    break;
                case 2:
                    level = new Level2();
                    break;
                case 3:
                    level = new Level3();
                    break;
            }

            level.Init(graphics, Content, new TowerMapLevelInitParams()
            {

            });
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (level.Complete)
            {
                AdvanceLevel();
                if (level_index > last_level)
                {
                    Exit();
                    return;
                }
            }

            if (level.Failed)
            {
                // restart level
                InitLevel();
            }

            double fps = 1000.0D / gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Math.Abs(fps - fps_previous) > 0.1D)
            {
                Debug.WriteLine("FPS: " + fps);
                fps_previous = fps;
            }

            // standard exit method
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) { Exit(); }

            /*
            floor.Update(gameTime);
            level.Update(gameTime);
            bauble.Update(gameTime);
            platform.Update(gameTime);
            */

            level.Update(gameTime);

            // bounding box collision detection for platforms vs. player
            // bounding sphere collision detection for objects vs. player

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);

            ResetProjection();

            // skybox?

            // game objects
            /*
            floor.Draw(effect);
            level.Draw(effect);
            bauble.Draw(effect);
            platform.Draw(effect);
            */

            level.Draw(effect);
        }

        private void ResetProjection()
        {
            var cameraUpVector = Vector3.UnitZ;
            var cameraPosition = level.CameraPosition;
            var cameraLookAtVector = level.CameraLookAt;
            //var cameraLookAtVector = Vector3.Zero;

            effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);

            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float fieldOfView = MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 3000;

            effect.Projection =
                Matrix.CreatePerspectiveFieldOfView(
                    fieldOfView,
                    aspectRatio,
                    nearClipPlane,
                    farClipPlane);

            //GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rs;

            //effect.EnableDefaultLighting();

            //effect.VertexColorEnabled = true;
            //effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);

            effect.LightingEnabled = true; // turn on the lighting subsystem
            effect.PreferPerPixelLighting = true;

            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(0.75f, 0.75f, 0.75f);
            effect.DirectionalLight0.Direction = new Vector3(0.2f, 0.6f, -0.6f);

            //effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights
            effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f); // ambient 5

            /*
             * all you have to do is set up a VertexBuffer and an IndexBuffer, then set the Indicies and Vertices
             * on your GraphicsDevice to each of them. After that, you use the GraphicsDevice's DrawIndexedPrimitives
             * to draw the data you've buffered. Start an effect before you do so just like with a model and voila.
             */

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Baubulous.Portable.GameObjects;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;

namespace Baubulous.Portable.GameLogic
{
    public abstract class TowerMapLevel : BaseLifecycleObject<TowerMapLevelInitParams>
    {
        public bool Complete { get; set; }

        public bool Failed { get; set; }

        protected float gravity = 2.0f;

        protected Waves floor;
        protected Tower tower;

        protected IGameItem player;

        protected IList<IGameItem> moveables;
        protected IList<IGameItem> collidables;
        protected IList<IGameItem> collision_checks;
        protected IList<BaubleCollectible> collectables;

        protected TowerMapLevel()
        {
            GameState = new GameState();
            Complete = false;
        }

        protected override void DoInit(TowerMapLevelInitParams init)
        {
            TouchPanel.EnabledGestures =
                //GestureType.Hold |
                //GestureType.Tap |
                //GestureType.DoubleTap |
                //GestureType.FreeDrag |
                //GestureType.Flick
                //GestureType.Pinch;
                //GestureType.DragComplete |
                GestureType.VerticalDrag
                //GestureType.FreeDrag |
                //GestureType.Hold
                ;
        }

        protected override void DoLoadResources(ContentManager content)
        {
        }

        protected abstract string TowerTexture { get; }

        protected override IList<IDrawable> GenerateInitChildren()
        {
            floor = new Waves();
            tower = new Tower();

            float worldRadius = 2.75f;

            floor.Init(graphics, content, new WavesDefinition());
            tower.Init(graphics, content, new TowerDefinition() { cX = 0.0f, cY = 0.0f, baseZ = -0.2f, height = 8.0f, radius = 2.0f, textureName = TowerTexture });

            var draws = new List<IDrawable>()
            {
                floor,
                tower
            };

            var levelItems = CreateLevelItems();

            draws.AddRange(levelItems);

            foreach (var item in levelItems)
            {
                item.InitInteraction(worldRadius);
            }

            moveables = levelItems.Where(i => i.Interaction.Moves).ToList();
            collidables = levelItems.Where(i => i.Interaction.Collidable).ToList();
            collision_checks = levelItems.Where(i => i.Interaction.CheckCollisions).ToList();
            collectables = levelItems.Where(i => i is BaubleCollectible).Cast<BaubleCollectible>().ToList();

            return draws;
        }


        protected void CreateInitialCameraPosition()
        {
            CameraX = Focus.Interaction.cX;
            CameraY = Focus.Interaction.cY;
            CameraDistance = 10f;
        }

        protected float CameraX, CameraY, CameraDistance;
        protected bool CameraReady = false;

        public Vector3 CameraPosition
        {
            get { return Interaction.CalcWorldMatrix(CameraDistance, CameraX, CameraY).Translation; }
        }

        public IGameItem Focus { get; protected set; }

        public Vector3 CameraLookAt
        {
            get
            {
                return Focus.GetWorldMatrixPosition();
            }
        }

        protected void UpdateCameraPosition(GameTime time)
        {
            if (!CameraReady)
            {
                CreateInitialCameraPosition();
                CameraReady = true;
                return;
            }

            CameraX = Focus.Interaction.cX;
            CameraY = Focus.Interaction.cY;
        }

        protected abstract IList<IGameItem> CreateLevelItems();

        protected override void DoDraw(BasicEffect effect)
        {
            // NOP - drawing is done in children
        }

        protected override void DoUpdate(GameTime time)
        {
            if (collectables.All(c => c.Counted))
            {
                Complete = true;
            }
            if (player.Interaction.cY < 0.0f)
            {
                Failed = true;
            }

            RetrieveGestures();
            UpdateCameraPosition(time);
            DoGravityAndMotions(time);
            DoCollisions();
        }

        protected void RetrieveGestures()
        {
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.VerticalDrag)
                {
                    if (Math.Abs(gesture.Delta.Y) > GameState.SwipeTolerance)
                    {
                        GameState.SwipeUp += gesture.Delta.Y;

                        if (GameState.SwipeUp < 0.0f && Math.Abs(GameState.SwipeUp) > GameState.MaxSwipeUp)
                            GameState.SwipeUp = -GameState.MaxSwipeUp;

                    }
                }
            }
        }

        public GameState GameState { get; protected set; }

        protected void DoGravityAndMotions(GameTime time)
        {
            List<IGameItem> moversToRemove = new List<IGameItem>();

            foreach (var mover in moveables)
            {
                // gravity
                if (mover.Interaction.Gravity)
                {
                    mover.Interaction.dY -= (time.ElapsedGameTime.Milliseconds / 1000f) * gravity; // gravity
                }

                // motion
                var x = mover.Interaction.cX + mover.Interaction.dX * (time.ElapsedGameTime.Milliseconds / 1000f);
                var y = mover.Interaction.cY + mover.Interaction.dY * (time.ElapsedGameTime.Milliseconds / 1000f);
                mover.Interaction.BoundingBox.CentreAround(new Vector2(x, y));

                if (mover is BaubleCollectible)
                {
                    if (mover.Interaction.cY > tower.init.baseZ + tower.init.height)
                    {
                        // count baubles that hit the top
                        (mover as BaubleCollectible).Counted = true;
                        moversToRemove.Add(mover);
                    }
                }
            }

            if (moversToRemove.Count > 0)
            {
                foreach (var gone in moversToRemove)
                {
                    moveables.Remove(gone);
                }
                moversToRemove.Clear();
            }

        }

        protected void DoCollisions()
        {
            foreach (var incoming in collision_checks)
            {
                foreach (var target in collidables)
                {
                    if (incoming == target) { continue; }

                    // TOOD: compare Interaction.BoundingBox/es and then manage interaction
                    if (incoming.Interaction.BoundingBox.Intersects(target.Interaction.BoundingBox, true))
                    {
                        incoming.OnCollideWith(target, true);
                        target.OnCollideWith(incoming, false);
                    }
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Pages;

namespace Planes
{
    class PlanesNavigationController : NavigationController
    {
        public PlanesNavigationController(GraphicsDeviceManager graphics)
            : base(graphics)
        {
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
        }

        public void Initialize()
        {
            base.Initialize(new GameView());
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager content)
        {
            base.LoadContent(spriteBatch, content);

            Load<SpriteFont>("InGameFont");
            Load<Texture2D>("World");
            Load<Texture2D>("POI");
        }
    }
}

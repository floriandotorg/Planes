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
    class GameView : View
    {
        private GeoCoordinate coord;
        private ProjectionMap map;

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();

            map = new ProjectionMap();
            map.PixelSize = new Point(800, 480);
            map.setTileMillBounds(-138.6518, -56.8490, 154.5513, 72.2355);

            // Berlin
            coord = new GeoCoordinate("52 31 7 N, 13 24 7 O");
        }

        public override void TouchDown(TouchLocation location)
        {
            base.TouchDown(location);

            GeoCoordinate c = new GeoCoordinate(location.Position, map);

            System.Diagnostics.Debug.WriteLine(c.Distance(coord));
        }

        public static Point Vector2ToPoint(Vector2 vector)
        {
            return new Point(Convert.ToInt32(vector.X), Convert.ToInt32(vector.Y));
        }

        public static Vector2 PointToVector2(Point point)
        {
            return new Vector2(Convert.ToSingle(point.X), Convert.ToSingle(point.Y));
        }

        public override void Draw(GameTime gameTime, AnimationInfo animationInfo)
        {
            base.Draw(gameTime, animationInfo);

            SpriteBatch.Draw(Load<Texture2D>("World"), RectangleToSystem(Viewport.Bounds), Color.White);
            
            Point p = Vector2ToPoint(coord.PixelPoint(map));

            SpriteBatch.Draw(Load<Texture2D>("Rectangle"), new Rectangle(p.X, p.Y, 5, 5), Color.Black);
        }
    }
}

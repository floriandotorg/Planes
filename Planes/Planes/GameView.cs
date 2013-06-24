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
        private Random _random;
        private ProjectionMap _projectionMap;
        private PointOfInterest _currentPOI;
        private Vector2 _touchLocation;

        private List<PointOfInterest> _poiList;

        private Header _header;

        public override void Initialize()
        {
            base.Initialize();

            _random = new Random(DateTime.Now.Millisecond);
            _projectionMap = new ProjectionMap();
            _poiList = new List<PointOfInterest>();

            _header = new Header();
            AddSubview(_header);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _projectionMap.PixelSize = new Point(800, 480);
            _projectionMap.setTileMillBounds(-138.6518, -56.8490, 154.5513, 72.2355);

            _poiList.Add(new PointOfInterest() { Name = "Berlin", Coordinate = new GeoCoordinate("52 31 7 N, 13 24 7 O") });
            _poiList.Add(new PointOfInterest() { Name = "Washington, D.C.", Coordinate = new GeoCoordinate("38 53 42 N, 77 2 12 W") });
            _poiList.Add(new PointOfInterest() { Name = "Paris", Coordinate = new GeoCoordinate("48 51 24 N, 2 21 6 E") });
            _poiList.Add(new PointOfInterest() { Name = "Brasília", Coordinate = new GeoCoordinate("15 47 56 S, 47 52 0 W") });
            _poiList.Add(new PointOfInterest() { Name = "Shanghai", Coordinate = new GeoCoordinate("31 12 0 N, 121 30 0 E") });

            pickNewPOI();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _header.Height = 50;
            _header.Width = 200;
            CenterSubview(_header, 0);
            _header.Y = 0;
        }

        private float _timer;

        private void pickNewPOI()
        {
            _touchLocation = new Vector2();
            _currentPOI = _poiList[_random.Next(_poiList.Count)];
            _header.CurrentPOI = _currentPOI.Name;

            restartTimer();
        }

        private void restartTimer()
        {
            _timer = 1.01f;
            updateTimer();
        }

        private void updateTimer()
        {
            _timer -= .01f;
            _header.Progress = _timer;

            if (_timer < 0)
            {
                Overlay(new ScoreOverlay(), true);
            }
            else if (_touchLocation == new Vector2())
            {
                PerformActionAfterDelay(updateTimer, TimeSpan.FromMilliseconds(20));
            }
        }

        public override bool TouchDown(TouchLocation location)
        {
            if (!base.TouchDown(location))
            {
                if (_touchLocation == new Vector2())
                {
                    _touchLocation = location.Position;
                    Overlay(new ScoreOverlay(String.Format("{0:0,0} Km", Convert.ToInt32(_currentPOI.Coordinate.Distance(new GeoCoordinate(location.Position, _projectionMap))))), true);
                }
            }

            return true;
        }

        public override void OverlayDimissed(View overlay)
        {
            base.OverlayDimissed(overlay);

            pickNewPOI();
        }

        public override void Draw(GameTime gameTime, AnimationInfo animationInfo)
        {
            SpriteBatch.Draw(Load<Texture2D>("World"), RectangleToSystem(Viewport.Bounds), Color.White);

            if (_touchLocation != new Vector2())
            {
                Vector2 touch = _touchLocation;
                Vector2 poi = _currentPOI.Coordinate.PixelPoint(_projectionMap);

                DrawLine(SpriteBatch, 1f, Color.Blue, touch, poi);

                SpriteBatch.Draw(Load<Texture2D>("POI"), new Rectangle(Convert.ToInt32(touch.X - 5f), Convert.ToInt32(touch.Y - 5f), 10, 10), Color.White);
                SpriteBatch.Draw(Load<Texture2D>("POI"), new Rectangle(Convert.ToInt32(poi.X - 5f), Convert.ToInt32(poi.Y - 5f), 10, 10), Color.White);
            }

            base.Draw(gameTime, animationInfo);
        }
    }
}

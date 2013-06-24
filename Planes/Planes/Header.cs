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
    class Header : View
    {
        private Label _currentPOI;
        private ProgressBar _progressBar;

        public float Progress
        {
            set
            {
                _progressBar.Progress = value;
            }
        }

        public string CurrentPOI
        {
            set
            {
                _currentPOI.Text = value;
                NeedsRelayout = true;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            _currentPOI = new Label();
            AddSubview(_currentPOI);

            _progressBar = new ProgressBar();
            AddSubview(_progressBar);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            BackgroundColor = Color.Gray * .7f;

            _currentPOI.Color = Color.Black;
            _currentPOI.Font = Load<SpriteFont>("InGameFont");

            _progressBar.BackgroundColor = Color.Blue * .8f;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            CenterSubview(_currentPOI, 0);
            _currentPOI.Y = 5;

            _progressBar.Width = Width - 10;
            _progressBar.Height = 15;
            _progressBar.X = 5;
            _progressBar.Y = Height - _progressBar.Height - 5;
        }
    }
}

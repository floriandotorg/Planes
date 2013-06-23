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
    class ScoreOverlay : View
    {
        private Label _headerLabel;
        private Label _resultLabel;
        private Button _nextButton;
        private string _resultText;

        public ScoreOverlay(string resultText)
        {
            _resultText = resultText;
        }

        public override void Initialize()
        {
            base.Initialize();

            _headerLabel = new Label();
            AddSubview(_headerLabel);

            _resultLabel = new Label();
            AddSubview(_resultLabel);

            _nextButton = new Button();
            AddSubview(_nextButton);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Width = 300;
            Height = 200;
            Superview.CenterSubview(this, 0);
            BackgroundColor = Color.Gray * .7f;

            _headerLabel.Text = "Close, but no cigar!";
            _headerLabel.Font = Load<SpriteFont>("InGameFont");
            CenterSubview(_headerLabel, 0);
            _headerLabel.Y = 15;

            _resultLabel.Text = _resultText;
            _resultLabel.Font = Load<SpriteFont>("InGameFont");
            CenterSubview(_resultLabel, 0);

            _nextButton.Text = "Next";
            _nextButton.Font = Load<SpriteFont>("InGameFont");
            _nextButton.Color = new Color(120, 120, 120);
            CenterSubview(_nextButton, 0);
            _nextButton.Y = Height - 15 - _nextButton.Height;
            _nextButton.Tap += _nextButton_Tap;
        }

        void _nextButton_Tap(object sender)
        {
            Dismiss(true);
        }

        public override void Draw(GameTime gameTime, AnimationInfo animationInfo)
        {
            base.Draw(gameTime, animationInfo);
        }
    }
}

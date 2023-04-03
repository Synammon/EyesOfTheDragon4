using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using SharedProject;
using SharedProject.Controls;
using SharedProject.GamesScreens;
using Microsoft.Xna.Framework.Content;
using SharedProject.Sprites;

namespace SummonersTale.StateManagement
{
    public interface INewGameState
    {
        GameState GameState { get; }
    }

    public class NewGameState : GameState, INewGameState
    {
        private RightLeftSelector _portraitSelector;
        private RightLeftSelector _genderSelector;
        private TextBox _nameTextBox;
        private readonly Dictionary<string, Texture2D> _femalePortraits;
        private readonly Dictionary<string, Texture2D> _malePortraits;
        private Button _create;
        private Button _back;
        private RenderTarget2D renderTarget2D;

        private int _points = 22;

        private Label _pointsLabel;
        private Label _remainingLabel;

        private Label _strengthLabel;
        private RightLeftSelector _strengthSelector;

        private Label _perceptionLabel;
        private RightLeftSelector _perceptionSelector;

        private Label _enduranceLabel;
        private RightLeftSelector _enduranceSelector;

        private Label _charismaLabel;
        private RightLeftSelector _charismaSelector;

        private Label _intellectLabel;
        private RightLeftSelector _intellectSelector;

        private Label _agilityLabel;
        private RightLeftSelector _agilitySelector;

        private Label _luckLabel;
        private RightLeftSelector _luckSelector;

        public GameState GameState => this;

        public NewGameState(Game game) : base(game)
        {
            Game.Services.AddService<INewGameState>(this);
            _femalePortraits = new();
            _malePortraits = new();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = Game.Services.GetService<SpriteBatch>();
            renderTarget2D = new(GraphicsDevice, Settings.TargetWidth, Settings.TargetHeight);

            ContentManager content = Game.Content;

            string[] items = new[] { "2", "3", "4", "5", "6", "7", "8", "9" };

            controls = new(content.Load<SpriteFont>(@"Fonts/MainFont"), 100);
            _genderSelector = new(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new Vector2(207 - 70, 298)
            };

            _genderSelector.SelectionChanged += GenderSelector_SelectionChanged;
            _genderSelector.SetItems(new[] { "Female", "Male" }, 300);

            _portraitSelector = new(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new Vector2(207 - 70, 458)
            };

            _portraitSelector.SelectionChanged += PortraitSelector_SelectionChanged;
            _portraitSelector.SetItems(new[] { "Fighter", "Wizard", "Rogue", "Priest" }, 300);
            
            _pointsLabel = new()
            {
                Text = "Points to spend: ",
                Position = new(700, 20),
                Color = Color.White,
            };

            _remainingLabel = new()
            {
                Text = "22",
                Position = new(900, 20),
                Color = Color.Red,
            };

            _strengthLabel = new Label()
            { 
                Color = Color.White,
                Text = "Strength",
                Position = new(700, 75)
            };

            _strengthSelector = new RightLeftSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new(900, 75)
            };

            _strengthSelector.SetItems(items, 75);
            _strengthSelector.SelectionChanged += Ability_SelectorChanged;

            _perceptionLabel = new Label()
            {
                Color = Color.White,
                Text = "Perception",
                Position = new(700, 150)
            };

            _perceptionSelector = new RightLeftSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new(900, 150)
            };

            _perceptionSelector.SetItems(items, 75);
            _perceptionSelector.SelectionChanged += Ability_SelectorChanged;

            _enduranceLabel = new Label()
            {
                Color = Color.White,
                Text = "Endurance",
                Position = new(700, 225)
            };

            _enduranceSelector = new RightLeftSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new(900, 225)
            };

            _enduranceSelector.SetItems(items, 75);
            _enduranceSelector.SelectionChanged += Ability_SelectorChanged;

            _charismaLabel = new Label()
            {
                Color = Color.White,
                Text = "Charisma",
                Position = new(700, 300)
            };

            _charismaSelector = new RightLeftSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new(900, 300)
            };

            _charismaSelector.SetItems(items, 75);
            _charismaSelector.SelectionChanged += Ability_SelectorChanged;

            _intellectLabel = new Label()
            {
                Color = Color.White,
                Text = "Intellect",
                Position = new(700, 375)
            };

            _intellectSelector = new RightLeftSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new(900, 375)
            };

            _intellectSelector.SetItems(items, 75);
            _intellectSelector.SelectionChanged += Ability_SelectorChanged;

            _agilityLabel = new Label()
            {
                Color = Color.White,
                Text = "Agility",
                Position = new(700, 450)
            };

            _agilitySelector = new RightLeftSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new(900, 450)
            };

            _agilitySelector.SetItems(items, 75);
            _agilitySelector.SelectionChanged += Ability_SelectorChanged;

            _luckLabel = new Label()
            {
                Color = Color.White,
                Text = "Luck",
                Position = new(700, 525)
            };

            _luckSelector = new RightLeftSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new(900, 525)
            };

            _luckSelector.SetItems(items, 75);
            _luckSelector.SelectionChanged += Ability_SelectorChanged;

            Texture2D background = new(GraphicsDevice, 100, 25);
            Texture2D caret = new(GraphicsDevice, 2, 25);
            Texture2D border = new(GraphicsDevice, 100, 25);

            caret.Fill(Color.Black);
            border.Fill(Color.Black);
            background.Fill(Color.White);

            _nameTextBox = new(background, caret, border)
            {
                Position = new(207, 138),
                HasFocus = true,
                Enabled = true,
                Color = Color.Black,
                Text = "Bethany",
                Size = new(100, 25)
            };

            _femalePortraits.Add(
            "Fighter",
                content.Load<Texture2D>(@"PlayerSprites/femalefighter"));
            _femalePortraits.Add(
            "Priest",
                content.Load<Texture2D>(@"PlayerSprites/femalepriest"));
            _femalePortraits.Add(
            "Rogue",
                content.Load<Texture2D>(@"PlayerSprites/femalerogue"));
            _femalePortraits.Add(
            "Wizard",
                content.Load<Texture2D>(@"PlayerSprites/femalewizard"));

            _malePortraits.Add(
            "Fighter",
                content.Load<Texture2D>(@"PlayerSprites/malefighter"));
            _malePortraits.Add(
            "Priest",
                content.Load<Texture2D>(@"PlayerSprites/malepriest"));
            _malePortraits.Add(
            "Rogue",
                content.Load<Texture2D>(@"PlayerSprites/malerogue"));
            _malePortraits.Add(
            "Wizard",
                content.Load<Texture2D>(@"PlayerSprites/malewizard"));


            _portraitSelector.SetItems(_femalePortraits.Keys.ToArray(), 300);
            _create = new(
                content.Load<Texture2D>(@"GUI\g9202"),
                ButtonRole.Accept)
            {
                Text = "Create",
                Position = new(180, 640)
            };

            _create.Click += Create_Click;
            _back = new(
                content.Load<Texture2D>(@"GUI\g9202"),
                ButtonRole.Cancel)
            {
                Text = "Back",
                Position = new(350, 640),
                Color = Color.White
            };

            _back.Click += Back_Click;

            ControlManager.Add(_pointsLabel);
            ControlManager.Add(_remainingLabel);
            ControlManager.Add(_nameTextBox);
            ControlManager.Add(_genderSelector);
            ControlManager.Add(_portraitSelector);
            ControlManager.Add(_create);
            ControlManager.Add(_back);
            ControlManager.Add(_strengthLabel);
            ControlManager.Add(_strengthSelector);
            ControlManager.Add(_perceptionLabel);
            ControlManager.Add(_perceptionSelector);
            ControlManager.Add(_enduranceLabel);
            ControlManager.Add(_enduranceSelector);
            ControlManager.Add(_charismaLabel);
            ControlManager.Add(_charismaSelector);
            ControlManager.Add(_intellectLabel);
            ControlManager.Add(_intellectSelector);
            ControlManager.Add(_agilityLabel);
            ControlManager.Add(_agilitySelector);
            ControlManager.Add(_luckLabel);
            ControlManager.Add(_luckSelector);
        }

        private void Ability_SelectorChanged(object sender, DirectionEventArgs e)
        {
            if (e.Direction == Direction.Up)
            {
                if (_points > 0)
                {
                    _points--;
                }
                else
                {
                    ((RightLeftSelector)sender).SelectedIndex--;
                }
            }
            else
            {
                if (_points < 22)
                {
                    _points++;
                }
                else
                {
                    ((RightLeftSelector)sender).SelectedIndex++;
                }
            }

            _remainingLabel.Text = _points.ToString();
        }

        private void Back_Click(object sender, EventArgs e)
        {
        }

        private void Create_Click(object sender, EventArgs e)
        {
            if (_points > 0) return;

            Dictionary<string, Animation> animations = new();

            Animation animation = new(3, 32, 32, 0, 0) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkdown", animation);

            animation = new(3, 32, 32, 0, 32) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkleft", animation);

            animation = new(3, 32, 32, 0, 64) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkright", animation);

            animation = new(3, 32, 32, 0, 96) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkup", animation);

            Player player = new(
                Game,
                new(_genderSelector.SelectedIndex == 0 ?
                    _femalePortraits[_portraitSelector.SelectedItem] :
                    _malePortraits[_portraitSelector.SelectedItem],
                animations))
            {
                Strength = 2 + _strengthSelector.SelectedIndex,
                Perception = 2 + _perceptionSelector.SelectedIndex,
                Endurance = 2 + _enduranceSelector.SelectedIndex,
                Charisma = 2 + _charismaSelector.SelectedIndex,
                Intellect = 2 + _intellectSelector.SelectedIndex,
                Agility = 2 + _agilitySelector.SelectedIndex,
                Luck = 2 + _luckSelector.SelectedIndex
            };

            player.Health = new(player.Strength * player.Endurance);
            player.Mana = new(player.Intellect * player.Endurance);

            player.Sprite.CurrentAnimation = "walkdown";

            IGamePlayState gamePlayState = Game.Services.GetService<IGamePlayState>();

            gamePlayState.Player = player;

            StateManager.PopState();
            StateManager.PushState(gamePlayState.Tag);

            gamePlayState.Tag.Enabled = true;
        }

        private void GenderSelector_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void PortraitSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (_genderSelector.SelectedIndex == 0)
            {
            }
            else
            {
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget2D);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            ControlManager.Draw(SpriteBatch);

            base.Draw(gameTime);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();

            SpriteBatch.Draw(
                renderTarget2D,
                new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y),
                Color.White);

            SpriteBatch.End();

        }
    }
}

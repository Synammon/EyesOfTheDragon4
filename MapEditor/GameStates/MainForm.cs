﻿using MapEditor.Forms;
using Microsoft.Xna.Framework;
using SharedProject;
using SharedProject.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.GameStates
{
    public interface IMainForm
    {
        Form Target { get; }
    }

    public class MainForm : Form, IMainForm
    {
        private FileForm fileForm;

        public Form Target => this;

        public MainForm(Game game, Vector2 position, Point size)
            : base(game, position, size)
        {
            Game.Services.AddService<IMainForm>(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Point size = new(700, 400);

            fileForm = new(Game, new(0, 0), size)
            {
                Visible = false,
                Enabled = false
            };

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            FullScreen = true;

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                fileForm.Visible = true;
                fileForm.Enabled = true;
                fileForm.Role = FileFormRole.Save;
                StateManager.PushState(fileForm);
                this.Visible = true;
            }

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                fileForm.Visible = true;
                fileForm.Enabled = true;
                fileForm.Role = FileFormRole.Open;
                StateManager.PushState(fileForm);
                this.Visible = true;
            }

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.P))
            {
                MessageForm frm = new(Game, new(100, 100), new(200, 100), "My message!", true)
                {
                    Visible = true,
                    Color = Color.Black,
                    Message = "My message!"
                };

                frm.Message = "My message!";
                StateManager.PushState(frm);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void Show()
        {
            base.Show();

            if (fileForm != null)
            {
                if (fileForm.Visible)
                {
                    if (fileForm.Role == FileFormRole.Open)
                    {

                    }
                    else if (fileForm.Role == FileFormRole.Create)
                    {

                    }
                    else if (fileForm.Role == FileFormRole.Save)
                    {

                    }
                    else if (fileForm.Role == FileFormRole.Directory)
                    {

                    }
                }
            }
        }
    }
}

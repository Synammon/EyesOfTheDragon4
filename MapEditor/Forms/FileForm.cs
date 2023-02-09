using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedProject;
using SharedProject.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Forms
{
    public enum FileFormRole { Open, Save, Create, Directory }

    public class FileForm : Form
    {
        TextBox selected;
        Button action;
        ListBox items;
        string dir = Environment.CurrentDirectory;

        public string FileName { get; set; }
        public FileFormRole Role { get; set; } = FileFormRole.Open;

        public FileForm(Game game, Vector2 position, Point size) : base(game, position, size)
        {
            Position = new(0, 0);

            Size = new(_graphicsDevice.PreferredBackBufferWidth, _graphicsDevice.PreferredBackBufferHeight);
            Bounds = new(0, 0, _graphicsDevice.PreferredBackBufferWidth, _graphicsDevice.PreferredBackBufferHeight);
            FullScreen = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Size = new(_graphicsDevice.PreferredBackBufferWidth, _graphicsDevice.PreferredBackBufferHeight);
            Bounds = new(0, 0, _graphicsDevice.PreferredBackBufferWidth, _graphicsDevice.PreferredBackBufferHeight);

            Background.Image = new(GraphicsDevice, Size.X, Size.Y);
            Background.Image.Fill(Color.White);

            Texture2D caret = new(GraphicsDevice, 2, 16);
            caret.Fill(Color.Black);

            Texture2D bg = new(GraphicsDevice, 4, 4);
            bg.Fill(Color.White);

            Texture2D brdr = new(GraphicsDevice, 4, 4);

            brdr.Fill(Color.Black);

            selected = new(bg, caret, brdr)
            {
                Position = new(5, 25),
                Size = new(Size.X - 80, 25),
                Color = Color.White,
                Text = "",
                Visible = true,
                Enabled = true,
                TabStop = true,
                HasFocus = true,
            };

            Controls.Add(selected);

            action = new(Game.Content.Load<Texture2D>(@"GUI/Button"), ButtonRole.Accept)
            {
                Position = new(Size.X - 55, 25),
                Color = Color.Black,
                Text = "Select",
                Visible = true,
                Enabled = true,
                TabStop = true,
                Size = new(50, 25),
                Role = ButtonRole.Menu,
            };

            action.Click += Action_Click;

            Controls.Add(action);

            caret = new(GraphicsDevice, Size.X - 40, 16);
            caret.Fill(Color.Blue);

            items = new(bg,
                        Game.Content.Load<Texture2D>(@"GUI/DownButton"),
                        Game.Content.Load<Texture2D>(@"GUI/UpButton"),
                        caret,
                        brdr)
            {
                Position = new(0, 60),
                Size = new(Size.X, Size.Y - 60),
                Color = Color.Black,
                TabStop = true,
                Enabled = true,
                Visible = true,
            };

            items.Selected += Items_Selected;
            Controls.Add(items);

            FillItems();
        }

        private void Action_Click(object sender, EventArgs e)
        {
            string path = string.Format("{0}{1}", dir, selected.Text);

            if (Role == FileFormRole.Open)
            {
                if (!File.Exists(path))
                {
                    // uh-oh!
                }
                else
                {
                    FileName = path;
                    StateManager.PopState();
                }
            }
            else if (Role == FileFormRole.Create)
            {
                if (File.Exists(path))
                {
                    // uh-oh!
                }
                else
                {
                    FileName = path;
                    StateManager.PopState();
                }
            }
            else if (Role == FileFormRole.Save)
            {
                if (File.Exists(path))
                {
                    // uh-oh. Overwrite?
                }
                else
                {
                    FileName = path;
                    StateManager.PopState();
                }
            }
        }

        private void Selected_Selected(object sender, EventArgs e)
        {
        }

        private void FillItems()
        {
            if (!dir.EndsWith("\\")) dir += "\\";

            string[] folders = Directory.GetDirectories(dir);
            string[] files = Directory.GetFiles(dir);

            items.Items.Clear();
            items.Items.Add("..");
            items.SelectedIndex = 0;

            foreach (var v in folders)
            {
                items.Items.Add(new DirectoryInfo(v).Name);
            }

            foreach (var v in files)
            {
                string path = Path.GetFileName(v);
                items.Items.Add(path);
            }
        }

        private void Items_Selected(object sender, SelectedIndexEventArgs e)
        {
            if (e.Index > 0)
            {
                if (File.Exists(dir + items.Items[e.Index]))
                {
                    FileName = items.Items[e.Index];
                    selected.Text = FileName;
                }
                else
                {
                    dir += items.Items[e.Index];
                    FillItems();
                }
            }
            else if (e.Index == 0)
            {
                dir = Directory.GetParent(Directory.GetParent(dir).FullName).FullName;
                FillItems();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Control c in Controls)
            {
                c.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void Show()
        {
            if (items != null)
            {
                FillItems();
                Title = Role.ToString();
            }

            base.Show();
        }

        protected override void Hide()
        {
            base.Hide();
        }
    }
}

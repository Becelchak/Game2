using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace Game
{
    public sealed partial class Missions : Form
    {
        public Missions()
        {
            InitializeComponent();
            Directory.SetCurrentDirectory("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources");
            var level1 = new LinkLabel()
            {
                Text = "Миссия 1",
                Font = new Font("Stencil", 22F, FontStyle.Bold, GraphicsUnit.Point),
                LinkColor = Color.Firebrick,
                BackColor = Color.Transparent,
                Size = new Size(150, 35),
                Location = new Point(120, 150)
            };
            var level2 = new LinkLabel()
            {
                Text = "Миссия 2",
                Font = new Font("Stencil", 22F, FontStyle.Bold, GraphicsUnit.Point),
                LinkColor = Color.Firebrick,
                BackColor = Color.Transparent,
                Size = new Size(150, 35),
                Location = new Point(120, 200),
            };
            var level3 = new LinkLabel()
            {
                Text = "Миссия 3",
                Font = new Font("Stencil", 22F, FontStyle.Bold, GraphicsUnit.Point),
                LinkColor = Color.Firebrick,
                BackColor = Color.Transparent,
                Size = new Size(150, 35),
                Location = new Point(120, 250),
            };

            var crush = new PictureBox()
            {
                Image = Resource.Crush,
                Size = new Size(200,150),
                Location = new Point(100,140)
            };

            var terminal = new PictureBox()
            {
                Image = Resource.Terminal,
                Size = new Size(238,188),
                Location = new Point(82,122)
            };

            Controls.Add(level1);
            Controls.Add(level2);
            Controls.Add(level3);
            Controls.Add(crush);
            Controls.Add(terminal);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackgroundImage = Resource.logoSPC;
            ClientSize = new Size(960, 600);

            level1.Click += (sender, args) =>
            {
                GameMap.CreateMap(GameMap.pack1.Dequeue());
                var f = new MissionBase(GameMap.pack1, new Point(50, 50), "Mission1", 
                    Resource.Enemy1, Resource.Dead1, 3, new SoundPlayer(Path.GetFullPath("die1.wav")))
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = Location
                };
                f.Show();
                Hide();
            };

            level2.Click += (sender, args) =>
            {
                GameMap.CreateMap(GameMap.pack2.Dequeue());
                var f = new MissionBase(GameMap.pack2, new Point(50, 50), "Mission2", 
                    Resource.Enemy2, Resource.Dead2, 2, new SoundPlayer(Path.GetFullPath("die2.wav")))
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = Location
                };
                f.Show();
                Hide();
            };
            level3.Click += (sender, args) =>
            {
                GameMap.CreateMap(GameMap.pack3.Dequeue());
                var f = new MissionBase(GameMap.pack3, new Point(50, 50), "Mission3", 
                    Resource.Enemy3, Resource.Dead3, 1, new SoundPlayer(Path.GetFullPath("die3.wav")))
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = Location
                };
                f.Show();
                Hide();
            };
        }
    }
}
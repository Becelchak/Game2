using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public partial class Missions : Form
    {
        public Missions()
        {
            InitializeComponent();
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
                Game_Map.CreateMap(Game_Map.pack1.Dequeue());
                var f = new Mission1(Game_Map.pack1, new Point(50, 50))
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = Location
                };
                f.Show();
                Hide();
            };

            level2.Click += (sender, args) =>
            {
                Game_Map.CreateMap(Game_Map.pack2.Dequeue());
                var f = new Mission1(Game_Map.pack2, new Point(50, 50))
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = Location
                };
                f.Show();
                Hide();
            };
            level3.Click += (sender, args) =>
            {
                Game_Map.CreateMap(Game_Map.pack3.Dequeue());
                var f = new Mission1(Game_Map.pack3, new Point(50, 50))
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
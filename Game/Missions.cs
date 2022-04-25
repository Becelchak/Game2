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
                Image = Image.FromFile("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\logoSPC.jpg"),
                Size = new Size(200,150),
                Location = new Point(100,140)
            };

            var terminal = new PictureBox()
            {
                Image = Image.FromFile("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\logoSPC.jpg"),
                Size = new Size(238,188),
                Location = new Point(82,122)
            };

            Controls.Add(level1);
            Controls.Add(level2);
            Controls.Add(level3);
            Controls.Add(crush);
            Controls.Add(terminal);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackgroundImage = Image.FromFile("C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\logoSPC.jpg");
            ClientSize = new Size(960, 600);
        }
    }
}
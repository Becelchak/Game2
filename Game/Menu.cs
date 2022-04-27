using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();

            var gameName = new Label()
            {
                Text = "Emergency protocol",
                Font = new Font("Stencil", 22F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Brown,
                Size = new Size(390,35),
                TextAlign = ContentAlignment.MiddleCenter,
                BackgroundImage = Resource.logoSPC
            };
            var buttonStart = new Button()
            {
                Text = "Начать игру",
                Font = new Font("Imprint MT Shadow", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204),
                Size = new Size(250,90),
                Margin = new Padding(55,0,0,0)

            };
            var buttonExit = new Button()
            {
                Text = "Выйти",
                Font = new Font("Imprint MT Shadow", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204),
                Size = new Size(250, 90),
                Margin = new Padding(55)
            };
            var table = new TableLayoutPanel();
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 180));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));

            table.Controls.Add(new Panel() { BackgroundImage = Resource.logoSPC },0,0);
            table.Controls.Add(new Panel() { BackgroundImage = Resource.logoSPC }, 1, 0);
            table.Controls.Add(gameName, 2, 1);
            table.Controls.Add(buttonStart,2,2);
            table.Controls.Add(buttonExit, 2, 3);

            table.BackgroundImage = Resource.logoSPC;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            table.Dock = DockStyle.Fill;
            Controls.Add(table);

            buttonExit.Click += (sender, args) =>
            {
                Close();
            };

            buttonStart.Click += (sender, args) =>
            {
                var f = new Missions
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

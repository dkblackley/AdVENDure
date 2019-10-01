using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdVENDture
{
    public partial class MainMenu : Form
    {
        int middleY = 300 - 50;
        int middleX = 200 - 50;
        int buttonX = 100;
        int buttonY = 100;

        public MainMenu()
        {

            InitializeComponent();
            generateMenu();

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        /*
            generate Objects for the menu.    

        */
        public void generateMenu()
        {

            Button start = new Button();
            start.SetBounds(middleX, middleY - buttonY - 20, buttonX, buttonY);

            start.BackgroundImageLayout = ImageLayout.Stretch;
            start.BackColor = Color.LightBlue;
            start.BackgroundImage = new Bitmap(AdVENDture.Properties.Resources.play);


            start.TabStop = false;
            start.FlatStyle = FlatStyle.Flat;
            start.FlatAppearance.BorderSize = 0;
            start.Click += start_Click;


            Button scoreboard = new Button();
            scoreboard.SetBounds(middleX, middleY, buttonX, buttonY);

            scoreboard.BackgroundImageLayout = ImageLayout.Stretch;
            scoreboard.BackColor = Color.LightBlue;
            scoreboard.BackgroundImage = new Bitmap(AdVENDture.Properties.Resources.star);

            scoreboard.TabStop = false;
            scoreboard.FlatStyle = FlatStyle.Flat;
            scoreboard.FlatAppearance.BorderSize = 0;
            scoreboard.Click += scoreboard_Click;


            Button close = new Button();
            close.SetBounds(middleX, middleY + buttonY + 20, buttonX, buttonY);

            close.BackgroundImageLayout = ImageLayout.Stretch;
            close.BackColor = Color.LightBlue;
            close.BackgroundImage = new Bitmap(AdVENDture.Properties.Resources.close);

            close.TabStop = false;
            close.FlatStyle = FlatStyle.Flat;
            close.FlatAppearance.BorderSize = 0;
            close.Click += close_Click;


            PictureBox background = new PictureBox();
            background.SetBounds(0, 0, 600, 600);

            //https://dribbble.com/shots/4411750-Seamless-Sky-Loop-Background
            background.Image = Image.FromFile("../../../Sky.gif");
            background.SizeMode = PictureBoxSizeMode.StretchImage;

            Controls.Add(start);
            Controls.Add(scoreboard);
            Controls.Add(close);
            Controls.Add(background);
        }

        /*
            Event Handeler for start game
                
        */
        private void start_Click(Object sender, EventArgs e)
        {

            GameForm game = new GameForm();

            DialogResult result = game.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.Visible = true;
            }
            else
            {
                Close();
            }
        }

        /*
            Event Handeler for showing high score board
                
        */
        private void scoreboard_Click(Object sender, EventArgs e)
        {

            popUpForm rankings = new popUpForm(-1);
            this.Visible = false;
            rankings.menuMode();

            DialogResult result = rankings.ShowDialog();
            

            if (result == DialogResult.OK)
            {
                this.Visible = true;
            }
            else
            {
                Close();
            }

        }

        /*
            Event Handeler for exit game
                
        */
        private void close_Click(Object sender, EventArgs e)
        {

            Close();

        }

    }

}

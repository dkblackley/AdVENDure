using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdVENDture
{

    public partial class GameForm : Form
    {

        private PictureBox[,] grid = new PictureBox[9, 13];
        private PictureBox player = new PictureBox();
        private Label timerDisplay = new Label();
        private Label scoreDisplay = new Label();
        private MenuStrip gameMenu = new MenuStrip();

        System.IO.Stream str = Properties.Resources.point;
        System.Media.SoundPlayer snd;

        //load images from file
        private Image cocacolaCan = new Bitmap(AdVENDture.Properties.Resources.CocaCola); 
        private Image vendingmachine = new Bitmap(AdVENDture.Properties.Resources.VendingMachine);
        private Image candy = new Bitmap(AdVENDture.Properties.Resources.candy);
        private Image candyBlue = new Bitmap(AdVENDture.Properties.Resources.candyBlue);
        private Image candyGreen = new Bitmap(AdVENDture.Properties.Resources.candyGreen);
        private Image candyOrange = new Bitmap(AdVENDture.Properties.Resources.candyOrange);
        private Image candyPink = new Bitmap(AdVENDture.Properties.Resources.candyPink);

        private Image crisps = new Bitmap(AdVENDture.Properties.Resources.crisps);
        private Image crispsGreen = new Bitmap(AdVENDture.Properties.Resources.crispsGreen);
        private Image urWater = new Bitmap(AdVENDture.Properties.Resources.urWater);

        //set up variables to be used at runtime
        private int scrollSpeed = 20;
        private int bottomRow = 8;
        private double gameTime = 30;
        private int score = 0;
        private bool gameWon = false;

        private System.Timers.Timer _timer;
        private DateTime _startTime;
        //colours used for graphics
        Color enemy = Color.Yellow;
        Color backg = Color.Black;

        //constructor
        public GameForm()
        {

            InitializeComponent();
            setupGame();
            setupMenuStrip();
            StartGame();

        }

        /*
         * The following method is responsible for setting up the play area, this will include the grid of picture boxes,
         * the labels and graphics
         */
        private void setupGame() {

            //set background to be black
            this.BackColor = backg;
            //set up keypress event
            this.KeyPress += new KeyPressEventHandler(Form1_KeyPress);
            snd = new System.Media.SoundPlayer(str);

            //Cycle through each element in the picture box array and sets up its initial position and colour
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {

                    grid[x, y] = new PictureBox();
                    grid[x, y].Size = new Size(40, 40);
                    grid[x, y].Location = new Point((40 * x) + 35, (40 * y));
                    grid[x, y].BorderStyle = BorderStyle.None;
                    grid[x, y].BackColor = backg;
                    grid[x, y].SizeMode = PictureBoxSizeMode.StretchImage;
                    Controls.Add(grid[x, y]);

                }
            }

            //set up background
            this.BackgroundImage = vendingmachine;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //set up player character
            player.Image = cocacolaCan;
            player.SetBounds(200, 150, 40, 60);
            player.BringToFront();
            player.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(player);

            this.BackColor = Color.Black;
            //setting up labels
            timerDisplay.SetBounds(400, 50, 100, 50);
            timerDisplay.ForeColor = Color.Goldenrod;
            timerDisplay.Font = new Font("Verdana", 20, FontStyle.Bold);
            timerDisplay.BackColor = Color.Transparent;
            timerDisplay.AutoSize = false;
            timerDisplay.TextAlign = ContentAlignment.MiddleCenter;

            scoreDisplay.SetBounds(400, 100,100,100);
            scoreDisplay.ForeColor = Color.Goldenrod;
            scoreDisplay.Font = new Font("Verdana", 20, FontStyle.Bold);
            scoreDisplay.BackColor = Color.Transparent;
            scoreDisplay.AutoSize = false;
            scoreDisplay.TextAlign = ContentAlignment.MiddleCenter;

            Controls.Add(timerDisplay);
            Controls.Add(scoreDisplay);
        }

        /*
         * The following method is used to set up the various menu and submenu strips
         */
        private void setupMenuStrip()
        {

            ToolStripMenuItem[] itemsInMenu = new ToolStripMenuItem[2];
            ToolStripMenuItem[] subItemsInMenu = new ToolStripMenuItem[2];
            ToolStripMenuItem[] difficulty = new ToolStripMenuItem[3];
            ToolStripMenuItem[] helpItems = new ToolStripMenuItem[2];


            itemsInMenu[0] = new ToolStripMenuItem();
            itemsInMenu[0].Name = "File";
            itemsInMenu[0].Text = "File";

            itemsInMenu[1] = new ToolStripMenuItem();
            itemsInMenu[1].Name = "Help";
            itemsInMenu[1].Text = "Help";

            subItemsInMenu[0] = new ToolStripMenuItem();
            subItemsInMenu[0].Name = "Difficulty";
            subItemsInMenu[0].Text = "Difficulty";

            subItemsInMenu[1] = new ToolStripMenuItem();
            subItemsInMenu[1].Name = "Close";
            subItemsInMenu[1].Text = "Close";
            subItemsInMenu[1].Click += new EventHandler(helpClickHandler);

            difficulty[0] = new ToolStripMenuItem();
            difficulty[0].Name = "Easy";
            difficulty[0].Text = "Easy";
            difficulty[0].Click += new EventHandler(difficultyClickHandler);

            difficulty[1] = new ToolStripMenuItem();
            difficulty[1].Name = "Default";
            difficulty[1].Text = "Default";
            difficulty[1].Click += new EventHandler(difficultyClickHandler);

            difficulty[2] = new ToolStripMenuItem();
            difficulty[2].Name = "Hard";
            difficulty[2].Text = "Hard";
            difficulty[2].Click += new EventHandler(difficultyClickHandler);

            helpItems[0] = new ToolStripMenuItem();
            helpItems[0].Name = "Credits";
            helpItems[0].Text = "Credits";
            helpItems[0].Click += new EventHandler(helpClickHandler);

            helpItems[1] = new ToolStripMenuItem();
            helpItems[1].Name = "Instructions";
            helpItems[1].Text = "Instructions";
            helpItems[1].Click += new EventHandler(helpClickHandler);

            subItemsInMenu[0].DropDownItems.AddRange(difficulty);
            itemsInMenu[0].DropDownItems.AddRange(subItemsInMenu);
            itemsInMenu[1].DropDownItems.AddRange(helpItems);

            gameMenu.Items.AddRange(itemsInMenu);

            Controls.Add(gameMenu);
        }

        /*
         * The following method handles the event for clicking on the menustrip
         * options that aren't related to difficulty
         */
        private void helpClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem choice = sender as ToolStripMenuItem;

            //close the game if the user chooses to
            if (choice.Name == "Close")
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            //Print out the creators names
            else if (choice.Name == "Credits")
            {
                int tempspeed = scrollSpeed;
                scrollSpeed = 0;
                DialogResult result = MessageBox.Show("Game Created by Daniel and Patryk", "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
                scrollSpeed = tempspeed;

            }
            //Instructions on how to play the game
            else
            {
                int tempspeed = scrollSpeed;
                scrollSpeed = 0;
                DialogResult result = MessageBox.Show("Use A & D to move left and right and attempt to collect as many yellow squares as you can! \n\n Candy: 1 point \nBlue Crisps: 5 points \nWater: -10 Points", "Instructions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                scrollSpeed = tempspeed;
            }
        }

        /*
         * The following method handles what happens when the user tries to change
         * the difficulty through the menustrip
         */
        private void difficultyClickHandler(object sender, EventArgs e)
        {
            //Changing difficulty changes the scroll speed then refreshes
            ToolStripMenuItem difficultySelected = sender as ToolStripMenuItem;
            if (difficultySelected.Name == "Easy")
            {
                scrollSpeed = 10;
                refreshGame();
            }
            else if (difficultySelected.Name == "Default")
            {
                scrollSpeed = 20;
                refreshGame();
            }
            else
            {
                scrollSpeed = 30;
                refreshGame();
            }
        }

        /*
         * The following method Handles keypresses
         */
        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if A or D pressed
            if (e.KeyChar >= 97 && e.KeyChar <= 100)
            {
                switch (e.KeyChar)
                {
                    case (char)97:
                        playerMove(0);
                        e.Handled = true;
                        break;

                    case (char)100:
                        playerMove(1);
                        e.Handled = true;
                        break;
                }
            }
        }

        /*
         * The following method decides what to do depending on which way the player wants 
         * to move
         */
        void playerMove(int n)
        {
            //stop the player from moving out of the vending machine
            if(n == 1 && (player.Location.X < 350))
            {

                    player.Left = player.Left + 10;

            }
            else if(n == 0 && (player.Location.X >= 40))
            {

                player.Left = player.Left - 10;

            }
        }

        // https://stackoverflow.com/questions/21644006/how-to-trigger-an-event-every-specific-time-interval-in-c -25/01/19

        /*
         * The following method is responsible for setting up the initial timer
         */
        public void StartGame()
        {
            _startTime = DateTime.Now;
            _timer = new System.Timers.Timer(600); // 10 seconds
            _timer.Elapsed += timer_Elapsed;
            _timer.Enabled = true;
        }

        /*
         * What to do when the timer ticks over
         */
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            TimeSpan timeSinceStart = DateTime.Now - _startTime; 
            Random rnd = new Random();
            //check if the game has been won yet
            if (!gameWon)
            {
                //get a random number between 1 and 3
                int ammount = rnd.Next(1, 30);

                //attempt to spawn an enemy 3 times
                for (int i = 0; i < ammount; i++)
                {

                    int x = rnd.Next(0, 9);
                    int y = rnd.Next(0, 13);

                    //if the box is off screen, spawn the enemy
                    if (grid[x, y].Location.Y > 460 && grid[x, y].BackColor != Color.Yellow)
                    {

                        grid[x, y].BackColor = enemy;

                        int enemyImage = rnd.Next(0, 20);

                        if (enemyImage >= 0 && enemyImage <= 10)
                        {
                            int n = rnd.Next(0, 5);

                            switch (n)
                            {
                                case 0:
                                    grid[x, y].Image = candy;
                                    break;
                                case 1:
                                    grid[x, y].Image = candyBlue;
                                    break;
                                case 2:
                                    grid[x, y].Image = candyGreen;
                                    break;
                                case 3:
                                    grid[x, y].Image = candyOrange;
                                    break;
                                case 4:
                                    grid[x, y].Image = candyPink;
                                    break;
                            }

                        }
                        else if(enemyImage == 13)
                        {
                            grid[x, y].Image = crisps;

                        }
                        else if (enemyImage == 14)
                        {
                            grid[x, y].Image = crispsGreen;

                        }
                        else
                        {
                            grid[x, y].Image = urWater;

                        }

                    }
                }
            }
        }

        //delete this method before submitting, only used to quickly jump from form to code through double click
        private void GameForm_Load(object sender, EventArgs e)
        {
            


        }

        /*
         * The following method refreshes the grid of pictureboxs and also resets all the
         * gameplay based values to default
         */
        private void refreshGame()
        {
            gameWon = false;
            score = 0;
            gameTime = 30;

            for (int x = 0; x < grid.GetLength(0); x++)
            {

                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y].Image = null;       
                    grid[x, y].BackColor = backg;

                }

            }
        }
        /*
         * The following method takes in the x and y position of a picture box in the grid
         * and then checks if the player is touching that spot
         */
        public void checkCollision(int x, int y)
        {
            //check if the player is on the same location as the x and y values inputted
            bool checkX = (grid[x,y].Location.X >= player.Location.X && grid[x,y].Location.X <= player.Location.X+40) || (grid[x,y].Location.X+40 >= player.Location.X && grid[x,y].Location.X +40 <= player.Location.X+40);
            bool checkY = (grid[x,y].Location.Y >= player.Location.Y && grid[x,y].Location.Y <= player.Location.Y+40) || (grid[x,y].Location.Y+40 >= player.Location.Y && grid[x,y].Location.Y+40 <= player.Location.Y+40);
            bool collision = checkX && checkY;

            //if the player is touching a yellow box, add some points and remove the box
            if(grid[x,y].BackColor == Color.Yellow && collision == true)
            {

                snd.Play();
                if (grid[x, y].Image == urWater)
                {
                    score -= 10;

                }
                else if(grid[x, y].Image == crisps)
                {
                    score += 5;

                }
                else
                {
                    score++;
                }

                grid[x,y].BackColor = backg;
                grid[x, y].Image = null;

            }

        }
        /*
         * The following method determines what happens on the background timers click
         */
        private void backgroundMoveTime_Tick(object sender, EventArgs e)
        {
            //check if the game has been won yet
            if (!gameWon)
            {
                //if game has not been won, check if there is any time left on the timer
                if (Math.Round(gameTime) == 0)
                {
                    //set the game to be won
                    gameWon = true;
                    //open the scoreboard
                    var newForm = new popUpForm(score);
                    var result = newForm.ShowDialog();

                    //wait for input from the scorboard
                    if (result == DialogResult.OK)
                    {
                        //if the player wants to play again, call refreshGame()
                        if (newForm.playAgain)
                        {
                            refreshGame();
                        }
                        else
                        {
                            //else quit back to menu
                            this.DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                }
                else //else if game has not finished
                {
                    //tick down the game timer
                    gameTime -= 0.01;

                    //update labels
                    timerDisplay.Text = Convert.ToString(Math.Round(gameTime));
                    scoreDisplay.Text = "Score " + Convert.ToString(score);

                }

                //following loop scrolls up the background
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    //move the background up
                    grid[bottomRow, y].Top -= scrollSpeed;

                    //check collision and move GUI to top
                    checkCollision(bottomRow, y);
                    player.BringToFront();
                    gameMenu.BringToFront();

                    //checks if the picturebox is off the screen
                    if (grid[bottomRow, y].Top <= 0)
                    {
                        grid[bottomRow, y].Top = 500;
                        grid[bottomRow, y].BackColor = backg;
                        grid[bottomRow, y].Image = null;
                        //change the bottom row to hold the new bottom row
                        bottomRow--;
                    }

                    //if the bottom row has gone negative, reset it
                    if (bottomRow == -1)
                    {
                        bottomRow = 8;
                    }
                }

            }
            
        }

    }

}


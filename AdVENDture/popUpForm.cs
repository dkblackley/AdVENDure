using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace AdVENDture
{
    public partial class popUpForm : Form
    {
        //variables to be used in form
        private int score;
        private bool scoreEntered = false;
        public bool playAgain;

        //GUI elements
        private Button displayScoresButton, quitButton, playAgainButton;
        private Label playerScore, playersScores, name, playerNames;
        private TextBox inputName;
        private TextBox[,] scores;

        //form styling
        String font = "Verdana";
        int fontSize = 12;
        Color colour = Color.Black;

        //constructor
        public popUpForm(int points)
        {

            InitializeComponent();
            this.score = points;
            setupForm();
            this.ControlBox = false;

        }

        /*
         * Form responsible for setting up the glabal variables
         */
        private void setupForm()
        {
            //load the background from file
            this.BackgroundImage = new Bitmap(AdVENDture.Properties.Resources.Sky);

            //memory allocation
            scores = new TextBox[5, 2];
            inputName = new TextBox();
            inputName.SetBounds(225, 40, 255, 20);
            playerScore = new Label();
            playersScores = new Label();
            name = new Label();
            playerNames = new Label();
            displayScoresButton = new Button();
            quitButton = new Button();
            playAgainButton = new Button();


            setupButtons();
            setupLabels();
            setupTextBoxes();

            string[,] scoresLoaded = loadScores();
            setScores(scoresLoaded);

        }

        /*
         * Sets up the positioning and other various settings for the textboxes
         */
        private void setupTextBoxes()
        {
            //space between boxes
            int step = 150;
            for (int i = 0; i < 5; i++)
            {
                scores[i, 0] = new TextBox();
                scores[i, 1] = new TextBox();
                scores[i, 0].SetBounds(110, step, 180, 20);
                scores[i, 1].SetBounds(300, step, 180, 20);
 

                step += 25;

                Controls.Add(scores[i, 0]);
                Controls.Add(scores[i, 1]);
            }
            Controls.Add(inputName);
        }

        /*
         * Sets up the positioning and other various settings for the Labels
         */
        private void setupLabels()
        {

            playerScore.ForeColor = colour;
            playerScore.SetBounds(250, 10, 150, 20);
            playerScore.Font = new Font(font, fontSize,FontStyle.Bold);
            playerScore.Text = "Score: " + Convert.ToString(score);
            playerScore.BackColor = Color.Transparent;

            playersScores.ForeColor = colour;
            playersScores.Font = new Font(font, fontSize, FontStyle.Bold);
            playersScores.Location = new Point(295, 120);
            playersScores.Text = "Score:";
            playersScores.BackColor = Color.Transparent;

            name.ForeColor = colour;
            name.BackColor = Color.Transparent;
            name.Font = new Font(font, fontSize, FontStyle.Bold);
            name.SetBounds(105, 40, 120, 40);
            name.Text = "Enter Name:";

            playerNames.ForeColor = colour;
            playerNames.Font = new Font(font, fontSize, FontStyle.Bold);
            playerNames.Location = new Point(105, 120);
            playerNames.Text = "Name:";
            playerNames.BackColor = Color.Transparent;

            Controls.Add(playerScore);
            Controls.Add(playersScores);
            Controls.Add(name);
            Controls.Add(playerNames);
        }

        /*
         * Sets up the positioning and other various settings for the Buttons
         */
        private void setupButtons()
        {

            quitButton.BackColor = Color.Transparent;
            quitButton.FlatStyle = FlatStyle.Flat;
            quitButton.FlatAppearance.BorderSize = 0;
            quitButton.SetBounds(50, 300, 100, 50);
            quitButton.Text = "Return.";
            quitButton.Image = new Bitmap(AdVENDture.Properties.Resources.back);
            quitButton.FlatAppearance.MouseOverBackColor = Color.LightBlue;
            quitButton.Click += new EventHandler(quitButton_Click);

            playAgainButton.BackColor = Color.Transparent;
            playAgainButton.FlatStyle = FlatStyle.Flat;
            playAgainButton.FlatAppearance.BorderSize = 0;
            playAgainButton.SetBounds(450, 300, 100,50);
            playAgainButton.Image = new Bitmap(AdVENDture.Properties.Resources.replay);
            playAgainButton.FlatAppearance.MouseOverBackColor = Color.LightBlue;
            playAgainButton.Click += new EventHandler(playAgainButton_Click);

            displayScoresButton.BackColor = Color.LightBlue;
            displayScoresButton.FlatStyle = FlatStyle.Flat;
            displayScoresButton.Font = new Font(font, fontSize, FontStyle.Bold);
            displayScoresButton.FlatAppearance.MouseOverBackColor = Color.LightBlue;
            displayScoresButton.FlatAppearance.BorderSize = 0;
            displayScoresButton.SetBounds(250, 70, 100, 30);
            displayScoresButton.Text = "SUBMIT";
            displayScoresButton.Click += new EventHandler(displayScoresButton_Click);

            Controls.Add(displayScoresButton);
            Controls.Add(playAgainButton);
            Controls.Add(quitButton);

        }

        /*
         * Event handler for when the submit button is clicked
         */
        private void displayScoresButton_Click(object sender, EventArgs e)
        {
            //Input validation
            if (!scoreEntered && inputName.Text != "" && Convert.ToInt32(scores[4, 1].Text) < score)
            {
                //if the score made it onto the leaderboard, save the new leaderboard
                saveScores(inputName.Text);
            }
        }

        /*
         * The following method Loads the scores from the files and returns them as an array of strings
         */
        string[,] loadScores()
        {
            //takes the scores from the saved file
            StreamReader scoreReader = File.OpenText("../../../scores.csv");
            string[,] scores = new String[5, 2];
            string tempLine;
            string[] tempSplit = new string[2];

            //open and read in each line in file
            for (int i = 0; (tempLine = scoreReader.ReadLine()) != null; i++)
            {
                //split the csv file
                tempSplit = tempLine.Split(',');
                scores[i, 0] = tempSplit[0];
                scores[i, 1] = tempSplit[1];
            }
            //close the reader
            scoreReader.Close();

            return scores;
        }

        //remove this from final upload
        private void popUpForm_Load(object sender, EventArgs e)
        {

        }

        /*
         * The following method takes in scores and displays them on screen in the  relevant textboxes
         */
        void setScores(string[,] scoresToDisplay)
        {

            for (int i = 0; i < 5; i++)
            {
                scores[i, 0].Text = scoresToDisplay[i, 0];
                scores[i, 1].Text = scoresToDisplay[i, 1];
            }
        }

        /*
         * The following method checks to see if the score entered is actually
         * a score that can be put onto a place in the leaderboard
         */
        void saveScores(string name)
        {

            string[,] scoresLoaded = loadScores();
            //scoreHite determines whether the other scores need to be pushed down the leaderboard
            bool scoreHit = false;
            string tempName1, tempScore1, tempName, tempScore;

            tempName = null;
            tempScore = null;
            tempName1 = null;
            tempScore1 = null;

            //cycle through each position on the leaderboard
            for (int i = 0; i < 5; i++)
            {
                //if the score has been saved in a previous position
                if (scoreHit)
                {
                    //push the score down by 1 position
                    tempName1 = scoresLoaded[i, 0];
                    tempScore1 = scoresLoaded[i, 1];
                    scoresLoaded[i, 0] = tempName;
                    scoresLoaded[i, 1] = tempScore;

                    tempName = tempName1;
                    tempScore = tempScore1;


                }
                //if the score is bigger than the current score
                else if (Convert.ToInt32(scoresLoaded[i, 1]) < score)
                {
                    //set scoreHit to true and then bump the current score down by one
                    scoreHit = true;
                    tempName = scoresLoaded[i, 0];
                    tempScore = scoresLoaded[i, 1];

                    scoresLoaded[i, 0] = name;
                    scoresLoaded[i, 1] = Convert.ToString(score);

                }
            }

            setScores(scoresLoaded);
            saveScoresToFile(scoresLoaded);
        }

        /*
         * The following method overwrites the previous scores with the new ones
         */
        void saveScoresToFile(string[,] scoresToSave)
        {
            //make a csv file
            StreamWriter streamWriter = File.CreateText("../../../scores.csv");

            //cycle through the scores
            for (int i = 0; i < 5; i++)
            {
                //write each line to the file
                streamWriter.WriteLine(scoresToSave[i, 0] + ',' + scoresToSave[i, 1]);
            }

            //close the writer
            streamWriter.Close();
        }

        /*
         * What to do if the player clicks the playagain button
         */
        private void playAgainButton_Click(object sender, EventArgs e)
        {
            //set play again to true so that the gameform can check whether it should start up again
            this.playAgain = true;
            this.DialogResult = DialogResult.OK;
            Close();

        }
        /*
         * What to do if the player clicks the quit button
         */
        private void quitButton_Click(object sender, EventArgs e)
        {

            this.playAgain = false;
            this.DialogResult = DialogResult.OK;
            Close();

        }

        /*
         * This method is called if the user checks the scoreboard from the main menu
         */
        public void menuMode()
        {
            //hides all the functions for playing again or submitting a score
            playAgainButton.Visible = false;
            playerScore.Visible = false;
            name.Visible = false;
            displayScoresButton.Visible = false;
            inputName.Visible = false;

        }
    }
}

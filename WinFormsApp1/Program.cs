// Program: Snowman Game
// Written by: Sam Adams, Kevin Graziosi, Lenny Tran
// Date: 2/20/25
// This program lets a user play the Snowman Game. A random word is chosen from a file, and the player must guess that random word
// within 6 guesses. For each incorrect guess, a part of the snowman is drawn. If all parts of the snowman are drawn, you lose. For each
// correct guess, all instances of the letter are revealed in the word. For every guess, the letter is added to an array of guessed words
// that is shown at the bottom of the screen. When the player guesses the word, they win. 

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;
using System.Media;
using Microsoft.VisualBasic.ApplicationServices;

public class SnowmanGame : Form
{
    // Our global variables
    private Label lblWord, lblGuesses, lblFeedback, lblWelcome, lblHelpInfo, lblGuessed, lblYourGuesses, lblWordTest;
    private TextBox txtGuess;
    private Button btnGuess, btnStartGame, btnHelp, btnBack;
    private PictureBox pbSnowmanDraw, pbSnowmanWelcome;
    private char[] guessedLetters = new char[0];
    private int guessesLeft = 6;
    private static int numGuesses = 0;
    public string randomWord;
    public string progressedWord;
    private Panel welcomePanel, gamePanel;

    
    public SnowmanGame()
    {
        Text = "Snowman Game";
        AutoSize = true;
        InitializeComponents(); // Show welcome screen first
        ShowWelcomeScreen(null, null);
    }

    private void InitializeComponents()
    {
        string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "festv-frlix.wav"); // made a sound folder so the sound can be accessed on any device
        System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundPath);
        player.Play();

        // Welcome Panel
        welcomePanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.LightBlue };

        lblWelcome = new Label
        {
            Text = "Welcome to Snowman Game",
            Font = new Font("The Heart Maze Demo", 22, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(550, 120)
        };

        lblYourGuesses = new Label
        {
            Text = "Your Guesses:",
            Font = new Font("The Heart Maze Demo", 22, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(650, 600)
        };

        lblGuessed = new Label
        {
            Text = "",
            Font = new Font("Arial", 18, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(675, 700)
        };

        btnStartGame = new Button
        {
            Text = "Start Game",
            Location = new Point(700, 600),
            Size = new Size(100, 40)
        };
        btnStartGame.Click += StartGame;

        pbSnowmanWelcome = new PictureBox
        {
            Location = new Point(600, 200),
            Size = new Size(310, 350)
        };

        pbSnowmanDraw = new PictureBox
        {
            Location = new Point(600, 200),
            Size = new Size(310, 350)
        };

        pbSnowmanWelcome.Image = new Bitmap(310, 350);
        using (Graphics g = Graphics.FromImage(pbSnowmanWelcome.Image))
        {
            // Clear the background
            g.Clear(Color.Transparent); 

            // Draw the snowman
            g.DrawEllipse(Pens.Black, 110, 55, 80, 80); // Head (repositioned slightly lower)
            g.DrawEllipse(Pens.Black, 100, 135, 95, 95); // Body (repositioned to align with the head)
            g.DrawLine(Pens.Black, 80, 150, 110, 170); // Left Arm (repositioned)
            g.DrawLine(Pens.Black, 220, 150, 190, 170); // Right Arm (repositioned)
            g.DrawEllipse(Pens.Black, 90, 230, 115, 115); // Base (repositioned to align with the body)

            // Draw the hat on top of the head
            g.FillRectangle(Brushes.Black, 130, 10, 50, 50); // Hat top (rectangle, positioned above the head)
            g.FillRectangle(Brushes.Black, 110, 60, 90, 10); // Hat brim (wider rectangle, positioned below the hat top)
        }

        lblHelpInfo = new Label // displays instructions on the home screen
        {
            Text = "You must guess letters in the random word." +
                " Letters a-z, hyphens, and apostrophes are valid characters." +
                " You have 6 guesses until the snowman is complete!",
            Location = new Point(525, 700),
            Width = 500,
            Height = 400,
            Font = new Font("Arial", 14, FontStyle.Regular)
        };

        welcomePanel.Controls.Add(pbSnowmanWelcome); // add these attributes to be displayed on the welcome screen
        welcomePanel.Controls.Add(lblWelcome);
        welcomePanel.Controls.Add(btnStartGame);
        welcomePanel.Controls.Add(lblHelpInfo);
        Controls.Add(welcomePanel);

        // Game Panel (Hidden Initially)
        gamePanel = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = Color.LightBlue }; 

        lblWord = new Label { Text = "", Location = new Point(150, 400), Width = 200, Font = new Font("Arial", 18, FontStyle.Bold) }; // parts of the game screen
        lblGuesses = new Label { Text = "Guesses Left: 6", Location = new Point(1000, 400), AutoSize = true, Font = new Font("Arial", 18, FontStyle.Bold)};
        lblFeedback = new Label { Text = "", Location = new Point(1000, 450), AutoSize = true, Font = new Font("Arial", 18, FontStyle.Bold) };

        txtGuess = new TextBox { Location = new Point(140, 450), Width = 50, MaxLength = 1 };
        btnGuess = new Button { Text = "Guess", Location = new Point(200, 450), Width = 80, Font = new Font("Arial", 12) };
        btnGuess.Click += MakeGuess;

        gamePanel.Controls.Add(lblWord); // add parts of game attributes to the game screen
        gamePanel.Controls.Add(lblGuesses);
        gamePanel.Controls.Add(lblGuessed);
        gamePanel.Controls.Add(lblFeedback);
        gamePanel.Controls.Add(lblYourGuesses);
        gamePanel.Controls.Add(lblWordTest);
        gamePanel.Controls.Add(txtGuess);
        gamePanel.Controls.Add(btnGuess);
        gamePanel.Controls.Add(pbSnowmanDraw);
        Controls.Add(gamePanel);

        player.Play(); // play music
    }

    // Shows the welcome screen
    private void ShowWelcomeScreen(object sender, EventArgs e = null)
    {
        welcomePanel.Visible = true; // only show the welcome panel and attributes when this function is called
        gamePanel.Visible = false;
    }

    // Starts the game
    private void StartGame(object sender, EventArgs e = null)
    {
        welcomePanel.Visible = false;
        gamePanel.Visible = true; // now show the gameplay panel only when this function is called
        TextReader gameRandomString = new TextReader(); // initialize new text reader object
        string myString = gameRandomString.ReadRandomString(); // get a random string using text reader
        randomWord = myString;
        progressedWord = "";
        for (int i = 0; i < randomWord.Length; i++)
        {
            progressedWord += "~"; // at first, none of the blanks in the progressed word are filled
        }
        lblWord.Text = progressedWord; // set the first display of lblword to the non-guessed progressed word
        guessesLeft = 6; // start with 6 guesses for player
        lblYourGuesses.Text = "Your Guesses: ";
        lblGuesses.Text = "Guesses Left: 6";
        lblFeedback.Text = "";
        UpdateSnowman();
    }

    private void MakeGuess(object sender, EventArgs e)
    {
        //If nothing is entered
        if (txtGuess.Text.Length == 0) return;
        char userChar = char.ToLower(txtGuess.Text[0]);
        txtGuess.Clear();

        
        //Ensure that only valid characters will be typed
        if ((userChar < 97 || userChar > 122) && userChar != 45 && userChar != 39) {
            lblFeedback.Text = "Please only enter valid characters.";
            return;
        // If they guessed a letter correctly
        } else if (randomWord.Contains(userChar)) {
            for (int i = 0; i < randomWord.Length; i++)
            {
                //Edit the string so that the right chars will be revealed
                if (randomWord[i] == userChar)
                {
                    // Takes the progressed word and makes it into a StringBuilder object so we can replace the correct letter with the user guess 
                    StringBuilder sb = new StringBuilder(progressedWord);
                    sb[i] = userChar;
                    progressedWord = sb.ToString();
                }
            }
            lblWord.Text = progressedWord;
            lblFeedback.Text = "Correct!";
        }
        else
        {
            // If the guess hasn't been guessed already, and the random word doesn't contain the guess
            if (!guessedLetters.Contains(userChar) && !randomWord.Contains(userChar))
            {
                // Decrement guesses left and update the visuals
                guessesLeft--;
                lblGuesses.Text = $"Guesses Left: {guessesLeft}";
                lblGuesses.BringToFront();
                lblFeedback.Text = "Wrong guess!";
                UpdateSnowman();
            }
        }

        //If user guesses the same letter
        if (guessedLetters.Contains(userChar)) {
            lblFeedback.Text = "You already guessed this letter.";
            return;
        } else {
            //Add the guessed letter to guessedLetters array
            guessedLetters = guessedLetters.Append(userChar).ToArray();
            Array.Sort(guessedLetters);
            
            //Display the guessed letters
            for(int i = 0; i < guessedLetters.Length; i++)
            {
                if (lblGuessed.Text.Contains(guessedLetters[i])) {
                    continue;
                }
                lblGuessed.Text += guessedLetters[i];
            }
            numGuesses++;
        }

        string text = lblGuessed.Text;
        // Convert the string to a character array
        char[] characters = text.ToCharArray();
        // Sort the characters (alphabetically)
        Array.Sort(characters);
        // Convert the sorted characters back to a string
        string sortedText = new string(characters);
        // Update the label with the sorted text
        lblGuessed.Text = sortedText;

        // If we guessed the random word
        if (progressedWord == randomWord) {
            // show win screen and reset so the user can play again
            MessageBox.Show("You win! Congrats.");
            lblGuesses.Text = string.Empty;
            lblGuessed.Text = string.Empty;
            lblYourGuesses.Text = string.Empty;
            Array.Clear(guessedLetters, 0, guessedLetters.Length);
            welcomePanel.Visible = true;
            guessesLeft = 6;
            lblGuesses.Refresh();
            lblFeedback.Refresh();
        // If the user used all their guesses
        } else if (guessesLeft == 0) {
            // show loss screen and reset so the user can play again
            MessageBox.Show("You lose! The word was: " + randomWord);
            lblGuesses.Text = string.Empty;
            lblGuessed.Text = string.Empty;
            lblFeedback.Text = string.Empty;
            lblYourGuesses.Text = string.Empty;
            Array.Clear(guessedLetters, 0, guessedLetters.Length);
            welcomePanel.Visible = true;
            guessesLeft = 6;
            lblGuesses.Refresh();
            lblFeedback.Refresh();
            Console.WriteLine("Labels cleared. Guesses: " + lblGuesses.Text + ", Feedback: " + lblFeedback.Text);
        }
    }

    // Draws the different body parts of the snowman
    private void UpdateSnowman()
    {

        pbSnowmanDraw.Image = new Bitmap(310, 350);
        using (Graphics g = Graphics.FromImage(pbSnowmanDraw.Image))
        {
            // Clear the background (optional, depending on your design)
            g.Clear(Color.Transparent); // Or use a background color like Color.White

            // Draw the snowman
            if(guessesLeft <= 3) g.DrawEllipse(Pens.Black, 110, 55, 80, 80); // Head (repositioned slightly lower)
            if(guessesLeft <= 4) g.DrawEllipse(Pens.Black, 100, 135, 95, 95); // Body (repositioned to align with the head)
            if(guessesLeft <= 1) g.DrawLine(Pens.Black, 80, 150, 110, 170); // Left Arm (repositioned)
            if(guessesLeft <= 0) g.DrawLine(Pens.Black, 220, 150, 190, 170); // Right Arm (repositioned)
            if(guessesLeft <= 5) g.DrawEllipse(Pens.Black, 90, 230, 115, 115); // Base (repositioned to align with the body
            if (guessesLeft <= 2)
            {
                g.FillRectangle(Brushes.Black, 130, 10, 50, 50); // Hat top (rectangle, positioned above the head)
                g.FillRectangle(Brushes.Black, 110, 60, 90, 10);
            }// Hat brim (wider rectangle, positioned below the hat top)
        }

    }

    // Start windows forms application
    public static void Main()
    {
        Application.Run(new SnowmanGame());
    }
}

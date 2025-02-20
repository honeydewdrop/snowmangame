// Program: Snowman Game
// Written by: Sam Adams, Kevin Graziosi, Lenny Tran
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

public class SnowmanGame : Form
{
    private Label lblWord, lblGuesses, lblFeedback, lblWelcome, lblHelpInfo, lblGuessed, lblYourGuesses, lblWordTest;
    private TextBox txtGuess;
    private Button btnGuess, btnStartGame, btnHelp, btnBack;
    private PictureBox pbSnowman, pbSnowmanWelcome;
    private char[] guessedLetters = new char[0];
    private int guessesLeft = 6;
    private static int numGuesses = 0;
    public string randomWord;
    public string progressedWord;
    private Panel welcomePanel, gamePanel;

    public SnowmanGame()
    {
        Text = "Snowman Game";
        Size = new Size(800, 600);
        InitializeComponents(); // Show welcome screen first
        ShowWelcomeScreen(null, null);
    }

    private void InitializeComponents()
    {
        // Welcome Panel
        welcomePanel = new Panel { Size = new Size(800, 600), BackColor = Color.LightBlue };

        lblWelcome = new Label
        {
            Text = "Welcome to Snowman Game",
            Font = new Font("The Heart Maze Demo", 22, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(175, 100)
        };

        lblWordTest = new Label
        {
            Text = "",
            Font = new Font("Arial", 16),
            Location = new Point(225, 300)
        };

        lblYourGuesses = new Label
        {
            Text = "Your Guesses:",
            Font = new Font("The Heart Maze Demo", 22, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(275, 450)
        };

        lblGuessed = new Label
        {
            Text = "",
            Font = new Font("Arial", 18, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(300, 500)
        };

        btnStartGame = new Button
        {
            Text = "Start Game",
            Location = new Point(350, 300),
            Size = new Size(100, 40)
        };
        btnStartGame.Click += StartGame;

        pbSnowmanWelcome = new PictureBox
        {
            Location = new Point(100, 170),
            Size = new Size(250, 250)
        };

        pbSnowmanWelcome.Image = new Bitmap(250, 250);
        using (Graphics g = Graphics.FromImage(pbSnowmanWelcome.Image))
        {
            g.DrawEllipse(Pens.Black, 50, 10, 50, 50); // Head
            g.DrawEllipse(Pens.Black, 40, 60, 70, 70); // Body
            g.DrawLine(Pens.Black, 20, 70, 50, 90); // Left Arm
            g.DrawLine(Pens.Black, 120, 70, 90, 90); // Right Arm
            g.DrawEllipse(Pens.Black, 40, 130, 80, 80); // Base
            g.FillRectangle(Brushes.Black, 58, -15, 40, 40);  // Hat top (rectangle)
            g.FillRectangle(Brushes.Black, 50, 18, 60, 10);  // Hat brim (wider rectangle)
        }

        lblHelpInfo = new Label
        {
            Text = "You must guess letters in the random word." +
                " Letters a-z, hyphens, and apostrophes are valid characters." +
                " You have 6 guesses until the snowman is complete!",
            Location = new Point(160, 450),
            Width = 500,
            Height = 400,
            Font = new Font("Arial", 14, FontStyle.Regular)
        };


        welcomePanel.Controls.Add(pbSnowmanWelcome);
        welcomePanel.Controls.Add(lblWelcome);
        welcomePanel.Controls.Add(btnStartGame);
        welcomePanel.Controls.Add(lblHelpInfo);
        Controls.Add(welcomePanel);

        // Game Panel (Hidden Initially)
        gamePanel = new Panel { Size = new Size(800, 600), Visible = false, BackColor = Color.LightBlue};

        lblWord = new Label { Text = "", Location = new Point(20, 20), Width = 200 };
        lblGuesses = new Label { Text = "Guesses Left: 6", Location = new Point(20, 50) };
        lblFeedback = new Label { Text = "", Location = new Point(20, 80), Width = 300 };

        txtGuess = new TextBox { Location = new Point(20, 110), Width = 50, MaxLength = 1 };
        btnGuess = new Button { Text = "Guess", Location = new Point(80, 110) };
        btnGuess.Click += MakeGuess;

        pbSnowman = new PictureBox { Location = new Point(300, 200), Size = new Size(350, 350) };

        gamePanel.Controls.Add(lblWord);
        gamePanel.Controls.Add(lblGuesses);
        gamePanel.Controls.Add(lblGuessed);
        gamePanel.Controls.Add(lblFeedback);
        gamePanel.Controls.Add(lblYourGuesses);
        gamePanel.Controls.Add(lblWordTest);
        gamePanel.Controls.Add(txtGuess);
        gamePanel.Controls.Add(btnGuess);
        gamePanel.Controls.Add(pbSnowman);
        Controls.Add(gamePanel);

    }

    private void ShowWelcomeScreen(object sender, EventArgs e = null)
    {
        welcomePanel.Visible = true;
        gamePanel.Visible = false;
    }

    private void StartGame(object sender, EventArgs e = null)
    {
        welcomePanel.Visible = false;
        gamePanel.Visible = true;
        TextReader gameRandomString = new TextReader();
        string myString = gameRandomString.ReadRandomString();
        randomWord = myString;
        progressedWord = "";
        for (int i = 0; i < randomWord.Length; i++)
        {
            progressedWord += "~";
        }
        lblWord.Text = progressedWord;
        guessesLeft = 6;
        //lblWordTest.Text = randomWord;
        lblYourGuesses.Text = "Your Guesses: ";
        lblGuesses.Text = "Guesses Left: 6";
        //guessedLetters = new char[0];
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
        } else if (randomWord.Contains(userChar)) {
            for (int i = 0; i < randomWord.Length; i++)
            {
                //Edit the string so that the right chars will be revealed
                if (randomWord[i] == userChar)
                {
                    //progressedWord = progressedWord.Remove(i, 1).Insert(i, Char.ToString(userChar));
                    StringBuilder sb = new StringBuilder(progressedWord);
                    sb[i] = userChar;
                    progressedWord = sb.ToString();
                } // revealChar from before
            }
            lblWord.Text = progressedWord;
            lblFeedback.Text = "Correct!";
        }
        else
        {
            if (!guessedLetters.Contains(userChar) && !randomWord.Contains(userChar))
            {
                guessesLeft--;
                lblGuesses.Text = $"Guesses Left: {guessesLeft}";
                lblFeedback.Text = "Wrong guess!";
                UpdateSnowman();
            }
        }

        if (guessedLetters.Contains(userChar)) {
            lblFeedback.Text = "You already guessed this letter.";
            return;
        } else {
            guessedLetters = guessedLetters.Append(userChar).ToArray();
            Array.Sort(guessedLetters);
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

        if (progressedWord == randomWord) {
            MessageBox.Show("You win! Congrats.");
            lblGuesses.Text = string.Empty;
            lblFeedback.Text = string.Empty;
            Array.Clear(guessedLetters, 0, guessedLetters.Length);
            welcomePanel.Visible = true;
            guessesLeft = 6;
            lblGuesses.Refresh();
            lblFeedback.Refresh();
        } else if (guessesLeft == 0) {
            MessageBox.Show("You lose! The word was: " + randomWord);
            lblGuesses.Text = string.Empty;
            lblFeedback.Text = string.Empty;
            Array.Clear(guessedLetters, 0, guessedLetters.Length);
            welcomePanel.Visible = true;
            guessesLeft = 6;
            lblGuesses.Refresh();
            lblFeedback.Refresh();
            Console.WriteLine("Labels cleared. Guesses: " + lblGuesses.Text + ", Feedback: " + lblFeedback.Text);
        }
    }

    private void UpdateSnowman()
    {
        pbSnowman.Image = new Bitmap(350, 350);
        using (Graphics g = Graphics.FromImage(pbSnowman.Image))
        {
            if (guessesLeft <= 3) g.DrawEllipse(Pens.Black, 50, 10, 50, 50); // Head
            if (guessesLeft <= 4) g.DrawEllipse(Pens.Black, 40, 60, 70, 70); // Body
            if (guessesLeft <= 1) g.DrawLine(Pens.Black, 20, 70, 50, 90); // Left Arm
            if (guessesLeft <= 0) g.DrawLine(Pens.Black, 120, 70, 90, 90); // Right Arm
            if (guessesLeft <= 5) g.DrawEllipse(Pens.Black, 40, 130, 80, 80); // Base
            if (guessesLeft == 2){
                g.FillRectangle(Brushes.Black, 58, -15, 40, 40);  // Hat top (rectangle) 
                g.FillRectangle(Brushes.Black, 50, 18, 60, 10);  // Hat brim (wider rectangle)
            }
        }
    }

    public static void Main()
    {
        Application.Run(new SnowmanGame());
    }
}

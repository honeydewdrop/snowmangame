namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "My Windows Form"; // Set title
            this.BackColor = Color.Blue;   // Set background color
            this.Size = new Size(800, 600); // Set window size
        }
    }
}

//pbSnowmanWelcome.Image = new Bitmap(150, 150);
//using (Graphics g = Graphics.FromImage(pbSnowman.Image))
//{
//    g.DrawEllipse(Pens.Black, 50, 10, 50, 50); // Head
//    g.DrawEllipse(Pens.Black, 40, 60, 70, 70); // Body
//    g.DrawLine(Pens.Black, 20, 70, 50, 90); // Left Arm
//    g.DrawLine(Pens.Black, 120, 70, 90, 90); // Right Arm
//    g.DrawEllipse(Pens.Black, 45, 130, 60, 60); // Base
//}
//btnHelp.Click += DisplayHelp;



//welcomePanel.Controls.Add(lblWelcome);
//welcomePanel.Controls.Add(btnStartGame);
//welcomePanel.Controls.Add(btnHelp);
//welcomePanel.Controls.Add(pbSnowmanWelcome);
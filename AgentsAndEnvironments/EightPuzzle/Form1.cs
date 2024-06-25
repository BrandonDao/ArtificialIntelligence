using SharedLibrary;
using SharpDX.WIC;

namespace EightPuzzle;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        visualizer1.IsPaused = true;

        forwardButton.Enabled = false;
        backButton.Enabled = false;
        pauseButton.Enabled = false;
        resetButton.Enabled = false;
    }

    private void AnimationSpeedTrackbar_Scroll(object sender, EventArgs e)
    {
        visualizer1.AnimationDelay = TimeSpan.FromMilliseconds(animationSpeedTrackbar.Maximum - animationSpeedTrackbar.Value);
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        visualizer1.IsPaused = false;

        startButton.Enabled = false;
        pauseButton.Enabled = true;
        resetButton.Enabled = true;
    }

    private void PauseButton_Click(object sender, EventArgs e)
    {
        visualizer1.IsPaused = true;

        startButton.Enabled = true;
        forwardButton.Enabled = true;
        backButton.Enabled = true;
    }

    private void ResetButton_Click(object sender, EventArgs e)
    {
        visualizer1.IsPaused = true;
        visualizer1.Reset();

        startButton.Enabled = true;
        forwardButton.Enabled = false;
        backButton.Enabled = false;
        pauseButton.Enabled = false;
        resetButton.Enabled = false;
    }

    private void BackButton_Click(object sender, EventArgs e) => visualizer1.GoBack();
    private void ForwardButton_Click(object sender, EventArgs e) => visualizer1.GoForward();

    private void LoadTilesButton_Click(object sender, EventArgs e)
    {
        if (inputTextbox.Text.Length != 9) return;

        visualizer1.LoadTiles(inputTextbox.Text);
    }
}
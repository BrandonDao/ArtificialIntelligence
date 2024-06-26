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

        var tiles = new int[3, 3];
        var linearForm = new int[9];
        var emptyTile = Microsoft.Xna.Framework.Point.Zero;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                linearForm[r * 3 + c] = inputTextbox.Text[r * 3 + c];
                tiles[r, c] = inputTextbox.Text[r * 3 + c] - '0';

                if (inputTextbox.Text[r * 3 + c] == '0')
                {
                    emptyTile = new Microsoft.Xna.Framework.Point(r, c);
                }
            }
        }

        int inversionCount = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                if (linearForm[i] <= 0 || linearForm[j] <= 0 || linearForm[i] <= linearForm[j]) continue;

                inversionCount++;
            }
        }
        if ((inversionCount & 1) != 0) return; 

        visualizer1.LoadTiles(tiles, emptyTile);
    }
}
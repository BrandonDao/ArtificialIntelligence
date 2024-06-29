namespace QLearning;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void UpdateSpeedTrackbar_Scroll(object sender, EventArgs e)
    {
        visualizer1.UpdateDuration = TimeSpan.FromMilliseconds(updateSpeedTrackbar.Maximum - updateSpeedTrackbar.Value);
        autoDecrementTimer.Interval = (int)(visualizer1.UpdateDuration.TotalMilliseconds + 1) * 250;
    }

    private void LearningRateTrackbar_Scroll(object sender, EventArgs e)
    {
        visualizer1.Mouse.learningRate = learningRateTrackbar.Value / 100f;
    }

    private void RewardDecayTrackbar_Scroll(object sender, EventArgs e)
    {
        visualizer1.Mouse.decayFactor = 1f - (rewardDecayTrackbar.Value / 100f);
    }

    private void CostOfLivingTrackbar_Scroll(object sender, EventArgs e)
    {
        visualizer1.Mouse.costOfLiving = costOfLivingTrackbar.Value;
    }

    private void EpsilionTrackbar_Scroll(object sender, EventArgs e)
    {
        visualizer1.Mouse.epsilon = epsilonTrackbar.Value / 100f;
    }

    private void ShowQValsCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        visualizer1.IsShowingQValues = showQValsCheckbox.Checked;
    }

    private void IsRunningCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        visualizer1.IsRunning = isRunningCheckbox.Checked;
    }

    private void AutoDecrementCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        autoDecrementTimer.Enabled = autoDecrementCheckbox.Checked;
        epsilonTrackbar.Enabled = !autoDecrementCheckbox.Checked;
        learningRateTrackbar.Enabled = !autoDecrementCheckbox.Checked;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        costOfLivingTrackbar.Value = 1;
        visualizer1.Mouse.costOfLiving = costOfLivingTrackbar.Value;

        updateSpeedTrackbar.Value = 300;
        visualizer1.UpdateDuration = TimeSpan.FromMilliseconds(updateSpeedTrackbar.Maximum - updateSpeedTrackbar.Value);
        autoDecrementTimer.Interval = (int)visualizer1.UpdateDuration.TotalMilliseconds * 250;

        learningRateTrackbar.Value = 25;
        visualizer1.Mouse.learningRate = learningRateTrackbar.Value / 100f;

        rewardDecayTrackbar.Value = 5;
        visualizer1.Mouse.decayFactor = 1f - (rewardDecayTrackbar.Value / 100f);

        epsilonTrackbar.Value = 50;
        visualizer1.Mouse.epsilon = epsilonTrackbar.Value / 100f;

        skipRenderTrackbar.Enabled = false;
    }

    private void AutoDecrementTimer_Tick(object sender, EventArgs e)
    {
        if (epsilonTrackbar.Value > 0)
        {
            epsilonTrackbar.Value--;
        }
        if (learningRateTrackbar.Value > 0)
        {
            learningRateTrackbar.Value--;
        }
    }

    private void SkipDrawCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        visualizer1.SkipRendering = skipDrawCheckbox.Checked;
        visualizer1.SkipRenderAmount = skipRenderTrackbar.Value;
        skipRenderTrackbar.Enabled = skipDrawCheckbox.Checked;
    }

    private void SkipRenderTrackbar_Scroll(object sender, EventArgs e)
    {
        visualizer1.SkipRenderAmount = skipRenderTrackbar.Value;
    }
}
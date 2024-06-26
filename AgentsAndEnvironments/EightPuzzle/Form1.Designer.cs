namespace EightPuzzle;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        visualizer1 = new Visualizer();
        animationSpeedTrackbar = new TrackBar();
        animationSpeedLabel = new Label();
        inputTextbox = new TextBox();
        formatLabel = new Label();
        loadTilesButton = new Button();
        startButton = new Button();
        pauseButton = new Button();
        resetButton = new Button();
        backButton = new Button();
        forwardButton = new Button();
        ((System.ComponentModel.ISupportInitialize)animationSpeedTrackbar).BeginInit();
        SuspendLayout();
        // 
        // visualizer1
        // 
        visualizer1.AnimationDelay = TimeSpan.Parse("00:00:00");
        visualizer1.IsPaused = false;
        visualizer1.Location = new Point(12, 12);
        visualizer1.MouseHoverUpdatesOnly = false;
        visualizer1.Name = "visualizer1";
        visualizer1.Size = new Size(426, 426);
        visualizer1.TabIndex = 0;
        visualizer1.Text = "visualizer1";
        // 
        // animationSpeedTrackbar
        // 
        animationSpeedTrackbar.LargeChange = 100;
        animationSpeedTrackbar.Location = new Point(444, 30);
        animationSpeedTrackbar.Maximum = 1000;
        animationSpeedTrackbar.Minimum = 50;
        animationSpeedTrackbar.Name = "animationSpeedTrackbar";
        animationSpeedTrackbar.Size = new Size(344, 45);
        animationSpeedTrackbar.SmallChange = 50;
        animationSpeedTrackbar.TabIndex = 1;
        animationSpeedTrackbar.TickFrequency = 25;
        animationSpeedTrackbar.Value = 250;
        animationSpeedTrackbar.Scroll += AnimationSpeedTrackbar_Scroll;
        // 
        // animationSpeedLabel
        // 
        animationSpeedLabel.AutoSize = true;
        animationSpeedLabel.Location = new Point(444, 12);
        animationSpeedLabel.Name = "animationSpeedLabel";
        animationSpeedLabel.Size = new Size(98, 15);
        animationSpeedLabel.TabIndex = 2;
        animationSpeedLabel.Text = "Animation Speed";
        // 
        // inputTextbox
        // 
        inputTextbox.Location = new Point(463, 113);
        inputTextbox.Name = "inputTextbox";
        inputTextbox.Size = new Size(228, 23);
        inputTextbox.TabIndex = 3;
        // 
        // formatLabel
        // 
        formatLabel.AutoSize = true;
        formatLabel.Location = new Point(463, 93);
        formatLabel.Name = "formatLabel";
        formatLabel.Size = new Size(303, 15);
        formatLabel.TabIndex = 4;
        formatLabel.Text = "Format Example: (123456780) represents the solved state";
        // 
        // loadTilesButton
        // 
        loadTilesButton.Location = new Point(697, 113);
        loadTilesButton.Name = "loadTilesButton";
        loadTilesButton.Size = new Size(75, 23);
        loadTilesButton.TabIndex = 5;
        loadTilesButton.Text = "Load Tiles";
        loadTilesButton.UseVisualStyleBackColor = true;
        loadTilesButton.Click += LoadTilesButton_Click;
        // 
        // startButton
        // 
        startButton.Location = new Point(501, 174);
        startButton.Name = "startButton";
        startButton.Size = new Size(75, 23);
        startButton.TabIndex = 6;
        startButton.Text = "Start";
        startButton.UseVisualStyleBackColor = true;
        startButton.Click += StartButton_Click;
        // 
        // pauseButton
        // 
        pauseButton.Location = new Point(582, 174);
        pauseButton.Name = "pauseButton";
        pauseButton.Size = new Size(75, 23);
        pauseButton.TabIndex = 7;
        pauseButton.Text = "Pause";
        pauseButton.UseVisualStyleBackColor = true;
        pauseButton.Click += PauseButton_Click;
        // 
        // resetButton
        // 
        resetButton.Location = new Point(663, 174);
        resetButton.Name = "resetButton";
        resetButton.Size = new Size(75, 23);
        resetButton.TabIndex = 8;
        resetButton.Text = "Reset";
        resetButton.UseVisualStyleBackColor = true;
        resetButton.Click += ResetButton_Click;
        // 
        // backButton
        // 
        backButton.Location = new Point(543, 203);
        backButton.Name = "backButton";
        backButton.Size = new Size(75, 23);
        backButton.TabIndex = 9;
        backButton.Text = "Go Back";
        backButton.UseVisualStyleBackColor = true;
        backButton.Click += BackButton_Click;
        // 
        // forwardButton
        // 
        forwardButton.Location = new Point(624, 203);
        forwardButton.Name = "forwardButton";
        forwardButton.Size = new Size(81, 23);
        forwardButton.TabIndex = 10;
        forwardButton.Text = "Go Forward";
        forwardButton.UseVisualStyleBackColor = true;
        forwardButton.Click += ForwardButton_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(forwardButton);
        Controls.Add(backButton);
        Controls.Add(resetButton);
        Controls.Add(pauseButton);
        Controls.Add(startButton);
        Controls.Add(loadTilesButton);
        Controls.Add(formatLabel);
        Controls.Add(inputTextbox);
        Controls.Add(animationSpeedLabel);
        Controls.Add(animationSpeedTrackbar);
        Controls.Add(visualizer1);
        Name = "Form1";
        Text = "Form1";
        ((System.ComponentModel.ISupportInitialize)animationSpeedTrackbar).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Visualizer visualizer1;
    private TrackBar animationSpeedTrackbar;
    private Label animationSpeedLabel;
    private TextBox inputTextbox;
    private Label formatLabel;
    private Button loadTilesButton;
    private Button startButton;
    private Button pauseButton;
    private Button resetButton;
    private Button backButton;
    private Button forwardButton;
}

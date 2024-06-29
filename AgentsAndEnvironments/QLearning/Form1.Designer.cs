namespace QLearning;

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
        components = new System.ComponentModel.Container();
        visualizer1 = new Visualizer();
        deltaTLabel = new Label();
        label2 = new Label();
        label3 = new Label();
        updateSpeedTrackbar = new TrackBar();
        learningRateTrackbar = new TrackBar();
        rewardDecayTrackbar = new TrackBar();
        costOfLivingTrackbar = new TrackBar();
        costOfLivingLabel = new Label();
        epsilonTrackbar = new TrackBar();
        epsilonLabel = new Label();
        showQValsCheckbox = new CheckBox();
        isRunningCheckbox = new CheckBox();
        autoDecrementCheckbox = new CheckBox();
        skipDrawCheckbox = new CheckBox();
        autoDecrementTimer = new System.Windows.Forms.Timer(components);
        skipRenderTrackbar = new TrackBar();
        ((System.ComponentModel.ISupportInitialize)updateSpeedTrackbar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)learningRateTrackbar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)rewardDecayTrackbar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)costOfLivingTrackbar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)epsilonTrackbar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)skipRenderTrackbar).BeginInit();
        SuspendLayout();
        // 
        // visualizer1
        // 
        visualizer1.Location = new Point(12, 12);
        visualizer1.MouseHoverUpdatesOnly = false;
        visualizer1.Name = "visualizer1";
        visualizer1.Size = new Size(568, 426);
        visualizer1.TabIndex = 0;
        visualizer1.Text = "visualizer1";
        // 
        // deltaTLabel
        // 
        deltaTLabel.AutoSize = true;
        deltaTLabel.Location = new Point(596, 202);
        deltaTLabel.Name = "deltaTLabel";
        deltaTLabel.Size = new Size(80, 15);
        deltaTLabel.TabIndex = 1;
        deltaTLabel.Text = "Update Speed";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(804, 303);
        label2.Name = "label2";
        label2.Size = new Size(81, 15);
        label2.TabIndex = 2;
        label2.Text = "Reward Decay";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(813, 375);
        label3.Name = "label3";
        label3.Size = new Size(79, 15);
        label3.TabIndex = 3;
        label3.Text = "Learning Rate";
        // 
        // updateSpeedTrackbar
        // 
        updateSpeedTrackbar.Location = new Point(586, 220);
        updateSpeedTrackbar.Maximum = 600;
        updateSpeedTrackbar.Name = "updateSpeedTrackbar";
        updateSpeedTrackbar.Size = new Size(202, 45);
        updateSpeedTrackbar.TabIndex = 4;
        updateSpeedTrackbar.Value = 300;
        updateSpeedTrackbar.Scroll += UpdateSpeedTrackbar_Scroll;
        // 
        // learningRateTrackbar
        // 
        learningRateTrackbar.Location = new Point(803, 393);
        learningRateTrackbar.Maximum = 100;
        learningRateTrackbar.Name = "learningRateTrackbar";
        learningRateTrackbar.Size = new Size(202, 45);
        learningRateTrackbar.TabIndex = 5;
        learningRateTrackbar.Value = 50;
        learningRateTrackbar.Scroll += LearningRateTrackbar_Scroll;
        // 
        // rewardDecayTrackbar
        // 
        rewardDecayTrackbar.Location = new Point(794, 321);
        rewardDecayTrackbar.Name = "rewardDecayTrackbar";
        rewardDecayTrackbar.Size = new Size(202, 45);
        rewardDecayTrackbar.TabIndex = 6;
        rewardDecayTrackbar.Value = 5;
        rewardDecayTrackbar.Scroll += RewardDecayTrackbar_Scroll;
        // 
        // costOfLivingTrackbar
        // 
        costOfLivingTrackbar.Location = new Point(586, 321);
        costOfLivingTrackbar.Maximum = 100;
        costOfLivingTrackbar.Name = "costOfLivingTrackbar";
        costOfLivingTrackbar.Size = new Size(202, 45);
        costOfLivingTrackbar.TabIndex = 8;
        costOfLivingTrackbar.Value = 10;
        costOfLivingTrackbar.Scroll += CostOfLivingTrackbar_Scroll;
        // 
        // costOfLivingLabel
        // 
        costOfLivingLabel.AutoSize = true;
        costOfLivingLabel.Location = new Point(596, 303);
        costOfLivingLabel.Name = "costOfLivingLabel";
        costOfLivingLabel.Size = new Size(80, 15);
        costOfLivingLabel.TabIndex = 7;
        costOfLivingLabel.Text = "Cost of Living";
        // 
        // epsilonTrackbar
        // 
        epsilonTrackbar.Location = new Point(586, 393);
        epsilonTrackbar.Maximum = 100;
        epsilonTrackbar.Name = "epsilonTrackbar";
        epsilonTrackbar.Size = new Size(202, 45);
        epsilonTrackbar.TabIndex = 10;
        epsilonTrackbar.Value = 50;
        epsilonTrackbar.Scroll += EpsilionTrackbar_Scroll;
        // 
        // epsilonLabel
        // 
        epsilonLabel.AutoSize = true;
        epsilonLabel.Location = new Point(596, 375);
        epsilonLabel.Name = "epsilonLabel";
        epsilonLabel.Size = new Size(45, 15);
        epsilonLabel.TabIndex = 9;
        epsilonLabel.Text = "Epsilon";
        // 
        // showQValsCheckbox
        // 
        showQValsCheckbox.AutoSize = true;
        showQValsCheckbox.Location = new Point(596, 35);
        showQValsCheckbox.Name = "showQValsCheckbox";
        showQValsCheckbox.Size = new Size(103, 19);
        showQValsCheckbox.TabIndex = 11;
        showQValsCheckbox.Text = "Show Q Values";
        showQValsCheckbox.UseVisualStyleBackColor = true;
        showQValsCheckbox.CheckedChanged += ShowQValsCheckbox_CheckedChanged;
        // 
        // isRunningCheckbox
        // 
        isRunningCheckbox.AutoSize = true;
        isRunningCheckbox.Location = new Point(596, 60);
        isRunningCheckbox.Name = "isRunningCheckbox";
        isRunningCheckbox.Size = new Size(82, 19);
        isRunningCheckbox.TabIndex = 12;
        isRunningCheckbox.Text = "Is Running";
        isRunningCheckbox.UseVisualStyleBackColor = true;
        isRunningCheckbox.CheckedChanged += IsRunningCheckbox_CheckedChanged;
        // 
        // autoDecrementCheckbox
        // 
        autoDecrementCheckbox.AutoSize = true;
        autoDecrementCheckbox.Location = new Point(596, 85);
        autoDecrementCheckbox.Name = "autoDecrementCheckbox";
        autoDecrementCheckbox.Size = new Size(207, 19);
        autoDecrementCheckbox.TabIndex = 13;
        autoDecrementCheckbox.Text = "Auto-Decrement Learning/Epsilon";
        autoDecrementCheckbox.UseVisualStyleBackColor = true;
        autoDecrementCheckbox.CheckedChanged += AutoDecrementCheckbox_CheckedChanged;
        // 
        // skipDrawCheckbox
        // 
        skipDrawCheckbox.AutoSize = true;
        skipDrawCheckbox.Location = new Point(596, 110);
        skipDrawCheckbox.Name = "skipDrawCheckbox";
        skipDrawCheckbox.Size = new Size(105, 19);
        skipDrawCheckbox.TabIndex = 14;
        skipDrawCheckbox.Text = "Skip Rendering";
        skipDrawCheckbox.UseVisualStyleBackColor = true;
        skipDrawCheckbox.CheckedChanged += SkipDrawCheckbox_CheckedChanged;
        // 
        // autoDecrementTimer
        // 
        autoDecrementTimer.Interval = 10;
        autoDecrementTimer.Tick += AutoDecrementTimer_Tick;
        // 
        // skipRenderTrackbar
        // 
        skipRenderTrackbar.Location = new Point(586, 135);
        skipRenderTrackbar.Maximum = 5000;
        skipRenderTrackbar.Name = "skipRenderTrackbar";
        skipRenderTrackbar.Size = new Size(202, 45);
        skipRenderTrackbar.TabIndex = 15;
        skipRenderTrackbar.Value = 100;
        skipRenderTrackbar.Scroll += SkipRenderTrackbar_Scroll;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1017, 450);
        Controls.Add(skipRenderTrackbar);
        Controls.Add(skipDrawCheckbox);
        Controls.Add(autoDecrementCheckbox);
        Controls.Add(isRunningCheckbox);
        Controls.Add(showQValsCheckbox);
        Controls.Add(epsilonTrackbar);
        Controls.Add(epsilonLabel);
        Controls.Add(costOfLivingTrackbar);
        Controls.Add(costOfLivingLabel);
        Controls.Add(rewardDecayTrackbar);
        Controls.Add(learningRateTrackbar);
        Controls.Add(updateSpeedTrackbar);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(deltaTLabel);
        Controls.Add(visualizer1);
        Name = "Form1";
        Text = "Form1";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)updateSpeedTrackbar).EndInit();
        ((System.ComponentModel.ISupportInitialize)learningRateTrackbar).EndInit();
        ((System.ComponentModel.ISupportInitialize)rewardDecayTrackbar).EndInit();
        ((System.ComponentModel.ISupportInitialize)costOfLivingTrackbar).EndInit();
        ((System.ComponentModel.ISupportInitialize)epsilonTrackbar).EndInit();
        ((System.ComponentModel.ISupportInitialize)skipRenderTrackbar).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Visualizer visualizer1;
    private Label deltaTLabel;
    private Label label2;
    private Label label3;
    private TrackBar updateSpeedTrackbar;
    private TrackBar learningRateTrackbar;
    private TrackBar rewardDecayTrackbar;
    private TrackBar costOfLivingTrackbar;
    private Label costOfLivingLabel;
    private TrackBar epsilonTrackbar;
    private Label epsilonLabel;
    private CheckBox showQValsCheckbox;
    private CheckBox isRunningCheckbox;
    private CheckBox autoDecrementCheckbox;
    private CheckBox skipDrawCheckbox;
    private System.Windows.Forms.Timer autoDecrementTimer;
    private TrackBar skipRenderTrackbar;
}

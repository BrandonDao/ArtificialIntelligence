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
        visualizer1 = new Visualizer();
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
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(visualizer1);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion

    private Visualizer visualizer1;
}

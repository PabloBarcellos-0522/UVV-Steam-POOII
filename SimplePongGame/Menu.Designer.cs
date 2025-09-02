namespace SimplePongGame
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Jogar = new Button();
            players_button = new Button();
            SuspendLayout();
            // 
            // Jogar
            // 
            Jogar.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Jogar.Location = new Point(350, 216);
            Jogar.Name = "Jogar";
            Jogar.Size = new Size(243, 61);
            Jogar.TabIndex = 0;
            Jogar.Text = "Jogar Agora!";
            Jogar.UseVisualStyleBackColor = true;
            Jogar.Click += Jogar_Click;
            // 
            // players_button
            // 
            players_button.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            players_button.Location = new Point(350, 283);
            players_button.Name = "players_button";
            players_button.Size = new Size(243, 61);
            players_button.TabIndex = 1;
            players_button.Text = "1 Player";
            players_button.UseVisualStyleBackColor = true;
            players_button.Click += button1_Click;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Ping_Pong_Game_;
            ClientSize = new Size(928, 574);
            Controls.Add(players_button);
            Controls.Add(Jogar);
            Name = "Menu";
            Text = "Menu";
            ResumeLayout(false);
        }

        #endregion

        private Button Jogar;
        private Button players_button;
    }
}
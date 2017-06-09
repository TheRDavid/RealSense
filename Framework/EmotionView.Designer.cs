using System.Drawing;
using System.Windows.Forms;

namespace RealSense
{
    partial class EmotionView
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
            this.components = new System.ComponentModel.Container();
            this.update = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxSadness = new System.Windows.Forms.PictureBox();
            this.pictureBoxFear = new System.Windows.Forms.PictureBox();
            this.pictureBoxContempt = new System.Windows.Forms.PictureBox();
            this.pictureBoxSurprise = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxJoy = new System.Windows.Forms.PictureBox();
            this.pictureBoxAnger = new System.Windows.Forms.PictureBox();
            this.pictureBoxDisgust = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSadness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxContempt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurprise)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJoy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisgust)).BeginInit();
            this.BackColor = Color.White;
            this.SuspendLayout();
            // 
            // update
            // 
            this.update.Enabled = true;
            this.update.Interval = 3000;
            this.update.Tick += new System.EventHandler(this.updateTime);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            //this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(512, 321);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxSadness, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxFear, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxContempt, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxSurprise, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 163);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(506, 155);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // pictureBoxSadness
            // 
            this.pictureBoxSadness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSadness.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxSadness.Name = "pictureBoxSadness";
            this.pictureBoxSadness.Size = new System.Drawing.Size(120, 149);
            this.pictureBoxSadness.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSadness.TabIndex = 0;
            this.pictureBoxSadness.TabStop = false;
            this.pictureBoxSadness.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string text = "Sadness";
                Font font = new Font("Arial", 18);
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = pictureBoxFear.Width / 2 - textSize.Width / 2;
                locationToDraw.Y = (pictureBoxSadness.Height - 5) - (textSize.Height);

                e.Graphics.DrawString(text, font, Brushes.White, locationToDraw);
            });
            // 
            // pictureBoxFear
            // 
            this.pictureBoxFear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxFear.Location = new System.Drawing.Point(129, 3);
            this.pictureBoxFear.Name = "pictureBoxFear";
            this.pictureBoxFear.Size = new System.Drawing.Size(120, 149);
            this.pictureBoxFear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFear.TabIndex = 1;
            this.pictureBoxFear.TabStop = false;
            this.pictureBoxFear.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string text = "Fear";
                Font font = new Font("Arial", 18);
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = pictureBoxFear.Width/2- textSize.Width/2;
                locationToDraw.Y = (pictureBoxFear.Height - 5) - (textSize.Height);

                e.Graphics.DrawString(text, font, Brushes.White, locationToDraw);
            });
            // 
            // pictureBoxContempt
            // 
            this.pictureBoxContempt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxContempt.Location = new System.Drawing.Point(255, 3);
            this.pictureBoxContempt.Name = "pictureBoxContempt";
            this.pictureBoxContempt.Size = new System.Drawing.Size(120, 149);
            this.pictureBoxContempt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxContempt.TabIndex = 2;
            this.pictureBoxContempt.TabStop = false;
            this.pictureBoxContempt.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string text = "Contempt";
                Font font = new Font("Arial", 18);
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = pictureBoxFear.Width / 2 - textSize.Width / 2;
                locationToDraw.Y = (pictureBoxContempt.Height - 5) - (textSize.Height);

                e.Graphics.DrawString(text, font, Brushes.White, locationToDraw);
            });
            // 
            // pictureBoxSurprise
            // 
            this.pictureBoxSurprise.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSurprise.Location = new System.Drawing.Point(381, 3);
            this.pictureBoxSurprise.Name = "pictureBoxSurprise";
            this.pictureBoxSurprise.Size = new System.Drawing.Size(122, 149);
            this.pictureBoxSurprise.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSurprise.TabIndex = 3;
            this.pictureBoxSurprise.TabStop = false;
            this.pictureBoxSurprise.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string text = "Surprise";
                Font font = new Font("Arial", 18);
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = pictureBoxFear.Width / 2 - textSize.Width / 2;
                locationToDraw.Y = (pictureBoxSurprise.Height - 5) - (textSize.Height);

                e.Graphics.DrawString(text, font, Brushes.White, locationToDraw);
            });
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.pictureBoxJoy, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.pictureBoxAnger, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pictureBoxDisgust, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(506, 154);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // pictureBoxJoy
            // 
            this.pictureBoxJoy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxJoy.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxJoy.Name = "pictureBoxJoy";
            this.pictureBoxJoy.Size = new System.Drawing.Size(162, 148);
            this.pictureBoxJoy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxJoy.TabIndex = 0;
            this.pictureBoxJoy.TabStop = false;
            this.pictureBoxJoy.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string text = "Joy";
                Font font = new Font("Arial", 18);
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = pictureBoxFear.Width / 2 - textSize.Width / 2;
                locationToDraw.Y = (pictureBoxJoy.Height-5) - (textSize.Height);

                e.Graphics.DrawString(text, font, Brushes.White, locationToDraw);
            });
            // 
            // pictureBoxAnger
            // 
            this.pictureBoxAnger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxAnger.Location = new System.Drawing.Point(171, 3);
            this.pictureBoxAnger.Name = "pictureBoxAnger";
            this.pictureBoxAnger.Size = new System.Drawing.Size(162, 148);
            this.pictureBoxAnger.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxAnger.TabIndex = 1;
            this.pictureBoxAnger.TabStop = false;
            this.pictureBoxAnger.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string text = "Anger";
                Font font = new Font("Arial", 18);
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = pictureBoxFear.Width / 2 - textSize.Width / 2;
                locationToDraw.Y = (pictureBoxAnger.Height - 5) - (textSize.Height);

                e.Graphics.DrawString(text, font, Brushes.White, locationToDraw);
            });
            // 
            // pictureBoxDisgust
            // 
            this.pictureBoxDisgust.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDisgust.Location = new System.Drawing.Point(339, 3);
            this.pictureBoxDisgust.Name = "pictureBoxDisgust";
            this.pictureBoxDisgust.Size = new System.Drawing.Size(164, 148);
            this.pictureBoxDisgust.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxDisgust.TabIndex = 2;
            this.pictureBoxDisgust.TabStop = false;
            this.pictureBoxDisgust.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string text = "Disgust";
                Font font = new Font("Arial", 18);
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = pictureBoxFear.Width / 2 - textSize.Width / 2;
                locationToDraw.Y = (pictureBoxDisgust.Height - 5) - (textSize.Height);

                e.Graphics.DrawString(text, font, Brushes.White, locationToDraw);
            });
            // 
            // EmotionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 321);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EmotionView";
            this.Text = "EmotionView";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSadness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxContempt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurprise)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJoy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisgust)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer update;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBoxSadness;
        private System.Windows.Forms.PictureBox pictureBoxFear;
        private System.Windows.Forms.PictureBox pictureBoxContempt;
        private System.Windows.Forms.PictureBox pictureBoxSurprise;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.PictureBox pictureBoxJoy;
        private System.Windows.Forms.PictureBox pictureBoxAnger;
        private System.Windows.Forms.PictureBox pictureBoxDisgust;
        
        
    }
}
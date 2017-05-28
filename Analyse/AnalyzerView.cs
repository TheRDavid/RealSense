using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Vlc.DotNet.Forms;
using System.Threading;
using System.Drawing.Text;

namespace RealSense
{
    // AVERAGE OF ALL ON THE TOP ROW
    class AnalyzerView : Form
    {
        private MenuStrip menuStrip1;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem angARRToolStripMenuItem;
        private ToolStripMenuItem fearToolStripMenuItem;
        private ToolStripMenuItem contemptToolStripMenuItem;
        private ToolStripMenuItem surpriseToolStripMenuItem;
        private ToolStripMenuItem joyToolStripMenuItem;
        private ToolStripMenuItem sadnessToolStripMenuItem;
        private ToolStripMenuItem disgustToolStripMenuItem;
        private FlowLayoutPanel elementPanel = new FlowLayoutPanel();

        private static double videoRatio = 900.0 / 1080.0;
        private static int VIEW_TINY = 100, VIEW_SMALL = 200, VIEW_DEFAULT = 300, VIEW_LARGE = 500;
        private ToolStripMenuItem viewSizeToolStripMenuItem;
        private ToolStripMenuItem tinyToolStripMenuItem;
        private ToolStripMenuItem smallToolStripMenuItem;
        private ToolStripMenuItem deafultToolStripMenuItem;
        private ToolStripMenuItem largeToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private static int viewHeight = VIEW_DEFAULT;
        private static int WINDOW_WIDTH;
        private static int gap = 20;
        private Thread updaterThread;
        private static int currentFrame = 0;

        private TrackBar scrubber = new TrackBar();
        private Button playButton = new Button();
        private Panel controlPanel = new Panel();

        private static Brush textBrush = new SolidBrush(Color.FromArgb(255, 88, 88, 88));
        private static Font textFont = new Font("Arial", 11);

        private static int cellWidth = 790;
        private bool loading = false;

        public AnalyzerView()
        {
            MinimumSize = new Size(cellWidth, 300);
            InitializeComponent();
            elementPanel.FlowDirection = FlowDirection.TopDown;
            elementPanel.AutoScroll = true;
            elementPanel.WrapContents = false;
            elementPanel.Bounds = new Rectangle(0, menuStrip1.Height, Width, Height - 100);
            controlPanel.Bounds = new Rectangle(0, menuStrip1.Height + elementPanel.Height, Width, 50);
            Controls.Add(elementPanel);
            Controls.Add(controlPanel);
            this.scrubber.Value = 0;
            this.scrubber.TickFrequency = 1;
            this.scrubber.Maximum = 299;
            this.scrubber.TickStyle = TickStyle.None;
            this.scrubber.Scroll += (sender, e) =>
            {
                currentFrame = scrubber.Value;

                foreach (Control c in elementPanel.Controls)
                {
                    if (c.GetType() == typeof(DataSetView))
                    {
                        DataSetView dsv = (DataSetView)c;
                        if (dsv.vlcControl.IsPlaying)
                            dsv.vlcControl.Pause();
                        dsv.vlcControl.Position = (float)currentFrame / 300f;
                    }
                }
            };
            this.playButton.Text = "Play/Pause";
            this.playButton.Bounds = new Rectangle(30, 10, 80, 30);
            this.controlPanel.Controls.Add(playButton);
            this.controlPanel.Controls.Add(scrubber);

            arrange();

            this.Show();

            updaterThread = new Thread(loadFrame);
            updaterThread.Start();
        }

        private void loadFrame()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (!loading) // prevent interference with loading Files!
                {
                    foreach (Control c in elementPanel.Controls)
                    {
                        if (c.GetType() == typeof(DataSetView))
                        {
                            DataSetView dsv = (DataSetView)c;
                            dsv.udpateAndVisualizeData();
                        }
                    }
                }
            }
        }

        private void loadFiles(string type)
        {
            loading = true;
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Recordings\\";
            string[] files = Directory.GetFiles(dir);
            List<string> filesOfInterest = new List<string>();
            foreach (string f in files)
            {
                if (f.EndsWith(type))
                {
                    filesOfInterest.Add(f.Substring(0, f.Length - type.Length - 1));
                }
            }
            elementPanel.Controls.Clear();
            int index = 0;
            foreach (string s in filesOfInterest)
            {
                DataSetView dsv = new DataSetView(s, type, index);
                elementPanel.Controls.Add(dsv);
                index++;
            }

            arrange();
            loading = false;
        }

        private void arrange()
        {
            WINDOW_WIDTH = Width;
            elementPanel.Bounds = new Rectangle(0, menuStrip1.Height, Width, Height - 100);
            controlPanel.Bounds = new Rectangle(0, menuStrip1.Height + elementPanel.Height, Width, 50);
            this.scrubber.Bounds = new Rectangle(130, 10, WINDOW_WIDTH - 150, 40);

            foreach (Control c in elementPanel.Controls)
            {
                ((DataSetView)c).arrange();
            }
            Text = Size.ToString();
        }

        private class DataSetView : Panel
        {

            public FaceRecording faceRecording;
            public Vlc.DotNet.Forms.VlcControl vlcControl;
            public PictureBox dataPictureBox = new PictureBox();
            string dataFile, videoFile, shortName;
            int index;
            Model model;
            Bitmap dataImage;

            private RSModule[] microExpressionModules = new RSModule[12];
            private RSModule[] emotionModules = new RSModule[7];


            FriggnAweseomeGraphix.MEMonitor[] monitors = new FriggnAweseomeGraphix.MEMonitor[] {
                new FriggnAweseomeGraphix.MEMonitor("","",0,0,25,5),    // anger
                new FriggnAweseomeGraphix.MEMonitor("","",0,55,25,5),   // joy
                new FriggnAweseomeGraphix.MEMonitor("","",0,110,25,5),  // fear
                new FriggnAweseomeGraphix.MEMonitor("","",0,165,25,5),  // contempt
                new FriggnAweseomeGraphix.MEMonitor("","",55,25,25,5),  // sadness
                new FriggnAweseomeGraphix.MEMonitor("","",55,80,25,5),  // disgust
                new FriggnAweseomeGraphix.MEMonitor("","",55,135,25,5)  // surprise
            };

            MethodInvoker pictureUpdate;

            public DataSetView(string fileBaseName, string type, int idx)
            {
                initModules();
                index = idx;
                videoFile = fileBaseName + ".mp4";
                dataFile = fileBaseName + "." + type;
                shortName = fileBaseName.Substring(fileBaseName.LastIndexOf("\\"));
                faceRecording = FaceRecording.load(dataFile);

                vlcControl = new Vlc.DotNet.Forms.VlcControl();
                vlcControl.VlcLibDirectoryNeeded += new EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.OnVlcControlNeedLibDirectory);
                ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
                vlcControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
                vlcControl.Location = new System.Drawing.Point(0, 0);
                arrange();
                vlcControl.VlcMediaplayerOptions = null;

                dataImage = new Bitmap(dataPictureBox.Bounds.Width, dataPictureBox.Bounds.Height);
                dataPictureBox.Image = dataImage;

                pictureUpdate = delegate
                {
                    // Innerhalb dieses Blocks können Controls
                    // und Forms angesprochen werden, neue Fenster
                    // erzeugt werden etc....
                    dataPictureBox.Image = dataImage;
                    dataPictureBox.Refresh();
                };

                Controls.Add(vlcControl);
                Controls.Add(dataPictureBox);
                vlcControl.SetMedia(new Uri(videoFile), null);
                vlcControl.Play();
                vlcControl.Pause();

                model.NullFace = faceRecording.getNullFace();

                arrange();
            }

            private void initModules()
            {
                model = new RealSense.Model(false);
                RSModule.Init(model);
                microExpressionModules[0] = new ME_BearTeeth();
                microExpressionModules[1] = new ME_BrowShift();
                microExpressionModules[2] = new ME_EyelidTight();
                microExpressionModules[3] = new ME_JawDrop();
                microExpressionModules[4] = new ME_LipCorner();
                microExpressionModules[5] = new ME_LipLine();
                microExpressionModules[6] = new ME_LipsTightened();
                microExpressionModules[7] = new ME_LipStretched();
                microExpressionModules[8] = new ME_LowerLipLowered();
                microExpressionModules[9] = new ME_LowerLipRaised();
                microExpressionModules[10] = new ME_NoseWrinkled();
                microExpressionModules[11] = new ME_UpperLipRaised();

                foreach (RSModule m in microExpressionModules)
                    m.Debug = false;

                emotionModules[0] = new EM_Anger();
                emotionModules[1] = new EM_Joy();
                emotionModules[2] = new EM_Fear();
                emotionModules[3] = new EM_Contempt();
                emotionModules[4] = new EM_Sadness();
                emotionModules[5] = new EM_Disgust();
                emotionModules[6] = new EM_Surprise();

                foreach (RSModule m in emotionModules)
                    m.Debug = false;
            }

            public void udpateAndVisualizeData()
            {
                if (vlcControl.IsPlaying) vlcControl.Pause();
                model.CurrentFace = faceRecording.getFace(currentFrame);
                model.NullFace = faceRecording.getNullFace();
                RSModule.Init(model);

                foreach (RSModule rsm in microExpressionModules)
                {
                    rsm.Work(null);
                }

                foreach (RSModule rsm in emotionModules)
                {
                    rsm.Work(null);
                }

                monitors[0].currentValue = (int)model.Emotions["Anger"];
                monitors[1].currentValue = (int)model.Emotions["Joy"];
                monitors[2].currentValue = (int)model.Emotions["Fear"];
                monitors[3].currentValue = (int)model.Emotions["Contempt"];
                monitors[4].currentValue = (int)model.Emotions["Sadness"];
                monitors[5].currentValue = (int)model.Emotions["Disgust"];
                monitors[6].currentValue = (int)model.Emotions["Surprise"];

                //Bitmap newImage = new Bitmap(Width, viewHeight);
                Graphics g = Graphics.FromImage(dataImage);
                g.Clear(System.Drawing.SystemColors.MenuBar);
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                foreach (FriggnAweseomeGraphix.MEMonitor monitor in monitors)
                {
                    FriggnAweseomeGraphix.drawMEMontior(g, monitor, false);
                }

                int yPos = (int)(gap * 1.5), yPos2 = yPos;
                int idx = 0;
                if (viewHeight != VIEW_TINY)
                    foreach (KeyValuePair<string, double> entry in model.AU_Values)
                    {
                        int xBase, yBase;
                        if (idx++ > 7 && viewHeight != VIEW_LARGE)
                        {
                            yBase = yPos2;
                            xBase = monitors[0].radius * 4 + gap + 420;
                            yPos2 += gap;
                        }
                        else
                        {
                            yBase = yPos;
                            yPos += gap;
                            xBase = monitors[0].radius * 4 + gap + 100;
                        }

                        g.DrawString(entry.Key.Substring(entry.Key.IndexOf(".") + 1), textFont, textBrush, xBase, yBase - 5);
                        g.DrawString((int)entry.Value + "", textFont, textBrush, xBase + 250, yBase - 5);
                    }

                g.DrawString(shortName, textFont, textBrush, monitors[0].radius * 8 + gap, 0);
                g.DrawString("Frame: " + currentFrame, textFont, textBrush, monitors[0].radius * 8 + gap + 250, 0);
                g.DrawLine(new Pen(textBrush), 0, Height - 1, Width, Height - 1);
                Invoke(pictureUpdate);
            }

            public void arrange()
            {
                int y = gap + index * viewHeight;
                vlcControl.Size = new System.Drawing.Size((int)(viewHeight * videoRatio), viewHeight);
                vlcControl.Location = new Point(WINDOW_WIDTH - (int)(viewHeight * videoRatio) - 50, 0);
                Bounds = new Rectangle(0, y, WINDOW_WIDTH, viewHeight);
                dataPictureBox.Bounds = new Rectangle(0, 0, WINDOW_WIDTH - vlcControl.Width, viewHeight);
                dataImage = new Bitmap(dataPictureBox.Width, dataPictureBox.Height);
                // update Monitors
                if (viewHeight != VIEW_TINY)
                {
                    int radius = (viewHeight - 5 * gap - 4 * monitors[0].thickness) / 4 / 2;

                    monitors[0].x = gap;
                    monitors[0].y = gap;

                    monitors[1].x = gap;
                    monitors[1].y = gap * 2 + radius * 2 + monitors[0].thickness;

                    monitors[2].x = gap;
                    monitors[2].y = gap * 3 + 2 * radius * 2 + monitors[0].thickness * 2;

                    monitors[3].x = gap;
                    monitors[3].y = gap * 4 + 3 * radius * 2 + monitors[0].thickness * 3;

                    monitors[4].x = 2 * gap + radius * 2;
                    monitors[4].y = gap + radius;

                    monitors[5].x = 2 * gap + radius * 2; ;
                    monitors[5].y = gap * 2 + radius + 2 * radius + monitors[0].thickness;

                    monitors[6].x = 2 * gap + radius * 2;
                    monitors[6].y = gap * 3 + radius + 4 * radius + monitors[0].thickness * 2;

                    for (int i = 0; i < monitors.Length; i++)
                    {
                        monitors[i].radius = radius;
                        monitors[i].thickness = radius / 2;
                    }
                }
                else
                {
                    int radius = (viewHeight - 2 * gap) / 2;
                    for (int i = 0; i < monitors.Length; i++)
                    {
                        monitors[i].x = gap + i * gap + i * radius * 2;
                        monitors[i].y = gap;
                        monitors[i].radius = radius;
                        monitors[i].thickness = radius / 2;
                    }
                }
            }

            public void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
            {
                e.VlcLibDirectory = new DirectoryInfo("C:\\Users\\prouser\\Documents\\Vlc.DotNet-master\\Vlc.DotNet-master\\lib\\x86\\");
            }

        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.angARRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contemptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surpriseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sadnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disgustToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tinyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deafultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.viewSizeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.angARRToolStripMenuItem,
            this.fearToolStripMenuItem,
            this.contemptToolStripMenuItem,
            this.surpriseToolStripMenuItem,
            this.joyToolStripMenuItem,
            this.sadnessToolStripMenuItem,
            this.disgustToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // angARRToolStripMenuItem
            // 
            this.angARRToolStripMenuItem.Name = "angARRToolStripMenuItem";
            this.angARRToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.angARRToolStripMenuItem.Text = "AngARR";
            this.angARRToolStripMenuItem.Click += new System.EventHandler(this.angARRToolStripMenuItem_Click);
            // 
            // fearToolStripMenuItem
            // 
            this.fearToolStripMenuItem.Name = "fearToolStripMenuItem";
            this.fearToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.fearToolStripMenuItem.Text = "Fear";
            this.fearToolStripMenuItem.Click += new System.EventHandler(this.fearToolStripMenuItem_Click);
            // 
            // contemptToolStripMenuItem
            // 
            this.contemptToolStripMenuItem.Name = "contemptToolStripMenuItem";
            this.contemptToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.contemptToolStripMenuItem.Text = "Contempt";
            this.contemptToolStripMenuItem.Click += new System.EventHandler(this.contemptToolStripMenuItem_Click);
            // 
            // surpriseToolStripMenuItem
            // 
            this.surpriseToolStripMenuItem.Name = "surpriseToolStripMenuItem";
            this.surpriseToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.surpriseToolStripMenuItem.Text = "Surprise";
            this.surpriseToolStripMenuItem.Click += new System.EventHandler(this.surpriseToolStripMenuItem_Click);
            // 
            // joyToolStripMenuItem
            // 
            this.joyToolStripMenuItem.Name = "joyToolStripMenuItem";
            this.joyToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.joyToolStripMenuItem.Text = "Joy";
            this.joyToolStripMenuItem.Click += new System.EventHandler(this.joyToolStripMenuItem_Click);
            // 
            // sadnessToolStripMenuItem
            // 
            this.sadnessToolStripMenuItem.Name = "sadnessToolStripMenuItem";
            this.sadnessToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.sadnessToolStripMenuItem.Text = "Sadness";
            this.sadnessToolStripMenuItem.Click += new System.EventHandler(this.sadnessToolStripMenuItem_Click);
            // 
            // disgustToolStripMenuItem
            // 
            this.disgustToolStripMenuItem.Name = "disgustToolStripMenuItem";
            this.disgustToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.disgustToolStripMenuItem.Text = "Disgust";
            this.disgustToolStripMenuItem.Click += new System.EventHandler(this.disgustToolStripMenuItem_Click);
            // 
            // viewSizeToolStripMenuItem
            // 
            this.viewSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tinyToolStripMenuItem,
            this.smallToolStripMenuItem,
            this.deafultToolStripMenuItem,
            this.largeToolStripMenuItem});
            this.viewSizeToolStripMenuItem.Name = "viewSizeToolStripMenuItem";
            this.viewSizeToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.viewSizeToolStripMenuItem.Text = "View Size";
            // 
            // tinyToolStripMenuItem
            // 
            this.tinyToolStripMenuItem.Name = "tinyToolStripMenuItem";
            this.tinyToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.tinyToolStripMenuItem.Text = "Tiny";
            this.tinyToolStripMenuItem.Click += new System.EventHandler(this.tinyToolStripMenuItem_Click);
            // 
            // smallToolStripMenuItem
            // 
            this.smallToolStripMenuItem.Name = "smallToolStripMenuItem";
            this.smallToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.smallToolStripMenuItem.Text = "Small";
            this.smallToolStripMenuItem.Click += new System.EventHandler(this.smallToolStripMenuItem_Click);
            // 
            // deafultToolStripMenuItem
            // 
            this.deafultToolStripMenuItem.Name = "deafultToolStripMenuItem";
            this.deafultToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.deafultToolStripMenuItem.Text = "Deafult";
            this.deafultToolStripMenuItem.Click += new System.EventHandler(this.deafultToolStripMenuItem_Click);
            // 
            // largeToolStripMenuItem
            // 
            this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            this.largeToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.largeToolStripMenuItem.Text = "Large";
            this.largeToolStripMenuItem.Click += new System.EventHandler(this.largeToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // AnalyzerView
            // 
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AnalyzerView";
            this.Text = "Analyzer";
            this.ResizeEnd += new System.EventHandler(this.AnalyzerView_ResizeEnd);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private void angARRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFiles("anger");
        }

        private void fearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFiles("fear");
        }

        private void contemptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFiles("contempt");
        }

        private void surpriseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFiles("surprise");
        }

        private void joyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFiles("joy");
        }

        private void sadnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFiles("sadness");
        }

        private void disgustToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFiles("disgust");
        }

        private void tinyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewHeight = VIEW_TINY;
            arrange();
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewHeight = VIEW_SMALL;
            arrange();
        }

        private void AnalyzerView_ResizeEnd(object sender, EventArgs e)
        {
            arrange();
        }

        private void deafultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewHeight = VIEW_DEFAULT;
            arrange();
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewHeight = VIEW_LARGE;
            arrange();
        }
    }
}

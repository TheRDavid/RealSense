using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RealSense
{
    public partial class EmotionView : Form
    {
        private Model model;

        public EmotionView(Model model)
        {
            this.model = model;
            InitializeComponent();
            model.EmotionPictureBoxes = new Dictionary<Model.Emotion, PictureBox>();

            model.EmotionPictureBoxes[Model.Emotion.ANGER] = pictureBoxAnger;
            model.EmotionPictureBoxes[Model.Emotion.CONTEMPT] = pictureBoxContempt;
            model.EmotionPictureBoxes[Model.Emotion.DISGUST] = pictureBoxDisgust;
            model.EmotionPictureBoxes[Model.Emotion.FEAR] = pictureBoxFear;
            model.EmotionPictureBoxes[Model.Emotion.JOY] = pictureBoxJoy;
            model.EmotionPictureBoxes[Model.Emotion.SADNESS] = pictureBoxSadness;
            model.EmotionPictureBoxes[Model.Emotion.SURPRISE] = pictureBoxSurprise;
        }

        private void updateTime(object sender, EventArgs e)
        {
            foreach (Model.Emotion emotion in Enum.GetValues(typeof(Model.Emotion)))
            {
                model.EmotionPictureBoxes[emotion].Image = model.EmotionBitmaps[emotion];
            }
            


        }
    }
}

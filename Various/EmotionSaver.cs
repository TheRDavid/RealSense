using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace RealSense
{
    class EmotionSaver : RSModule
    {
        int x, y, width, height;
        int counter = 30, counter2 = 120;
        bool saverInit = false;
        Bitmap source, croppedImage;

        public void init()
        {
            saverInit = true;
            model.EmotionBitmaps = model.ExcampleBitmaps;
            foreach (Model.Emotion emotion in Enum.GetValues(typeof(Model.Emotion)))
            {
                model.EmotionMax[emotion] = 30;
            }
        }

        public override void Work(Graphics g)
        {
            if (!saverInit) init();
            if (counter2-- > 0) return;
            if (--counter ==0)
            {
                counter = 30;
                if (model.CurrentPoseDiff < model.PoseMax && model.calibrationProgress == 100)
                {
                    source = model.ColorBitmap;
                    x = source.Width / 2 + 50;
                    y = 0;
                    width = source.Width / 2 - 50;
                    height = source.Height;
                    croppedImage = (Bitmap)source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);

                    foreach (Model.Emotion emotion in Enum.GetValues(typeof(Model.Emotion)))
                    {
                        if (model.Emotions[emotion] > model.EmotionMax[emotion])
                        {
                            model.EmotionMax[emotion] = model.Emotions[emotion];
                            model.EmotionBitmaps[emotion] = croppedImage;

                        }
                    }
                    /**
                    if (model.Emotions["Contempt02"] > contemptMax)
                    {
                        angerMax = model.Emotions["Contempt02"];
                        File.Delete("C:/Users/prouser/Pictures/emotions/contempt.png");
                        croppedImage.Save("C:/Users/prouser/Pictures/emotions/contempt.png");

                    }

                    if (model.Emotions["Disgust"] > disgustMax)
                    {
                        angerMax = model.Emotions["Disgust"];
                        File.Delete("C:/Users/prouser/Pictures/emotions/disgust.png");
                        croppedImage.Save("C:/Users/prouser/Pictures/emotions/disgust.png");

                    }

                    if (model.Emotions["Fear"] > fearMax)
                    {
                        angerMax = model.Emotions["Fear"];
                        File.Delete("C:/Users/prouser/Pictures/emotions/fear.png");
                        croppedImage.Save("C:/Users/prouser/Pictures/emotions/fear.png");

                    }

                    if (model.Emotions["Joy"] > joyMax)
                    {
                        angerMax = model.Emotions["Joy"];
                        File.Delete("C:/Users/prouser/Pictures/emotions/joy.png");
                        croppedImage.Save("C:/Users/prouser/Pictures/emotions/joy.png");

                    }

                    if (model.Emotions["Sadness"] > sadnessMax)
                    {
                        angerMax = model.Emotions["Sadness"];
                        File.Delete("C:/Users/prouser/Pictures/emotions/sadness.png");
                        croppedImage.Save("C:/Users/prouser/Pictures/emotions/sadness.png");

                    }

                    if (model.Emotions["Surprise"] > surpriseMax)
                    {
                        angerMax = model.Emotions["Surprise"];
                        File.Delete("C:/Users/prouser/Pictures/emotions/surprise.png");
                        croppedImage.Save("C:/Users/prouser/Pictures/emotions/surprise.png");

                    }
    */
                }
            }
           
            }
        }
    }


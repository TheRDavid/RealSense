using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealSense
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FriggnAweseomeGraphix
    {
        /**
         * ##########
         * #   UI   #
         * ##########
         */
        static Pen ME_MonitorPen;
        static Brush MEMonitorBrush;

        static Font percentageFont = new Font("Futura", 24, FontStyle.Bold);
        public static Font majorFont = new Font("Futura", 36, FontStyle.Bold);
        public static Font minorFont = new Font("Futura", 21, FontStyle.Regular);
        public static Color fgColor = Color.FromArgb(55, 186, 68, 75);
        static Color bgColor = Color.FromArgb(255, 200, 200, 200);
        public static Color fontColor = Color.FromArgb(255, 103, 103, 104);

        public class MEMonitor
        {
            public int x;
            public int y;
            public int radius;
            public int thickness;
            public int currentValue = 0;
            public int targetValue = 0;
            public String majorText;
            public String minorText;
            public bool showPercent = true;

            public MEMonitor(String majText, String minText, int xP, int yP, int rad, int thick)
            {
                x = xP;
                y = yP;
                radius = rad;
                thickness = thick;
                majorText = majText;
                minorText = minText;
            }

            public void step()
            {
                currentValue += (targetValue - currentValue) / 10;
            }
        }

        public static void drawMEMontior(Graphics gfx, MEMonitor monitor)
        {
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (monitor.currentValue < 0) monitor.currentValue = 0;
            if (monitor.currentValue > 100) monitor.currentValue = 100;
            String text = monitor.currentValue + "%";
            SizeF size = gfx.MeasureString(text, percentageFont);
            MEMonitorBrush = new SolidBrush(bgColor);
            ME_MonitorPen = new Pen(MEMonitorBrush);
            ME_MonitorPen.Width = monitor.thickness;
            Rectangle area = new Rectangle(monitor.x, monitor.y, monitor.radius * 2, monitor.radius * 2);
            gfx.DrawEllipse(ME_MonitorPen, area);
            ME_MonitorPen.Brush = new SolidBrush(Color.FromArgb(fgColor.A + 2 * monitor.currentValue, fgColor.R, fgColor.G, fgColor.B));
            gfx.DrawArc(ME_MonitorPen, area, -90, (int)(360.0 / 100 * monitor.currentValue));
            MEMonitorBrush = new SolidBrush(fontColor);
            if(monitor.showPercent) gfx.DrawString(text, percentageFont, MEMonitorBrush, (int)(monitor.x + monitor.radius - size.Width / 2), monitor.y + monitor.radius - size.Height / 2);
            ME_MonitorPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            ME_MonitorPen.Width = monitor.thickness / 4;
           // if (monitor.currentValue >= 100)
            //    ME_MonitorPen.Brush = new SolidBrush(fgColor);
           // else ME_MonitorPen.Brush = new SolidBrush(bgColor);
          //  gfx.DrawEllipse(ME_MonitorPen, new Rectangle(monitor.x + monitor.thickness, monitor.y + monitor.thickness, (monitor.radius - monitor.thickness) * 2, (monitor.radius - monitor.thickness) * 2));
            MEMonitorBrush = new SolidBrush(fontColor);
            gfx.DrawString(monitor.majorText, majorFont, MEMonitorBrush, monitor.x + monitor.radius * 2 + 40, monitor.y + monitor.radius - 50);
            gfx.DrawString(monitor.minorText, minorFont, MEMonitorBrush, monitor.x + monitor.radius * 2 + 40 + 5, monitor.y + monitor.radius + 10);
        }


        /**
         * ############
         * # Graphics #
         * ############
         */

        /**
         * old and slow, but pretty version
         */
        public static unsafe void pretty_blur(Bitmap source, Bitmap target, int x0, int y0, int x1, int y1, int factor, BitmapData sourceData, BitmapData targetData)
        {
            int blurLength = 1 + 2 * factor;
            int blurLengthBit = blurLength * 4;
            int blurLengthBitBy2 = blurLengthBit / 2;
            int blurLengthBy2 = blurLength / 2;
            int factorSq = blurLength * blurLength;
            int owidthTimesSize = 4 * source.Width;
            int twidthTimesSize = 4 * target.Width;
            int numPixels = source.Width * source.Height;
            byte* sourcePointer = (byte*)sourceData.Scan0;
            byte* targetPointer = (byte*)targetData.Scan0;

            byte[] sorroundingPixels = new byte[factorSq * 4];


            for (int yy = y0; yy < y1; yy++)
            {
                int idxByRow = yy * owidthTimesSize;
                for (int xx = x0; xx < x1; xx++)
                {
                    int oindex = pixelIndex(xx, yy, owidthTimesSize, 4);
                    int tindex = pixelIndex(xx - x0, yy - y0, twidthTimesSize, 3);
                    byte* mainIndex = targetPointer + tindex;
                    byte* originalIndex = sourcePointer + oindex;



                    for (int by = 0; by < blurLength; by++)
                    {
                        int yIdx = by * blurLengthBit;
                        for (int bx = 0; bx < blurLengthBit; bx += 4)
                        {
                            int yIdxpbx = yIdx + bx;
                            byte* areaIndex = sourcePointer + pixelIndex(xx - blurLengthBitBy2 + bx, yy - blurLengthBy2 + by, owidthTimesSize, 4);

                            sorroundingPixels[yIdxpbx] = *(areaIndex);
                            sorroundingPixels[yIdxpbx + 1] = *(areaIndex + 1);
                            sorroundingPixels[yIdxpbx + 2] = *(areaIndex + 2);
                        }
                    }


                    double r = 0, g = 0, b = 0, a = 0;
                    for (int sy = 0; sy < blurLength; sy++)
                    {
                        int yIdx = sy * blurLengthBit;
                        for (int sx = 0; sx < blurLength; sx++)
                        {

                            int yxIdx = yIdx + sx * 4;
                            b += (sorroundingPixels[yxIdx]);
                            g += (sorroundingPixels[yxIdx + 1]);
                            r += (sorroundingPixels[yxIdx + 2]);
                        }
                    }
                    *(mainIndex) = (byte)(b / factorSq);
                    *(mainIndex + 1) = (byte)(g / factorSq);
                    *(mainIndex + 2) = (byte)(r / factorSq);
                    *(mainIndex + 3) = (byte)(a / factorSq);
                }
            }
        }

        /**
         * Revised version, a lot faster, but sacrifices prettyness (also uuuuugly code)
         */
        public static unsafe void sonic_blur(Bitmap source, Bitmap target,
                                        int x0, int y0, int x1, int y1, int factor, int channels,
                                        BitmapData sourceData, BitmapData targetData)
        {
            int blurLength = 1 + 2 * factor;                                            // take calculations as far 'upstairs' in the loops as possible,
            int blurLengthBit = blurLength * channels;                                  // refrain from multiplying and dividing
            int blurLengthBitBy2 = blurLengthBit / 2;
            int blurLengthBy2 = blurLength / 2;
            int owidthTimesSize = channels * source.Width;
            int twidthTimesSize = channels * target.Width;
            byte* sourcePointer = (byte*)sourceData.Scan0;
            byte* targetPointer = (byte*)targetData.Scan0;
            int blurLength_t_owts = blurLength * owidthTimesSize;
            int c0, c1, c2;                                                             // color channels (ignore alpha)
            int shift = blurLength;                                                     // if it's bad, but works, it's still bad (but it works)

            for (int yy = y0, ycalc = 0, ycalc2 = y0 * owidthTimesSize; yy < y1; yy++, ycalc += twidthTimesSize, ycalc2 += owidthTimesSize)
            {
                int yy_m_blb2 = (yy - blurLengthBy2) * owidthTimesSize;
                int bymx = blurLength_t_owts + yy_m_blb2;
                for (int xx = x0; xx < x1; xx++)
                {
                    byte* mainIndex = targetPointer + (xx - x0) * channels + ycalc;
                    int xx_m_blbb2 = xx - blurLengthBitBy2;
                    int bxmx = blurLengthBit + xx_m_blbb2;
                    c0 = 0; c1 = 0; c2 = 0;
                    for (int by = yy_m_blb2; by < bymx; by += owidthTimesSize)
                    {
                        byte* sp_p_by = sourcePointer + by;
                        for (int bx = xx_m_blbb2; bx < bxmx; bx += channels)            // jump by amount of channels
                        {
                            byte* areaIndex = sp_p_by + (bx << 2);
                            c2 += *(areaIndex);                                         // accumulate at the byte-adress right away,
                            c1 += *(areaIndex + 1);                                     // don't weigh it, nobody ain't got time for that
                            c0 += *(areaIndex + 2);
                        }
                    }
                    c2 = (c2) / 9;                                                 //  cheating a lil 'bit' (kek) to prevent divisions
                    c0 = (c0) / 9;
                    c1 = (c1) / 9;
                    *(mainIndex) = (byte)(c2 > 255 ? 255 : c2);                         // 1 byte - keep it below 256!
                    *(mainIndex + 1) = (byte)(c0 > 255 ? 255 : c0);
                    *(mainIndex + 2) = (byte)(c1 > 255 ? 255 : c1);
                }
            }
        }


        private static int pixelIndex(int x, int y, int widthTimesSize, int size)
        {

            int idxByRow = y * widthTimesSize;
            int idxByCol = x * size;

            return idxByRow + idxByCol;
        }

        public static void drawFadingLine(Graphics gfx, float x0, float y0, float x1, float y1)
        {

            LinearGradientBrush brush = new LinearGradientBrush(
               new Point((int)x0, (int)y0),
               new Point((int)x1, (int)y1),
               Color.FromArgb(0, 180, 0, 0),   // Opaque red
               Color.FromArgb(255, 180, 0, 0));  // Opaque blue
            Pen pen = new Pen(brush);
            pen.Width = 8;
            gfx.DrawLine(pen, x0, y0, x1, y1);

        }

    }

}

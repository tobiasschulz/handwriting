//
// DebugForm.cs
//
// Author:
//       Tobias Schulz <tobiasschulz.code@outlook.de>
//
// Copyright (c) 2015 Tobias Schulz
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using HandWriting;

namespace DesktopKeyboard
{
    public class DebugForm : Form
    {
        private Size windowSize;
        private Point windowLocation;

        private PixelMap originalPoints;
        private PixelMap resultPoints;

        public DebugForm(Size size, Point location, PixelMap points)
        {
            Load += DebugForm_Load;
            windowSize = size;
            windowLocation = location;

            originalPoints = points;

            ResizeRedraw = true;
            Paint += new PaintEventHandler(OnPaint);
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
            Text = "Debug";
            StartPosition = FormStartPosition.Manual;
            Size = windowSize;
            Location = windowLocation;
            Console.WriteLine("windowSize: " + windowSize);
            Console.WriteLine("windowLocation: " + windowLocation);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            resultPoints = originalPoints.Trim();
            this.Refresh();
        }

        void OnPaint(object sender, PaintEventArgs e)
        {      
            Graphics g = e.Graphics;
            Brush black = Brushes.Black;

            if (resultPoints != null) {
                PointF scale = new PointF((float)Width / (float)resultPoints.Width, (float)Height / (float)resultPoints.Height);
                foreach (Pixel point in resultPoints.GetPixels(value: true)) {
                    int x = (int)((float)point.X * scale.X);
                    int y = (int)((float)point.Y * scale.Y);
                    g.FillRectangle(black, x, y, scale.X, scale.Y);
                }
            }

            g.Dispose();
        }

    }
}


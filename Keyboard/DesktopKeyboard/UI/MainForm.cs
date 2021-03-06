﻿//
// MainForm.cs
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
    public class MainForm : Form, ISize
    {
        private Size windowSize;
        private Point windowLocation;

        private readonly DrawablePixelArea drawArea;
        private readonly PixelArea debugArea1;
        private readonly PixelArea debugArea2;
        private uint previousChangeCounter;

        public MainForm(Size size, Point location)
        {
            Load += MainForm_Load;
            windowSize = size;
            windowLocation = location;

            drawArea = new DrawablePixelArea(reference: this, bounds: new RelativeBounds(reference: this, left: 20, top: 20, right: -520, bottom: -40));
            Controls.Add(drawArea);
            debugArea1 = new PixelArea(reference: this, bounds: new RelativeBounds(reference: this, left: 260, top: 20, right: -260, bottom: -40));
            Controls.Add(debugArea1);
            debugArea2 = new PixelArea(reference: this, bounds: new RelativeBounds(reference: this, left: 520, top: 20, right: -20, bottom: -40));
            Controls.Add(debugArea2);

            ResizeRedraw = true;


            Button button = new Button();
            button.Location = new Point(2, 2);
            button.Size = new Size(40, 20);
            button.Text = "Reset";
            Controls.Add(button);
            button.Click += (s, e) => drawArea.Reset();
            // button.MouseEnter += new EventHandler(OnEnter);

            //Paint += new PaintEventHandler(OnPaint);
            // CenterToScreen();

            //DebugForm form = new DebugForm(size, new Point(location.X + size.Width + 20, location.Y), points);
            //form.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = "Keyboard";
            StartPosition = FormStartPosition.Manual;
            Size = windowSize;
            Location = windowLocation;

            drawArea.Load();
            debugArea1.Load();
            debugArea2.Load();

            System.Windows.Forms.Timer frameTimer = new System.Windows.Forms.Timer();
            frameTimer.Interval = 1000 / 60;
            frameTimer.Tick += OnUpdate;
            frameTimer.Start();


            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (drawArea.Points.ChangeCounter != previousChangeCounter) {
                previousChangeCounter = drawArea.Points.ChangeCounter;
                debugArea1.Points = drawArea.Points.Trim().Normalize();
                debugArea2.Points = drawArea.Points.Trim(test: true).Normalize();

                debugArea1.Draw();
                debugArea2.Draw();

                debugArea1.Invalidate();
                debugArea2.Invalidate();
                //this.Refresh();
            }
        }

        void OnUpdate(object sender, EventArgs e)
        {
            drawArea.OnUpdate();
        }

        /*void OnPaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = e.Graphics) {
                /*Pen pen = new Pen(Color.Black, 1);
                pen.DashStyle = DashStyle.Dot;
                g.DrawLine(pen, 20, 40, 250, 40);
                pen.DashStyle = DashStyle.DashDot;
                g.DrawLine(pen, 20, 80, 250, 80);
                pen.DashStyle = DashStyle.Dash;
                g.DrawLine(pen, 20, 120, 250, 120);
                pen.DashStyle = DashStyle.DashDotDot;
                g.DrawLine(pen, 20, 160, 250, 160);
                pen.DashPattern = new float[] { 6f, 8f, 1f, 1f, 1f, 1f, 1f, 1f };
                g.DrawLine(pen, 20, 200, 250, 200);
                pen.DashStyle = DashStyle.Dot;*

                // drawArea.OnPaint(g);
                // debugArea1.OnPaint(g);
                //debugArea2.OnPaint(g);

            }
        }
*/
    }
}

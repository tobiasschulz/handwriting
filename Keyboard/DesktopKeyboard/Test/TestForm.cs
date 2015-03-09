//
// TestForm.cs
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
using System.Drawing;
using System.Windows.Forms;

namespace DesktopKeyboard
{
    public class TestForm : Form, ISize
    {
        private Size windowSize;
        private Point windowLocation;
        private GeoArea formArea;

        public TestForm(Size size, Point location)
        {
            Load += TestForm_Load;
            windowSize = size;
            windowLocation = location;

            formArea = new GeoArea(reference: this, bounds: new RelativeBounds(reference: this, left: 20, top: 20, right: -20, bottom: -40));
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            Text = "Keyboard";
            StartPosition = FormStartPosition.Manual;
            Size = windowSize;
            Location = windowLocation;

            formArea.Load();

            System.Windows.Forms.Timer frameTimer = new System.Windows.Forms.Timer();
            frameTimer.Interval = 1000 / 60;
            frameTimer.Tick += OnUpdate;
            frameTimer.Start();


        }

        void OnUpdate(object sender, EventArgs e)
        {
            formArea.OnUpdate();
        }

    }
}


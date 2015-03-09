//
// FormArea.cs
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
using HandWriting;
using System.Collections.Generic;

namespace DesktopKeyboard
{
    public sealed class GeoForms
    {
        readonly GeoFormCollection forms;

        readonly CharacterCollection characters;

        public IEnumerable<GeoForm> Forms { get { return forms; } }

        public uint ChangeCounter { get; set; }

        public Size Size { get; set; }

        public GeoForms(Size size)
        {
            Size = size;

            forms = new GeoFormCollection {
                /*
                new RectangleForm(name: "top border", x: new Range(min: 0.21, max: 0.79), y: new Range(min: 0.0, max: 0.20)),
                new RectangleForm(name: "bottom border", x: new Range(min: 0.21, max: 0.79), y: new Range(min: 0.80, max: 1.0)),
                new RectangleForm(name: "left border", x: new Range(min: 0.0, max: 0.20), y: new Range(min: 0.21, max: 0.79)),
                new RectangleForm(name: "right border", x: new Range(min: 0.80, max: 1.0), y: new Range(min: 0.21, max: 0.79)),

                new RectangleForm(name: "top left corner", x: new Range(min: 0.0, max: 0.20), y: new Range(min: 0.0, max: 0.20)),
                new RectangleForm(name: "top right corner", x: new Range(min: 0.80, max: 1.0), y: new Range(min: 0.0, max: 0.20)),
                new RectangleForm(name: "bottom left corner", x: new Range(min: 0.0, max: 0.20), y: new Range(min: 0.80, max: 1.0)),
                new RectangleForm(name: "bottom right corner", x: new Range(min: 0.80, max: 1.0), y: new Range(min: 0.80, max: 1.0)),

                new RectangleForm(name: "top left distanced corner", x: new Range(min: 0.22, max: 0.41), y: new Range(min: 0.22, max: 0.41)),
                new RectangleForm(name: "top right distanced corner", x: new Range(min: 0.59, max: 0.78), y: new Range(min: 0.22, max: 0.41)),
                new RectangleForm(name: "bottom left distanced corner", x: new Range(min: 0.22, max: 0.41), y: new Range(min: 0.59, max: 0.78)),
                new RectangleForm(name: "bottom right distanced corner", x: new Range(min: 0.59, max: 0.78), y: new Range(min: 0.59, max: 0.78)),

                new RectangleForm(name: "left to right bar 1/3", x: new Range(min: 0.0, max: 0.3), y: new Range(min: 0.42, max: 0.58)),
                new RectangleForm(name: "left to right bar 2/3", x: new Range(min: 0.3, max: 0.7), y: new Range(min: 0.42, max: 0.58)),
                new RectangleForm(name: "left to right bar 3/3", x: new Range(min: 0.7, max: 1.0), y: new Range(min: 0.42, max: 0.58)),
                new RectangleForm(name: "top to bottom bar 1/3", x: new Range(min: 0.42, max: 0.58), y: new Range(min: 0.0, max: 0.3)),
                new RectangleForm(name: "top to bottom bar 2/3", x: new Range(min: 0.42, max: 0.58), y: new Range(min: 0.3, max: 0.7)),
                new RectangleForm(name: "top to bottom bar 3/3", x: new Range(min: 0.42, max: 0.58), y: new Range(min: 0.7, max: 1.0)),
                */
            };

            for (int a = 1; a <= 5; a++) {
                for (int b = 1; b <= 5; b++) {
                    forms.Add(new RectangleForm(
                        name: "grid5 " + a + " " + b,
                        x: new Range(min: (a - 1) * 0.20, max: (a) * 0.20),
                        y: new Range(min: (b - 1) * 0.20, max: (b) * 0.20)
                    ));
                }
            }

            for (int a = 1; a <= 4; a++) {
                for (int b = 1; b <= 4; b++) {
                    forms.Add(new RectangleForm(
                        name: "grid4 " + a + " " + b,
                        x: new Range(min: 0.10 + (a - 1) * 0.20, max: 0.10 + (a) * 0.20),
                        y: new Range(min: 0.10 + (b - 1) * 0.20, max: 0.10 + (b) * 0.20)
                    ));
                }
            }

            characters = new CharacterCollection(forms) {
                new Character('A') { },
                new Character('B') { },
                new Character('C') { },
                new Character('D') { },
                new Character('E') { {
                        "grid5 1 1",
                        "grid5 2 1",
                        "grid5 3 1",
                        "grid5 4 1",
                        "grid5 5 1",
                        "grid5 1 2",
                        "grid5 1 3",
                        "grid5 2 3",
                        "grid5 3 3",
                        "grid5 4 3",
                        "grid5 5 3",
                        "grid5 1 4",
                        "grid5 1 5",
                        "grid5 2 5",
                        "grid5 3 5",
                        "grid5 4 5",
                        "grid5 5 5"
                    }
                },
                new Character('F') { {
                        "grid5 1 1",
                        "grid5 2 1",
                        "grid5 3 1",
                        "grid5 4 1",
                        "grid5 5 1",
                        "grid5 1 2",
                        "grid5 1 3",
                        "grid5 2 3",
                        "grid5 3 3",
                        "grid5 4 3",
                        "grid5 5 3",
                        "grid5 1 4",
                        "grid5 1 5"
                    }
                },
                new Character('G') { },
                new Character('H') { },
                new Character('I') { },
                new Character('J') { },
                new Character('K') { },
                new Character('L') { },
                new Character('M') { },
                new Character('N') { },
                new Character('O') { },
                new Character('P') { },
                new Character('Q') { },
                new Character('R') { },
                new Character('S') { },
                new Character('T') { },
                new Character('U') { },
                new Character('V') { },
                new Character('W') { },
                new Character('X') { },
                new Character('Y') { },
                new Character('Z') { },
            };
        }

        public int Count {
            get {
                int activated = 0;
                foreach (GeoForm form in forms) {
                    if (form.IsActivated) {
                        activated++;
                    }
                }
                return activated;
            }
        }

        public void Draw(Graphics g)
        {
            for (int layer = 0; layer < 100; layer++) { 
                foreach (GeoForm form in forms) {
                    form.Draw(g: g, size: Size, layer: layer);
                }
            }
        }

        public void AddPoint(Pixel pixel)
        {
            foreach (GeoForm form in forms) {
                if (form.Contains(pixel: pixel, size: Size)) {
                    form.IsActivated = true;
                }
            }
        }

        public void GetResult(bool success, out string result)
        {
            result = null;
        }
    }

    public abstract class GeoForm
    {
        public string Name { get; protected set; }

        public bool IsActivated { get; set; }

        public static readonly Brush DefaultBrush = new SolidBrush(Color.FromArgb(15, 0, 0, 0));
        public static readonly Brush ActivatedBrush = new SolidBrush(Color.FromArgb(15, 255, 0, 0));

        protected GeoForm(string name)
        {
            Name = name;
            IsActivated = false;
        }

        public abstract void Draw(Graphics g, Size size, int layer);

        public abstract bool Contains(Pixel pixel, Size size);
    }

    public sealed class RectangleForm : GeoForm
    {
        private Range X;
        private Range Y;

        public RectangleForm(string name, Range x, Range y)
            : base(name: name)
        {
            X = x;
            Y = y;
        }

        public override void Draw(Graphics g, Size size, int layer)
        {
            Brush brush = IsActivated ? ActivatedBrush : DefaultBrush;
            if (layer == 10) {
                int xMin = (int)(X.Min * size.Width);
                int xMax = (int)(X.Max * size.Width);
                int yMin = (int)(Y.Min * size.Height);
                int yMax = (int)(Y.Max * size.Height);
                g.FillRectangle(brush, xMin + 2, yMin + 2, xMax - xMin - 2, yMax - yMin - 2);
            }
        }

        public override bool Contains(Pixel pixel, Size size)
        {
            int xMin = (int)(X.Min * size.Width);
            int xMax = (int)(X.Max * size.Width);
            int yMin = (int)(Y.Min * size.Height);
            int yMax = (int)(Y.Max * size.Height);
            Log.Debug("Contains: pixel:", pixel, ", x: ", xMin, "...", xMax, ", y: ", yMin, "...", yMax);
            return xMin <= pixel.X && pixel.X <= xMax && yMin <= pixel.Y && pixel.Y <= yMax; 
        }

        public override string ToString()
        {
            return string.Format("RectangleForm(\"{0}\";X={1};Y={2})", Name, X, Y);
        }
    }
}


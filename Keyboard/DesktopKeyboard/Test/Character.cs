//
// Character.cs
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
using System.Collections.Generic;
using System.Collections;
using HandWriting;
using System.Linq;

namespace DesktopKeyboard
{
    public class Character : IEnumerable<GeoForm[]>, IEnumerable
    {
        public char Name { get; private set; }

        private List<Dictionary<string, GeoForm>> FormCombinations = new List<Dictionary<string, GeoForm>>();
        private List<string[]> rawFormCombinations = new List<string[]>();

        public Character(char name)
        {
            Name = name;
        }

        public bool Initialize(GeoFormCollection forms)
        {
            foreach (string[] rawFormCombi in rawFormCombinations) {
                Dictionary<string, GeoForm> formCombi = new Dictionary<string, GeoForm>();
                foreach (string formName in rawFormCombi) {
                    if (forms.Contains(formName: formName)) {
                        formCombi[formName] = forms.Get(formName: formName);
                    } else {
                        Log.FatalError("GeoForm doesn't exist: '" + formName + "'");
                        return false;
                    }
                }
                FormCombinations.Add(formCombi);
            }
            return true;
        }

        public void Add(params string[] formNames)
        {
            Console.WriteLine("Called Add(string)");
            rawFormCombinations.Add(formNames);
        }

        public IEnumerator<GeoForm[]> GetEnumerator()
        {
            return FormCombinations.Select(dict => dict.Values.ToArray()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}


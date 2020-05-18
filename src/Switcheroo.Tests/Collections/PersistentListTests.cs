﻿/*
The MIT License

Copyright (c) 2013 Riaan Hanekom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Switcheroo.Collections;

namespace Switcheroo.Tests.Collections
{
    using NUnit.Framework;

    [TestFixture]
    public class PersistentListTests
    {
        [Test]
        public void Construction_Sets_Head()
        {
            const int head = 5;
            IEnumerable<int> tail = Enumerable.Range(0, 5);

            var list = new PersistentList<int>(head, tail);

            Assert.AreEqual(list.Head, head);
        }

        [Test]
        public void Construction_Sets_Tail()
        {
            const int head = 5;
            IEnumerable<int> tail = Enumerable.Range(0, 5).ToList();

            var list = new PersistentList<int>(head, tail);
            CollectionAssert.AreEquivalent(tail, list.Tail);
        }

        [Test]
        public void Can_Enumerate_Through_Items()
        {
            const int head = 5;
            IEnumerable<int> tail = Enumerable.Range(0, 3).ToList();

            var list = new PersistentList<int>(head, tail);

            var equivalent = new[] { 5, 0, 1, 2 };
            CollectionAssert.AreEquivalent(equivalent, list);
        }

        [Test]
        public void Construction_Throws_If_Tail_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PersistentList<int>(5, null));
        }
    }
}
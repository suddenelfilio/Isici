﻿/*
The MIT License

Original source: Switcheroo - Copyright (c) 2013 Riaan Hanekom
Isici version: Copyright (c) 2020 Filip Stas

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

using System.Collections.Generic;

namespace Isici.Core.Abstractions.Toggles
{
    /// <summary>
    /// A toggle that has other toggle as dependencies.
    /// </summary>
    public interface IDependencyFeatureToggle : IFeatureToggle
    {
        /// <summary>
        /// Adds the specified feature toggle as a dependency to this one.
        /// </summary>
        /// <param name="toggle">The toggle to add as a dependency.</param>
        void AddDependency(IFeatureToggle toggle);

        /// <summary>
        /// Gets the dependencies of this feature toggle.
        /// </summary>
        /// <value>
        /// The dependencies of this feature toggle.
        /// </value>
        IEnumerable<IFeatureToggle> Dependencies { get; }
    }
}
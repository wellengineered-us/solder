// ***********************************************************************
// Copyright (c) 2019 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Framework
{
    /// <summary>
    /// Specify the life cycle of a Fixture
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class FixtureLifeCycleAttribute : NUnitAttribute, IApplyToTest
    {
        /// <summary>
        /// Construct a FixtureLifeCycleAttribute with a specified <see cref="LifeCycle"/>.
        /// </summary>
        public FixtureLifeCycleAttribute(LifeCycle lifeCycle)
        {
            LifeCycle = lifeCycle;
        }

        /// <summary>
        /// Defines the life cycle for this test fixture or assembly.
        /// </summary>
        public LifeCycle LifeCycle { get; }

        /// <summary>
        /// Overridden to set a TestFixture's <see cref="LifeCycle"/>.
        /// </summary>
        public void ApplyToTest(Test test)
        {
            var testFixture = test as TestFixture;
            if (testFixture != null)
            {
                testFixture.LifeCycle = LifeCycle;
            }
        }
    }
}

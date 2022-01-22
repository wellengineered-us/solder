// ***********************************************************************
// Copyright (c) 2017 Charlie Poole, Rob Prouse
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
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Commands
{
    /// <summary>
    /// OneTimeTearDownCommand performs any teardown actions
    /// specified for a suite and calls Dispose on the user
    /// test object, if any.
    /// </summary>
    public class DisposeFixtureCommand : AfterTestCommand
    {
        /// <summary>
        /// Construct a OneTimeTearDownCommand
        /// </summary>
        /// <param name="innerCommand">The command wrapped by this command</param>
        public DisposeFixtureCommand(TestCommand innerCommand)
            : base(innerCommand)
        {
            Guard.OperationValid(
                HasDisposableFixture(Test),
                $"DisposeFixtureCommand does not apply neither to {Test.GetType().Name}, nor to {Test.Parent?.GetType().Name ?? "it's parent (null)"}");

            AfterTest = (context) =>
            {
                try
                {
                    IDisposable disposable = context.TestObject as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
                catch (Exception ex)
                {
                    context.CurrentResult.RecordTearDownException(ex);
                }
            };
        }

        private static bool HasDisposableFixture(ITest test)
        {
            while (test != null)
            {
                if (test is IDisposableFixture)
                    return true;

                test = test.Parent;
            }

            return false;
        }
    }
}

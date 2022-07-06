using System;
using System.Threading;

namespace Common.Helpers
{
    public class DisposableToken : IDisposable
    {
        private readonly Action activateAction;

        private readonly Action disposeAction;

        public bool IsDisposed { get; private set; }

        private int disposeCount;

        private int activationCount;

        public DisposableToken(Action disposeAction)
        {
            this.disposeAction = disposeAction;

            this.activationCount = 1;
        }

        public DisposableToken(Action activateAction, Action disposeAction, bool immediatelyActivate = false)
        {
            this.activateAction = activateAction;

            this.disposeAction = disposeAction;

            this.activationCount = 0;
            if (immediatelyActivate)
            {
                Activate();
            }
        }

        public void Activate()
        {
            if ((Interlocked.Increment(ref activationCount) == 1) && (activateAction != null))
            {
                activateAction();
            }
        }

        public void Dispose()
        {
            if ((Interlocked.Increment(ref disposeCount) == 1) && (activationCount > 0))
            {
                IsDisposed = true;
                disposeAction();
            }


            GC.SuppressFinalize(this);
        }

        ~DisposableToken()
        {
            if ((Interlocked.Increment(ref disposeCount) == 1) && (activationCount > 0))
            {
                disposeAction();
            }
        }
    }
}
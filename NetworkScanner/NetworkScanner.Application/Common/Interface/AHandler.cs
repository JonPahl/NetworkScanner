/*
using NetworkScanner.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace NetworkScanner.Application
{
    /// <summary>
    /// Abstract Chain of Repository base class.
    /// </summary>
    public abstract class AHandler
    {
        protected AHandler successor;

        protected abstract Result Result { get; set; }

        protected AHandler()
        {

        }

        public void SetSuccessor(AHandler successor)
        {
            this.successor = successor;
        }

        public abstract Task HandleRequest(object request);

        public virtual Task<Result> HandleRequestWithResult(object request)
        {
            return Task.Run(() => new Result());
        }

        protected virtual void SetupNext<T>(T device)
        {
            throw new NotImplementedException();
        }

        //public implicit operator Task(Handler v)
        //{
        //    this.successor = successor;
        //}
    }
}

*/
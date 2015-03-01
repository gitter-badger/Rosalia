﻿using System;

namespace Rosalia.TestingSupport.Helpers
{
    using Rosalia.Core.Api;
    using Rosalia.Core.Tasks.Futures;

    public class Subflow<T> : TaskRegistry<T> where T : class
    {
        private readonly Func<TaskRegistry<T>, ITaskFuture<T>> _defineAction;

        public Subflow(Func<TaskRegistry<T>, ITaskFuture<T>> defineAction)
        {
            _defineAction = defineAction;
        }

        protected override ITaskFuture<T> RegisterTasks()
        {
            return _defineAction.Invoke(this);
        }

        public static Subflow<T> Define(Func<TaskRegistry<T>, ITaskFuture<T>> defineAction)
        {
            return new Subflow<T>(defineAction);
        }
    }
}
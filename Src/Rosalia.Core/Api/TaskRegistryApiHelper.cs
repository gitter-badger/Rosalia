﻿namespace Rosalia.Core.Api
{
    using System;
    using System.Collections.Generic;
    using Rosalia.Core.Api.Behaviors;
    using Rosalia.Core.Tasks.Futures;
    using Rosalia.Core.Tasks.Results;

    public class TaskRegistryApiHelper
    {
        protected Nothing Nothing
        {
            get { return Nothing.Value; }
        }

        /// <summary>
        /// Creates default behavior instance.
        /// </summary>
        public ITaskBehavior Default()
        {
            return new DefaultBehavior();
        }

        public ITaskBehavior DependsOn(Identity dependency)
        {
            return new DependsOnBehavior(dependency);
        }

        public ITaskBehavior DependsOn<T>(ITaskFuture<T> dependency) where T : class
        {
            return new DependsOnBehavior(dependency.Identity);
        }

        public ForEachConfig<T> ForEach<T>(IEnumerable<T> items)
        {
            return new ForEachConfig<T>(items);
        }

        public ITaskResult<T> Success<T>(T data)
        {
            return new SuccessResult<T>(data);
        }

        public ITaskResult<T> Failure<T>(string error)
        {
            return new FailureResult<T>(error);
        }

        public ITaskResult<T> Failure<T>(Exception error)
        {
            return new FailureResult<T>(error);
        }
    }
}
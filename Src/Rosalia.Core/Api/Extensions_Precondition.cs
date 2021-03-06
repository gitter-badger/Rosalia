﻿namespace Rosalia.Core.Api
{
    using System;
    using Rosalia.Core.Tasks;

    public static partial class Extensions
    {
        public static ITask<TResult> WithPrecondition<TResult>(this ITask<TResult> task, Func<bool> predicate, Func<TResult> defaultValueProvider) where TResult : class
        {
            return new PreconditionTask<TResult>(task, predicate, defaultValueProvider);
        }

        public static ITask<TResult> WithPrecondition<TResult>(this ITask<TResult> task, Func<bool> predicate, TResult defaultValue) where TResult : class
        {
            return new PreconditionTask<TResult>(task, predicate, defaultValue);
        }

        public static ITask<TResult> WithPrecondition<TResult>(this ITask<TResult> task, Func<bool> predicate) where TResult : class
        {
            return new PreconditionTask<TResult>(task, predicate);
        }

        public static ITask<TResult> WithPrecondition<TResult>(this ITask<TResult> task, bool predicate, Func<TResult> defaultValueProvider) where TResult : class
        {
            return new PreconditionTask<TResult>(task, () => predicate, defaultValueProvider);
        }

        public static ITask<TResult> WithPrecondition<TResult>(this ITask<TResult> task, bool predicate, TResult defaultValue) where TResult : class
        {
            return new PreconditionTask<TResult>(task, () => predicate, defaultValue);
        }

        public static ITask<TResult> WithPrecondition<TResult>(this ITask<TResult> task, bool predicate) where TResult : class
        {
            return new PreconditionTask<TResult>(task, () => predicate);
        }
    }
}
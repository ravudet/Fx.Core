namespace System.Threading.Tasks
{
    using System.Runtime.CompilerServices;

    public sealed class TaskWrapper<T> : ITask<T>
    {
        private readonly Task<T> task;

        public TaskWrapper(Task<T> task)
        {
            this.task = task;
        }

        public Exception? Exception
        {
            get
            {
                return this.task.Exception;
            }
        }

        public bool IsCanceled
        {
            get
            {
                return this.task.IsCanceled;
            }
        }

        public IAwaiter<T> GetAwaiter()
        {
            return new Awaiter(this.task.GetAwaiter());
        }

        private sealed class Awaiter : IAwaiter<T>
        {
            private readonly TaskAwaiter<T> taskAwaiter;

            public Awaiter(TaskAwaiter<T> taskAwaiter)
            {
                this.taskAwaiter = taskAwaiter;
            }

            public bool IsCompleted
            {
                get
                {
                    return this.taskAwaiter.IsCompleted;
                }
            }

            public T GetResult()
            {
                return this.taskAwaiter.GetResult();
            }

            public void OnCompleted(Action continuation)
            {
                this.taskAwaiter.OnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                this.taskAwaiter.UnsafeOnCompleted(continuation);
            }
        }

        public IConfiguredTask<T> ConfigureAwait(ConfigureAwaitOptions configureAwaitOptions)
        {
            return new ConfiguredTask(this.task, configureAwaitOptions);
        }

        private sealed class ConfiguredTask : IConfiguredTask<T>
        {
            private readonly Task<T> task;
            private readonly ConfigureAwaitOptions configureAwaitOptions;

            public ConfiguredTask(Task<T> task, ConfigureAwaitOptions configureAwaitOptions)
            {
                this.task = task;
                this.configureAwaitOptions = configureAwaitOptions;
            }

            public Exception? Exception
            {
                get
                {
                    return this.task.Exception;
                }
            }

            public bool IsCanceled
            {
                get
                {
                    return this.task.IsCanceled;
                }
            }

            public IAwaiter<T> GetAwaiter()
            {
                ConfiguredTaskAwaitable<T> configuredTaskAwaitable;
#if !NET8_0_OR_GREATER
                configuredTaskAwaitable = this.task.ConfigureAwait(configureAwaitOptions == ConfigureAwaitOptions.ContinueOnCapturedContext);
#else
                configuredTaskAwaitable = this.task.ConfigureAwait(configureAwaitOptions);
#endif

                return new Awaiter(configuredTaskAwaitable.GetAwaiter());
            }

            private sealed class Awaiter : IAwaiter<T>
            {
                private readonly ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter configuredTaskAwaiter;

                public Awaiter(ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter configuredTaskAwaiter)
                {
                    this.configuredTaskAwaiter = configuredTaskAwaiter;
                }

                public bool IsCompleted
                {
                    get
                    {
                        return this.configuredTaskAwaiter.IsCompleted;
                    }
                }

                public T GetResult()
                {
                    return this.configuredTaskAwaiter.GetResult();
                }

                public void OnCompleted(Action continuation)
                {
                    this.configuredTaskAwaiter.OnCompleted(continuation);
                }

                public void UnsafeOnCompleted(Action continuation)
                {
                    this.configuredTaskAwaiter.UnsafeOnCompleted(continuation);
                }
            }
        }
    }
}

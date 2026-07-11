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

        public IAwaitable<T> ConfigureAwait(bool continueOnCapturedContext)
        {
            return new Awaitable(this.task.ConfigureAwait(continueOnCapturedContext));
        }

        private sealed class Awaitable : IAwaitable<T>
        {
            private readonly ConfiguredTaskAwaitable<T> configuredTaskAwaitable;

            public Awaitable(ConfiguredTaskAwaitable<T> configuredTaskAwaitable)
            {
                this.configuredTaskAwaitable = configuredTaskAwaitable;
            }

            public IAwaiter<T> GetAwaiter()
            {
                return new Awaiter(this.configuredTaskAwaitable.GetAwaiter());
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
    }
}

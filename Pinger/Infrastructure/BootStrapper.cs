namespace Pinger.Infrastructure
{
    using Ninject;

    public class BootStrapper
    {
        private static BootStrapper instance;

        private readonly StandardKernel kernel;

        public BootStrapper()
        {
            this.kernel = new StandardKernel(new PingerModule());
        }

        public TObj Get<TObj>()
        {
            return this.kernel.Get<TObj>();
        }

        public static BootStrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BootStrapper();
                }

                return instance;
            }
        }
    }
}

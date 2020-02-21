namespace Pinger.Infrastructure
{
    using Pinger.ViewModels;

    public class ServiceLocator
    {
        public ILivePingerViewModel LivePingerViewModel
        {
            get
            {
                return BootStrapper.Instance.Get<ILivePingerViewModel>();
            }
        }
    }
}

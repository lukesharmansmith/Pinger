namespace Pinger.Services
{
    using Microsoft.Toolkit.Uwp.Notifications;
    using Pinger.Models;
    using Windows.Data.Xml.Dom;
    using Windows.UI.Notifications;

    public class ToastService : IToastService
    {
        private readonly ToastNotifier toastNotifier;

        private readonly IResourceLocator resourceLocator;

        public ToastService(IResourceLocator resourceLocator)
        {
            this.resourceLocator = resourceLocator;
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier("SharmanSmith.Pinger");
        }

        public void Show(PingResult result)
        {
            var image = this.resourceLocator.GetPathToAsset(result.IsSuccess ? ResourceAsset.ToastIconNormal : ResourceAsset.ToastIconBad);

            ToastContent toastContent = new ToastContent()
            {
                Launch = "action=ok",

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = $"Ping response : {result.Status}"
                            },
                            new AdaptiveText()
                            {
                                Text = $"Gateway : {(result.GatewayAddress == null ? string.Empty : result.GatewayAddress.ToString())}"
                            },
                            new AdaptiveText()
                            {
                                Text = $"RoundTrip time : {(result.RoundTripTime == 0 ? result.Duration.TotalMilliseconds : result.RoundTripTime )} ms"
                            },
                        },
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = "Via Pinger"
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = image,
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                }
            };

            ShowToastInternal(toastContent);
        }

        public void Show(string message)
        {
            ToastContent toastContent = new ToastContent()
            {
                Launch = "action=ok",

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = message
                            }
                        },
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = "Via Pinger"
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = this.resourceLocator.GetPathToAsset(ResourceAsset.ToastIconNormal),
                            HintCrop = ToastGenericAppLogoCrop.None
                        }
                    },
                }
            };

            this.ShowToastInternal(toastContent);
        }

        private void ShowToastInternal(ToastContent content)
        {
            //ToastNotificationManager.History.Clear();

            var doc = new XmlDocument();
            doc.LoadXml(content.GetContent());

            var toast = new ToastNotification(doc);

            toastNotifier.Show(toast);
        }
    }
}

namespace Pinger.Services
{
    using Microsoft.Toolkit.Uwp.Notifications;
    using Pinger.Models;
    using System.Windows;
    using Windows.Data.Xml.Dom;
    using Windows.UI.Notifications;

    public class ToastService : IToastService
    {
        private const string ApplicatiopnID = "SharmanSmith.Pinger";

        private readonly ToastNotifier toastNotifier;

        private readonly ToastNotificationHistory toastNotificationHistory;

        private readonly IResourceLocator resourceLocator;

        public ToastService(IResourceLocator resourceLocator)
        {
            this.resourceLocator = resourceLocator;
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier(ApplicatiopnID);
            this.toastNotificationHistory = ToastNotificationManager.History;
        }

        public void Show(ToastMessage message)
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
                                Text = message.Title
                            },
                            new AdaptiveText()
                            {
                                Text = message.Line1
                            },
                            new AdaptiveText()
                            {
                                Text = message.Line2
                            },
                        },
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = "Via Pinger"
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = this.resourceLocator.GetPathToAsset(message.Icon),
                            HintCrop = ToastGenericAppLogoCrop.None
                        }
                    },
                }
            };

            this.ShowToastInternal(toastContent);
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(content.GetContent());

                var toast = new ToastNotification(doc);

                this.toastNotificationHistory.Clear(ApplicatiopnID);
                this.toastNotifier.Show(toast);
            });
        }
    }
}

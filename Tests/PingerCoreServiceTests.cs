namespace Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pinger.Models;
    using Pinger.Services;
    using System;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;

    [TestClass]
    public class PingerCoreServiceTests
    {
        [TestMethod]
        public void PingerCoreServiceTests_StartCallsPingerStart()
        {
            var mockNetworkPinger = new Mock<INetworkPinger>();

            var pinger = GetInstance(mockNetworkPinger: mockNetworkPinger);

            pinger.Start();

            mockNetworkPinger.Verify(x => x.Start(), Times.Once, "Should of called the start method on the network pinger");
        }

        [TestMethod]
        public async Task PingerCoreServiceTests_PingerResponse_GoodResponse_NoToast()
        {
            var mockTrayIconHandler = new Mock<ITrayIconHandler>();
            var mockWindowHandler = new Mock<IWindowHandler>();
            var mockNetworkPinger = new Mock<INetworkPinger>();
            var mockToastService = new Mock<IToastService>();

            var pinger = GetInstance(mockTrayIconHandler, mockToastService, mockNetworkPinger, mockWindowHandler);

            pinger.SampleTimingThreshold = TimeSpan.FromSeconds(1);

            mockNetworkPinger.Raise(x => x.Response += null, GetSuccessResult());
            mockNetworkPinger.Raise(x => x.Response += null, GetSuccessResult());
            mockNetworkPinger.Raise(x => x.Response += null, GetSuccessResult());

            await Task.Delay(pinger.SampleTimingThreshold);

            mockNetworkPinger.Raise(x => x.Response += null, GetSuccessResult());
            mockNetworkPinger.Raise(x => x.Response += null, GetSuccessResult());
            mockNetworkPinger.Raise(x => x.Response += null, GetSuccessResult());

            mockToastService.Verify(x => x.Show(It.IsAny<ToastMessage>()), Times.Never, "Should of notified about failed network state - Toast");
            mockTrayIconHandler.Verify(x => x.SetTrayIconState(NetworkUpState.Failed), Times.Never, "Should of notified about failed network state - Tray");
        }

        [TestMethod]
        public async Task PingerCoreServiceTests_PingerResponse_BadResponse_Toast()
        {
            var mockTrayIconHandler = new Mock<ITrayIconHandler>();
            var mockWindowHandler = new Mock<IWindowHandler>();
            var mockNetworkPinger = new Mock<INetworkPinger>();
            var mockToastService = new Mock<IToastService>();

            var pinger = GetInstance(mockTrayIconHandler, mockToastService, mockNetworkPinger, mockWindowHandler);

            pinger.SampleTimingThreshold = TimeSpan.FromSeconds(1);

            mockNetworkPinger.Raise(x => x.Response += null, GetFailedResult());
            mockNetworkPinger.Raise(x => x.Response += null, GetFailedResult());
            mockNetworkPinger.Raise(x => x.Response += null, GetFailedResult());

            await Task.Delay(pinger.SampleTimingThreshold);

            mockNetworkPinger.Raise(x => x.Response += null, GetFailedResult());
            mockNetworkPinger.Raise(x => x.Response += null, GetFailedResult());
            mockNetworkPinger.Raise(x => x.Response += null, GetFailedResult());

            mockToastService.Verify(x => x.Show(It.IsAny<ToastMessage>()), Times.Once, "Should not of notified about failed network state - Toast");
            mockTrayIconHandler.Verify(x => x.SetTrayIconState(NetworkUpState.Failed), Times.AtLeastOnce, "Should not of notified about failed network state - Tray");
        }

        private static PingerResultEventArgs GetSuccessResult()
        {
            return new PingerResultEventArgs(new PingResult(15, DateTime.Now, TimeSpan.Zero, IPAddress.Parse("127.0.0.1"), IPStatus.Success));
        }

        private static PingerResultEventArgs GetFailedResult()
        {
            return new PingerResultEventArgs(new PingResult(15, DateTime.Now, TimeSpan.Zero, IPAddress.Parse("127.0.0.1"), IPStatus.BadHeader));
        }

        private static PingerCoreService GetInstance(
            Mock<ITrayIconHandler> mockTrayHandler = null,
            Mock<IToastService> mockToastService = null,
            Mock<INetworkPinger> mockNetworkPinger = null,
            Mock<IWindowHandler> mockWindowHandler = null)
        {
            if (mockTrayHandler == null) mockTrayHandler = new Mock<ITrayIconHandler>();
            if (mockToastService == null) mockToastService = new Mock<IToastService>();
            if (mockNetworkPinger == null) mockNetworkPinger = new Mock<INetworkPinger>();
            if (mockWindowHandler == null) mockWindowHandler = new Mock<IWindowHandler>();

            var mockInterfaceService = new Mock<IInterfaceService>();
            mockInterfaceService.SetupGet(x => x.ToastService).Returns(mockToastService.Object);
            mockInterfaceService.SetupGet(x => x.TrayIconHandler).Returns(mockTrayHandler.Object);
            mockInterfaceService.SetupGet(x => x.WindowHandler).Returns(mockWindowHandler.Object);

            return new PingerCoreService(mockInterfaceService.Object, mockNetworkPinger.Object);
        }
    }
}

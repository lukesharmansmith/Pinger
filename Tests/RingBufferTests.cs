namespace Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pinger.Services;
    using System.Linq;

    [TestClass]
    public class RingBufferTests
    {
        [TestMethod]
        public void RingBufferTests_WrapsAroundAtLimit()
        {
            var buffer = new RingBuffer<int>(10);

            for(int i=0; i < 20; i++)
            {
                buffer.AddItemToQueue(i);
            }

            Assert.AreEqual(10, buffer.Get.Count());
        }

        [TestMethod]
        public void RingBufferTests_WrapsAroundAtLimit_LastItem()
        {
            var buffer = new RingBuffer<int>(10);

            for (int i = 0; i < 20; i++)
            {
                buffer.AddItemToQueue(i);
            }

            Assert.AreEqual(10, buffer.Get.Count());
            Assert.AreEqual(10, buffer.Get.First());
        }
    }
}

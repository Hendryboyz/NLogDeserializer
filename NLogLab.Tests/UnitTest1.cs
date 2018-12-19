using NUnit.Framework;

namespace NLogLab.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void DynamicLog()
        {
            string json = "{\"array\":[{\"name\":\"henry\",\"age\":25},{\"name\":\"henry\",\"age\":25},{\"name\":\"henry\",\"age\":25}],\"array2\":[1,2,3],\"array3\":[[1,2,3],[1,2,3],[1,2,3]],\"boolean\":true,\"color\":\"#82b92c\",\"null\":null,\"number\":123,\"object\":{\"a\":\"b\",\"c\":\"d\",\"e\":\"f\"},\"string\":\"Hello World\"}";
            NLogMain main = new NLogMain();
            main.DemoDynamicWrite(json);
        }
    }
}

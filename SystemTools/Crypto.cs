using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using SystemTools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemTools
{
    [TestClass]
    public class Crypto
    {
        public static string GetHashString(string value)
        {
            var hashString = value.GetHashBytes().GetString();
            return hashString;
        }

        [TestMethod]
        public void GetHashStringTest()
        {
            var hashString = GetHashString("Hello World !!!");
            Debug.WriteLine(hashString);

            Assert.AreEqual("뉷Ꜭ増଴ᅝ钭쬒⹋", hashString);
        }
    }
}

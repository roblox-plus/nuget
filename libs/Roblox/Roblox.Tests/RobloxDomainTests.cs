using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Roblox.Api;

namespace Roblox.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class RobloxDomainTests
    {
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(RobloxDomain.EconomyApi, "/docs", null).Returns("https://economy.roblox.com/docs");

                yield return new TestCaseData(RobloxDomain.EconomyApi, "v2/groups/2518656/transactions?cursor=&limit=100", new Dictionary<string, string>
                {
                    ["transactionType"] = "Sale"
                }).Returns("https://economy.roblox.com/v2/groups/2518656/transactions?cursor=&limit=100&transactionType=Sale");

                yield return new TestCaseData(RobloxDomain.EconomyApi, "v2/users/48103520/transactions", new Dictionary<string, string>
                {
                    ["cursor"] = "",
                    ["limit"] = "100",
                    ["transactionType"] = "Purchase"
                }).Returns("https://economy.roblox.com/v2/users/48103520/transactions?cursor=&limit=100&transactionType=Purchase");

                yield return new TestCaseData(RobloxDomain.EconomyApi, "fake?", new Dictionary<string, string>
                {
                    ["test"] = "I have special & characters.",
                    ["null"] = null
                }).Returns("https://economy.roblox.com/fake?test=I+have+special+%26+characters.&null=");

                yield return new TestCaseData(RobloxDomain.EconomyApi, null, null).Returns("https://economy.roblox.com/");
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public string Build_ValidArguments_ReturnsUrl(string domain, string path, IReadOnlyDictionary<string, string> queryParameters)
        {
            var uri = RobloxDomain.Build(domain, path, queryParameters);
            return uri.ToString();
        }
    }
}

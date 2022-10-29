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

        [TestCase(RobloxEntity.User, 48103520, "WebGL3D", ExpectedResult = "https://www.roblox.com/users/48103520/profile")]
        [TestCase(RobloxEntity.Group, 2518656, "Roblox+", ExpectedResult = "https://www.roblox.com/groups/2518656/Roblox")]
        [TestCase(RobloxEntity.Asset, 1272714, "Wanwood Antlers", ExpectedResult = "https://www.roblox.com/catalog/1272714/Wanwood-Antlers")]
        [TestCase(RobloxEntity.Bundle, 192, "Korblox Deathspeaker", ExpectedResult = "https://www.roblox.com/bundles/192/Korblox-Deathspeaker")]
        [TestCase(RobloxEntity.GamePass, 4257788, "Bear Bee", ExpectedResult = "https://www.roblox.com/game-pass/4257788/Bear-Bee")]
        [TestCase(RobloxEntity.Badge, 1887996626, "WOE", ExpectedResult = "https://www.roblox.com/badges/1887996626/WOE")]
        [TestCase(RobloxEntity.Group, 2518656, "+", ExpectedResult = "https://www.roblox.com/groups/2518656/unnamed")]
        public string BuildWebsiteItemUrl_ValidArguments_ReturnsUrl(string robloxEntity, long id, string name)
        {
            // Most of these tests aren't actually good, because they don't touch enough special characters.
            var url = RobloxDomain.BuildWebsiteItemUrl(robloxEntity, id, name);
            return url.ToString();
        }
    }
}

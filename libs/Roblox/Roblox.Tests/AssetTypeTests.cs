using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Roblox.Api;

namespace Roblox.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class AssetTypeTests
    {
        [TestCase(AssetType.TShirt, ExpectedResult = "T-Shirt")]
        [TestCase(AssetType.TShirtAccessory, ExpectedResult = "T-Shirt")]
        [TestCase(AssetType.HairAccessory, ExpectedResult = "Hair")]
        [TestCase(AssetType.Shirt, ExpectedResult = "Shirt")]
        [TestCase(AssetType.DynamicHead, ExpectedResult = "Head")]
        public string GetDisplayName_ValidAssetType_ReturnsDisplayName(AssetType assetType)
        {
            return assetType.GetDisplayName();
        }
    }
}

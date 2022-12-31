using AutoFixture;

namespace AutoTesting.Test.TestItems;

public class TestCustomization : ICustomization
{
    public const string CustomizedText = "Customized Boo";

    public void Customize(IFixture fixture) =>
        fixture.Customize<TestDataClass>(x => x.With(t => t.Boo, CustomizedText));
}
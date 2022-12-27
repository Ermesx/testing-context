namespace AutoTesting.Test
{
    using AutoFixture.AutoMoq;
    using FluentAssertions;
    using Xunit;


    public class ConfigurationTests : TestingContext<ContextConfiguration>
    {
        static ConfigurationTests()
        {
            ContextConfiguration.ClearDefaults();
        }

        [Fact(Skip = "Run manually, because it's has static global influence")]
        public void Can_clear_default_customizations_before_any_action()
        {
            // Act 
            var testConfig = TestObject;

            // Assert
            testConfig.Customizations.Should().NotContain(c => c.GetType() == typeof(MockPostprocessor));
        }
    }
}


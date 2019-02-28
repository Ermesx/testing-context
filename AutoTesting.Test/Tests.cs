namespace AutoTesting.Test
{
    using System;
    using FluentAssertions;
    using TestItems;
    using Xunit;

    public class Tests : TestingContext<TestClass>
    {
        [Fact]
        public void Can_configure_mock()
        {
            // Arrange
            const string text = "boo";
            ConfigureMock<IFoo>(mock => mock.Setup(x => x.Boo()).Returns(text));
            var testClass = ClassUnderTest;
            
            // Act
            var boo = testClass.GetBoo();
            
            // Assert
            boo.Should().Be(text);
        }

        [Fact]
        public void Can_change_mock_configuration()
        {
            // Arrange
            const string text = "foo";
            ConfigureMock<IFoo>(mock => mock.Setup(x => x.Boo()).Returns("boo"));
            var testClass = ClassUnderTest;
            
            // Act
            ConfigureMock<IFoo>(mock => mock.Setup(x => x.Boo()).Returns(text));
            
            // Assert
            var boo = testClass.GetBoo();
            boo.Should().Be(text);
        }

        [Fact]
        public void Can_inject_specific_class()
        {
            // Arrange
            var specClass = new SpecFoo();
            Inject<IFoo>(specClass);
            var testClass = ClassUnderTest;

            // Act 
            var boo = testClass.GetBoo();

            // Assert
            boo.Should().Be(specClass.Boo());
        }

        [Fact]
        public void Can_inject_enum()
        {
            // Arrange
            const TestEnum @enum = TestEnum.T1;
            Inject(@enum);
            var testClass = ClassUnderTest;

            // Act 
            var testEnum = testClass.GetEnum();

            // Assert
            testEnum.Should().Be(@enum);
        }

        [Fact]
        public void Cannot_inject_the_same_class_twice()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                Inject<IFoo>(new SpecFoo());
                Inject<IFoo>(new SpecFoo());
            });
        }

        [Fact]
        public void Can_create_any_mock_data()
        {
            // Act
            var data = MockData<TestDataClass>();

            // Assert
            data.Guid.Should().NotBeEmpty();
            data.Boo.Should().NotBeNullOrEmpty();
            data.Foo.Should().NotBeNullOrEmpty();
        }
        
        
    }
}
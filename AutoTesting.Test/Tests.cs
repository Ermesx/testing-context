using AutoTesting.Test.TestItems;
using FluentAssertions;
using Xunit;

namespace AutoTesting.Test;

public class Tests : TestingContext<TestClass>
{
    [Fact]
    public void Can_configure_mock()
    {
        // Arrange
        const string text = "boo";
        Mock<IFoo>().Setup(x => x.Boo()).Returns(text);
        var testClass = TestObject;

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
        Mock<IFoo>().Setup(x => x.Boo()).Returns("boo");
        var testClass = TestObject;

        // Act
        Mock<IFoo>().Setup(x => x.Boo()).Returns(text);

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
        var testClass = TestObject;

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
        var testClass = TestObject;

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
    public void Cannot_inject_null()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Inject<IFoo>(null!));
    }

    [Fact]
    public void Can_create_any_mock_data()
    {
        // Act
        var data = Make<TestDataClass>();

        // Assert
        data.Guid.Should().NotBeEmpty();
        data.Boo.Should().NotBeNullOrEmpty();
        data.Foo.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Can_add_customization_and_it_works()
    {
        // Arrange
        var customization = new TestCustomization();
        Configuration.Customize(customization);

        // Act 
        var data = Make<TestDataClass>();

        // Assert
        data.Boo.Should().Be(TestCustomization.CustomizedText);
    }

    [Fact]
    public void Can_add_customization_by_function_and_it_works()
    {
        // Arrange
        const string text = "test customization";
        Configuration.Customize<TestDataClass>(x => x.With(t => t.Boo, text));

        // Act 
        var data = Make<TestDataClass>();

        // Assert
        data.Boo.Should().Be(text);
    }
}
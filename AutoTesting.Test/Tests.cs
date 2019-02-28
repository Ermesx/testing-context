namespace AutoTesting.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class Tests : TestingContext<Tests.TestClass>
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
            InjectClassFor<IFoo>(specClass);
            var testClass = ClassUnderTest;

            // Act 
            var boo = testClass.GetBoo();

            // Assert
            boo.Should().Be(specClass.Boo());
        }

        [Fact]
        public void Cannot_inject_the_same_class_twice()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                InjectClassFor<IFoo>(new SpecFoo());
                InjectClassFor<IFoo>(new SpecFoo());
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
        
        public class TestClass
        {
            private readonly IFoo _foo;

            public TestClass(IFoo foo)
            {
                _foo = foo;
            }

            public string GetBoo()
            {
                return _foo.Boo();
            }
        }
        
        public class SpecFoo : IFoo
        {
            public string Boo()
            {
                return "SpecFoo";
            }
        }
        
        public class TestDataClass
        {
            public TestDataClass(Guid guid)
            {
                Guid = guid;
            }

            public string Foo { get; set; }
            
            public string Boo { get; set; }
            
            public Guid Guid { get; }
        }
        
        public interface IFoo
        {
            string Boo();
        }
    }
}
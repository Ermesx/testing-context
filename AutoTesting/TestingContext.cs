namespace AutoTesting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Moq;

    public abstract class TestingContext<T> where T : class
    {
        private readonly Fixture _fixture;
        private readonly Dictionary<Type, Mock> _injectedMocks;
        private readonly Dictionary<Type, object> _injectedConcreteObjects;

        /// <summary>
        /// Create instance of testing class from configured fixture
        /// </summary>
        /// <returns></returns>
        public T ClassUnderTest => _fixture.Create<T>();

        /// <summary>
        /// Create instance of any class with assigned data within
        /// </summary>
        /// <returns></returns>
        public TData MockData<TData>() => _fixture.Create<TData>();

        protected TestingContext()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());

            _injectedMocks = new Dictionary<Type, Mock>();
            _injectedConcreteObjects = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Generates a mock for a class/interfaces and injects it into the final fixture
        /// </summary>
        /// <typeparam name="TMockType"></typeparam>
        /// <returns></returns>
        public void ConfigureMock<TMockType>(Action<Mock<TMockType>> configure)
            where TMockType : class
        {
            var existingMock = _injectedMocks.FirstOrDefault(x => x.Key == typeof(TMockType));
            if (existingMock.Key == null)
            {
                var newMock = new Mock<TMockType>();
                existingMock = new KeyValuePair<Type, Mock>(typeof(TMockType), newMock);
                _injectedMocks.Add(existingMock.Key, existingMock.Value);
                _fixture.Inject(newMock.Object);
            }

            var mock = existingMock.Value as Mock<TMockType>;
            configure(mock);
        }

        /// <summary>
        /// Injects a concrete object to be used when generating the fixture. 
        /// </summary>
        /// <typeparam name="TClassType"></typeparam>
        /// <returns></returns>
        public void Inject<TClassType>(TClassType injectedObject)
        {
            var existingClass = _injectedConcreteObjects.FirstOrDefault(x => x.Key == typeof(TClassType));
            if (existingClass.Key != null)
            {
                throw new ArgumentException($"{injectedObject.GetType().Name} has been injected more than once");
            }

            _injectedConcreteObjects.Add(typeof(TClassType), injectedObject);
            _fixture.Inject(injectedObject);
        }
    }
}

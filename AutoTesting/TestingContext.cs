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
        private readonly Dictionary<Type, Mock> _mocks;
        private readonly Dictionary<Type, object> _injectedObjects;

        protected TestingContext()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());

            _mocks = new Dictionary<Type, Mock>();
            _injectedObjects = new Dictionary<Type, object>();
        }
        
        /// <summary>
        /// Create instance of testing class from configured fixture
        /// </summary>
        /// <returns></returns>
        public T ClassUnderTest => _fixture.Create<T>();

        /// <summary>
        /// Create instance of any class with assigned data within
        /// </summary>
        /// <returns></returns>
        public TData Fixture<TData>() => _fixture.Create<TData>();

        /// <summary>
        /// Generates a mock for a class/interfaces and injects it into the final fixture
        /// </summary>
        /// <typeparam name="TMockType"></typeparam>
        /// <returns></returns>
        public void Mock<TMockType>() where TMockType : class
        {
            var mockType = typeof(TMockType);
            var existingMock = _mocks.FirstOrDefault(x => x.Key == mockType);
            if (existingMock.Key == null)
            {
                var newMock = new Mock<TMockType>();
                existingMock = new KeyValuePair<Type, Mock>(mockType, newMock);
                _mocks.Add(existingMock.Key, existingMock.Value);
                _fixture.Inject(newMock.Object);
            }

            var mock = existingMock.Value as Mock<TMockType>;
            return mock;
        }

        /// <summary>
        /// Injects a concrete object to be used when generating the fixture. 
        /// </summary>
        /// <typeparam name="TClassType"></typeparam>
        /// <returns></returns>
        public void Inject<TObjectType>(TObjectType injectedObject)
        {
            var objectType = typeof(TObjectType);
            var existingObject = _injectedObjects.FirstOrDefault(x => x.Key == objectType);
            if (existingObject.Key != null)
            {
                throw new ArgumentException($"{injectedObject.GetType().Name} has been injected more than once");
            }

            _injectedObjects.Add(objectType, injectedObject);
            _fixture.Inject(injectedObject);
        }
    }
}

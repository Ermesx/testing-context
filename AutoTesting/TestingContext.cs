namespace AutoTesting
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Moq;
    
    public abstract class TestingContext<T> : ITestingContext<T> where T : class
    {
        private readonly Fixture _fixture;
        private readonly Dictionary<Type, Mock> _mocks;
        private readonly Dictionary<Type, object> _injectedObjects;

        protected TestingContext()
        {
            _fixture = new Fixture();
            Configuration = new ContextConfiguration(_fixture);

            _mocks = new Dictionary<Type, Mock>();
            _injectedObjects = new Dictionary<Type, object>();
        }

        /// <inheritdoc />
        public ContextConfiguration Configuration { get; }

        /// <inheritdoc />
        public T TestObject => _fixture.Create<T>();

        /// <inheritdoc />
        public TData Make<TData>() => _fixture.Create<TData>();

        /// <inheritdoc />
        public Mock<TMockType> Mock<TMockType>() where TMockType : class
        {
            var mockType = typeof(TMockType);
            return !_mocks.ContainsKey(mockType) ? CreateNew() : _mocks[mockType] as Mock<TMockType>;

            Mock<TMockType> CreateNew()
            {
                var newMock = new Mock<TMockType>();
                _mocks.Add(mockType, newMock);
                _fixture.Inject(newMock.Object);

                return newMock;
            }
        }

        /// <inheritdoc />
        public void Inject<TObjectType>(TObjectType injectedObject)
        {
            var objectType = typeof(TObjectType);
            if (_injectedObjects.ContainsKey(objectType))
            {
                throw new ArgumentException($"{nameof(injectedObject)} has been injected more than once");
            }

            // It's rare case and not impact on performance of tests code
            _injectedObjects.Add(objectType, injectedObject);
            _fixture.Inject(injectedObject);
        }
    }
}

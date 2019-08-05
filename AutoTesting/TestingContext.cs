namespace AutoTesting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Dsl;
    using AutoFixture.Kernel;
    using Moq;
    
    public abstract class TestingContext<T> : ITestingContext<T> where T : class
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

        /// <inheritdoc />
        public void AddCustomization(ICustomization customization)
        {
            _fixture.Customize(customization);
        }

        /// <inheritdoc />
        public void AddCustomization<TData>(Func<ICustomizationComposer<TData>, ISpecimenBuilder> composerTransformation)
        {
            _fixture.Customize<TData>(composerTransformation);
        }

        /// <inheritdoc />
        public T TestObject => _fixture.Create<T>();

        /// <inheritdoc />
        [Obsolete("Use Make<> instead")]
        public TData Fixture<TData>() => Make<TData>();
        
        /// <inheritdoc />
        public TData Make<TData>() => _fixture.Create<TData>();

        /// <inheritdoc />
        public Mock<TMockType> Mock<TMockType>() where TMockType : class
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

        /// <inheritdoc />
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

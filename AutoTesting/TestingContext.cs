﻿namespace AutoTesting
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

            _injectedObjects.Add(objectType, injectedObject);
            _fixture.Inject(injectedObject);
        }
    }
}

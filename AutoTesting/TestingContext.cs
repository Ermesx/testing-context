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
        private readonly Dictionary<Type, object> _injectedConcreteClasses;

        /// <summary>
        /// Create instance of testing class and injects it into the final fixture
        /// </summary>
        /// <returns></returns>
        public T ClassUnderTest => _fixture.Create<T>();

        /// <summary>
        /// Create instance of any class as data object
        /// </summary>
        /// <returns></returns>
        public TMock CreateMockData<TMock>() => _fixture.Create<TMock>();

        protected TestingContext()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());

            _injectedMocks = new Dictionary<Type, Mock>();
            _injectedConcreteClasses = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Generates a mock for a class and injects it into the final fixture
        /// </summary>
        /// <typeparam name="TMockType"></typeparam>
        /// <returns></returns>
        public Mock<TMockType> GetMockFor<TMockType>()
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

            return existingMock.Value as Mock<TMockType>;
        }

        /// <summary>
        /// Injects a concrete class to be used when generating the fixture. 
        /// </summary>
        /// <typeparam name="TClassType"></typeparam>
        /// <returns></returns>
        public void InjectClassFor<TClassType>(TClassType injectedClass)
            where TClassType : class
        {
            var existingClass = _injectedConcreteClasses.FirstOrDefault(x => x.Key == typeof(TClassType));
            if (existingClass.Key != null)
            {
                throw new Exception($"{injectedClass.GetType().Name} has been injected more than once");
            }

            _injectedConcreteClasses.Add(typeof(TClassType), injectedClass);
            _fixture.Inject(injectedClass);
        }
    }
}

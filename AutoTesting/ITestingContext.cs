namespace AutoTesting
{
    using AutoFixture;
    using AutoFixture.Dsl;
    using AutoFixture.Kernel;
    using Moq;
    using System;

    public interface ITestingContext<out T> where T : class
    {
        /// <summary>
        /// Create instance of testing class from configured fixture
        /// </summary>
        /// <returns>Test object</returns>
        T TestObject { get; }

        /// <summary>
        /// Add customization to AutoFixture
        /// </summary>
        /// <returns></returns>
        void AddCustomization(ICustomization customization);
        
        /// <summary>
        /// Add customization factory function to AutoFixture
        /// </summary>
        /// <returns></returns>
        void AddCustomization<TData>(Func<ICustomizationComposer<TData>, ISpecimenBuilder> composerTransformation);

        /// <summary>
        /// Create instance of any class with assigned data within
        /// </summary>
        /// <returns>Auto created instance</returns>
        [Obsolete("Use Make<> instead")]
        TData Fixture<TData>();
        
        /// <summary>
        /// Create instance of any class with assigned data within
        /// </summary>
        /// <returns>Auto created instance</returns>
        TData Make<TData>();

        /// <summary>
        /// Generates a mock for a class/interfaces and injects it into the final fixture
        /// </summary>
        /// <typeparam name="TMockType"></typeparam>
        /// <returns></returns>
        Mock<TMockType> Mock<TMockType>() where TMockType : class;

        /// <summary>
        /// Injects a concrete object to be used when generating the fixture. 
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <returns></returns>
        void Inject<TObjectType>(TObjectType injectedObject);
    }
}
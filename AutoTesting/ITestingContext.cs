using Moq;

namespace AutoTesting;

public interface ITestingContext<out T> where T : class
{
    /// <summary>
    ///     Configure AutoFixture customizations
    /// </summary>
    /// <returns>Test object</returns>
    ContextConfiguration Configuration { get; }

    /// <summary>
    ///     Create instance of testing class from configured fixture
    /// </summary>
    /// <returns>Test object</returns>
    T TestObject { get; }

    /// <summary>
    ///     Create instance of any class with assigned data within
    /// </summary>
    /// <returns>Auto created instance</returns>
    TData Make<TData>();

    /// <summary>
    ///     Generates a mock for a class/interfaces and injects it into the final fixture
    /// </summary>
    /// <typeparam name="TMockType"></typeparam>
    /// <returns></returns>
    Mock<TMockType> Mock<TMockType>() where TMockType : class;

    /// <summary>
    ///     Injects a concrete object to be used when generating the fixture.
    /// </summary>
    /// <typeparam name="TObjectType"></typeparam>
    /// <returns></returns>
    void Inject<TObjectType>(TObjectType injectedObject);
}
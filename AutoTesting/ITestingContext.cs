using Moq;

namespace AutoTesting;

/// <summary>
/// Represents testing context for specific class.
/// </summary>
/// <typeparam name="T">Type of class to wrap with testing context.</typeparam>
/// <remarks>Only classes can be wrapped by testing context</remarks>
public interface ITestingContext<out T> where T : class
{
    /// <summary>
    /// Configure AutoFixture customizations.
    /// </summary>
    /// <returns>Context configuration.</returns>
    ContextConfiguration Configuration { get; }

    /// <summary>
    /// Create instance of testing class from configured context.
    /// </summary>
    /// <returns>Test object.</returns>
    T TestObject { get; }

    /// <summary>
    /// Create instance of any class with assigned data within.
    /// </summary>
    /// <typeparam name="TData">Type of class to create with filled data.</typeparam>
    /// <returns>Auto created instance.</returns>
    TData Make<TData>();

    /// <summary>
    /// Generates a mock for a class/interfaces and injects it into the final context.
    /// </summary>
    /// <typeparam name="TMockType">Type of class/interface to mock.</typeparam>
    /// <returns><see cref="Mock{TMockType}" /> of class/interface.</returns>
    Mock<TMockType> Mock<TMockType>() where TMockType : class;

    /// <summary>
    /// Injects a concrete object to be used when generating the context.
    /// </summary>
    /// <typeparam name="TObjectType">Type of injected object.</typeparam>
    /// <returns></returns>
    /// <remarks>Object cannot be nullable</remarks>
    void Inject<TObjectType>(TObjectType injectedObject) where TObjectType : notnull;
}
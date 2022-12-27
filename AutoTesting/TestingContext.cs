using AutoFixture;
using Moq;

namespace AutoTesting;

public abstract class TestingContext<T> : ITestingContext<T> where T : class
{
    private readonly Fixture _fixture;
    private readonly Dictionary<Type, object> _injectedObjects;
    private readonly Dictionary<Type, Mock> _mocks;

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
    public TData Make<TData>()
    {
        return _fixture.Create<TData>();
    }

    /// <inheritdoc />
    public Mock<TMockType> Mock<TMockType>() where TMockType : class
    {
        var mockType = typeof(TMockType);
        return _mocks.TryGetValue(mockType, out var value) ? (Mock<TMockType>)value : CreateNew();

        Mock<TMockType> CreateNew()
        {
            var newMock = new Mock<TMockType>();
            _mocks.Add(mockType, newMock);
            _fixture.Inject(newMock.Object);

            return newMock;
        }
    }

    /// <inheritdoc />
    public void Inject<TObjectType>(TObjectType injectedObject) where TObjectType : notnull
    {
        ArgumentNullException.ThrowIfNull(injectedObject);
        
        var objectType = typeof(TObjectType);
        if (_injectedObjects.ContainsKey(objectType))
            throw new ArgumentException($"{nameof(injectedObject)} has been injected more than once");

        // It's rare case and not impact on performance of tests code
        _injectedObjects.Add(objectType, injectedObject);
        _fixture.Inject(injectedObject);
    }
}
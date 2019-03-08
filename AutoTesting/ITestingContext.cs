namespace AutoTesting
{
    using Moq;

    public interface ITestingContext<out T> where T : class
    {
        T TestObject { get; }

        TData Fixture<TData>();

        Mock<TMockType> Mock<TMockType>() where TMockType : class;

        void Inject<TObjectType>(TObjectType injectedObject);
    }
}
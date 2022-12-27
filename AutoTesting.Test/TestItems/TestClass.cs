namespace AutoTesting.Test.TestItems;

public class TestClass
{
    private readonly TestEnum _enum;
    private readonly IFoo _foo;

    public TestClass(IFoo foo, TestEnum @enum)
    {
        _foo = foo;
        _enum = @enum;
    }

    public string GetBoo()
    {
        return _foo.Boo();
    }

    public TestEnum GetEnum()
    {
        return _enum;
    }
}
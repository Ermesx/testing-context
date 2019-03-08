# AutoTesting
[![Build status](https://ci.appveyor.com/api/projects/status/rndm33j6725jgdv5?svg=true)](https://ci.appveyor.com/project/Ermesx/testing-context)
![](https://img.shields.io/nuget/v/AutoTesting.svg)

Inspired by article https://dotnetcoretutorials.com/2018/05/12/the-testing-context/

Library with class which covers common testing issues below.

## Testing problems

* Any testing helper should limit the testing scope to a single class only.
* Changes to a constructor of a class should not require editing all tests for that class.
* Boilerplate code within the test class should be kept to a minimum.

### What's new in 3.0

Better API naming 
```diff
public class TestingContext<T> 
{
+    T TestObject { get; }
-    T ClassUnderTest { get; }
    
+    TData Fixture<TData>();
-    TData MockData<TData>();
    
+    Mock<TMockType> Mock<TMockType>() where TMockType : class;
-    void ConfigureMock<TMockType>(Action<TMockType> configure) where TMockType : class;

+    void Inject<TObjectType>(TObjectType injectedObject);
-    void InjectObject<TObjectType>(TObjectType injectedObject);
}
```
New interface for mocking even TestingContext

```csharp
public interface ITestingContext<out T> where T : class
{
    ...
}
```

#### Install package
```
dotnet add package AutoTesting 
```

## In action

To show you how the testing context works, we’ll create a quick couple of test classes.

``` c#
public interface ITestRepository
{
    List<string> GetNames();
}
 
public class TestRepository : ITestRepository
{
    public List<string> GetNames()
    {
        return new List<string>
        {
            "John",
            "Bob",
            "Wade"
        };
    }
}
 
public interface ITestService
{
    List<string> GetNamesExceptJohn();
}
 
public class TestService : ITestService
{
    private readonly ITestRepository _testRepository;
    private readonly UtilityService _utilityService;
 
    public TestService(ITestRepository testRepository, UtilityService utilityService)
    {
        _testRepository = testRepository;
        _utilityService = utilityService;
    }
 
    public List<string> GetNamesExceptJohn()
    {
        return _testRepository.GetNames().Where(x => !x.Equals("john", StringComparison.CurrentCultureIgnoreCase)).ToList();
    }
 
    public List<string> GetPalindromeNames()
    {
        return _testRepository.GetNames().Where(x => _utilityService.IsPalindrome(x)).ToList();
    }
 
}
 
public class UtilityService
{
    public bool IsPalindrome(string word)
    {
        return word == string.Join(string.Empty, word.ToCharArray().Reverse());
    }
}
```

Now onto the tests in (xUnit). Our first test looks like so :

``` c#
public class TestService_Tests : TestingContext<TestService>
{
    [Fact]
    public void WhenGetNamesExceptJohnCalled_JohnIsNotReturned()
    {
        Mock<ITestRepository>().Setup(x => x.GetNames()).Returns(new List<string> { "bob", "john" });
 
        var result = TestObject.GetNamesExceptJohn();
 
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("bob", result.First());
    }
}
```

More information and usecases in article:
https://dotnetcoretutorials.com/2018/05/12/the-testing-context/

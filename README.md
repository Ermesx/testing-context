# Testing Context
Inspired by article https://dotnetcoretutorials.com/2018/05/12/the-testing-context/

Library with class which covers common testing issues below.

## Testing problems

* Any testing helper should limit the testing scope to a single class only.
* Changes to a constructor of a class should not require editing all tests for that class.
* Boilerplate code within the test class should be kept to a minimum.

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
        GetMockFor<ITestRepository>().Setup(x => x.GetNames()).Returns(new List<string> { "bob", "john" });
 
        var result = ClassUnderTest.GetNamesExceptJohn();
 
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("bob", result.First());
    }
}
```

More information and usecases in article:
https://dotnetcoretutorials.com/2018/05/12/the-testing-context/

using Xunit;
using System;
using P3Project.API;
namespace zenref.Tests;

public class TestAPIen : Api
{

}

public class ApiTest
{
    [Fact]
    public void Test1()
    {

    }
    [Fact]
    public void ReferenceFetchIsNotImplemented()
    {
        TestAPIen api = new TestAPIen();
    }
}
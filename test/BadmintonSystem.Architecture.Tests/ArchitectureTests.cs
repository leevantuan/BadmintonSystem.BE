using BadmintonSystem.Contract.Abstractions.Messages;
using FluentAssertions;
using NetArchTest.Rules;

namespace BadmintonSystem.Architecture.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "BadmintonSystem.Domain";
    private const string ApplicationNamespace = "BadmintonSystem.Application";
    private const string InfrastructureNamespace = "BadmintonSystem.Infrastructure";
    private const string PersistenceNamespace = "BadmintonSystem.Persistence";
    private const string PresentationNamespace = "BadmintonSystem.Presentation";
    private const string ApiNamespace = "BadmintonSystem.API";

    #region ================================ Architecture ===============================

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrage
        // Take all Assembly in project
        var assembly = Domain.AssemblyReference.Assembly;

        // Những project can't reference
        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            PersistenceNamespace,
            ApiNamespace
        };

        // Act
        var testResult = Types.InAssembly(assembly) // Take all Assembly
                              .ShouldNot() // Should not Reference != Should Reference
                              .HaveDependencyOnAny(otherProjects) // Have Dependency On Any == Just Reference "otherProject" => Cancel, All => Phải Reference đến tất cả thì mới Cancel
                              .GetResult();

        // Assert => Nó phải đúng
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrage
        // Lấy hết các Assembly trong project
        var assembly = Application.AssemblyReference.Assembly;

        // Những project không reference tới
        var otherProjects = new[]
        {
            InfrastructureNamespace,
            PresentationNamespace,
            //PersistenceNamespace, // Using RawSQL
            ApiNamespace
        };

        // Act
        var testResult = Types.InAssembly(assembly) // Lấy tất cả các Assembly
                              .ShouldNot() // Should not Không Reference != Should nên Reference
                              .HaveDependencyOnAny(otherProjects) // Không dêpendency Any => chỉ cần Reference 1 là sẽ cancel, All => Phải Reference đến tất cả thì mới Cancel
                              .GetResult();

        // Assert => Nó phải đúng
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrage
        // Lấy hết các Assembly trong project
        var assembly = Domain.AssemblyReference.Assembly;

        // Những project không reference tới
        var otherProjects = new[]
        {
            PresentationNamespace,
            ApiNamespace
        };

        // Act
        var testResult = Types.InAssembly(assembly) // Lấy tất cả các Assembly
                              .ShouldNot() // Should not Không Reference != Should nên Reference
                              .HaveDependencyOnAny(otherProjects) // Không dêpendency Any => chỉ cần Reference 1 là sẽ cancel, All => Phải Reference đến tất cả thì mới Cancel
                              .GetResult();

        // Assert => Nó phải đúng
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Persistence_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrage
        // Lấy hết các Assembly trong project
        var assembly = Domain.AssemblyReference.Assembly;

        // Những project không reference tới
        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            ApiNamespace
        };

        // Act
        var testResult = Types.InAssembly(assembly) // Lấy tất cả các Assembly
                              .ShouldNot() // Should not Không Reference != Should nên Reference
                              .HaveDependencyOnAny(otherProjects) // Không dêpendency Any => chỉ cần Reference 1 là sẽ cancel, All => Phải Reference đến tất cả thì mới Cancel
                              .GetResult();

        // Assert => Nó phải đúng
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Presentation_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrage
        // Lấy hết các Assembly trong project
        var assembly = Domain.AssemblyReference.Assembly;

        // Những project không reference tới
        var otherProjects = new[]
        {
            InfrastructureNamespace,
            ApiNamespace
        };

        // Act
        var testResult = Types.InAssembly(assembly) // Lấy tất cả các Assembly
                              .ShouldNot() // Should not Không Reference != Should nên Reference
                              .HaveDependencyOnAny(otherProjects) // Không dêpendency Any => chỉ cần Reference 1 là sẽ cancel, All => Phải Reference đến tất cả thì mới Cancel
                              .GetResult();

        // Assert => Nó phải đúng
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion ================================ Architecture ===============================

    #region ================================= Test Naming Command ===================================

    [Fact]
    public void Command_Should_Have_NamingConventionEndingCommandHandler()
    {
        // Arrange
        // Takes all reference assembly of Application
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types.InAssembly(assembly) // Check all in assembly
                            .That() // Confirm type check
                            .ImplementInterface(typeof(ICommandHandler<>)) // This is filter, If it using Generic Interface "ICommandHandler"
                            .Should()
                            .HaveNameEndingWith("CommandHandler") // Name End must have "CommandHandler"
                            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandT_Should_Have_NamingConventionEndingCommandHandler()
    {
        var assembly = Application.AssemblyReference.Assembly;

        var testResult = Types
                        .InAssembly(assembly)
                        .That() // Lấy các cái có trong nó ra 
                        .ImplementInterface(typeof(ICommandHandler<,>)) // Lấy những cái Implement được kế thừa từ ICommandHandler
                        .Should().NotHaveNameEndingWith("CommandHandler") // Check string cuối phải là CommandHandler
                        .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    // Có Sealed để tăng Performance
    [Fact]
    public void CommandHandler_Should_Have_BeSealed()
    {
        var assembly = Application.AssemblyReference.Assembly;

        var testResult = Types
                        .InAssembly(assembly)
                        .That() // Lấy các cái có trong nó ra
                        .ImplementInterface(typeof(ICommandHandler<>)) // Lấy những cái Implement được kế thừa từ ICommandHandler
                        .Should().BeSealed() // Check có Sealed không
                        .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlerT_Should_Have_BeSealed()
    {
        var assembly = Application.AssemblyReference.Assembly;

        var testResult = Types
                        .InAssembly(assembly)
                        .That() // Lấy các cái có trong nó ra 
                        .ImplementInterface(typeof(ICommandHandler<,>)) // Lấy những cái Implement được kế thừa từ ICommandHandler
                        .Should().BeSealed() // Check có Sealed không
                        .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion ================================= Test Naming Command ===================================

    #region ================================= Test Naming Query ===================================

    [Fact]
    public void QueryT_Should_Have_NamingConventionEndingQueryHandler()
    {
        var assembly = Application.AssemblyReference.Assembly;

        var testResult = Types.InAssembly(assembly) // Check all in assembly
                            .That() // Confirm type check
                            .ImplementInterface(typeof(IQueryHandler<,>)) // This is filter, If it using Generic Interface "ICommandHandler"
                            .Should()
                            .HaveNameEndingWith("QueryHandler") // Name End must have "CommandHandler"
                            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    // Có Sealed để tăng Performance
    [Fact]
    public void QueryHandlerT_Should_Have_BeSealed()
    {
        var assembly = Application.AssemblyReference.Assembly;

        var testResult = Types.InAssembly(assembly)
                        .That() // Lấy các cái có trong nó ra
                        .ImplementInterface(typeof(IQueryHandler<,>)) // Lấy những cái Implement được kế thừa từ IQueryHandler
                        .Should().BeSealed() // Check có Sealed không
                        .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion ================================= Test Naming Command ===================================

}

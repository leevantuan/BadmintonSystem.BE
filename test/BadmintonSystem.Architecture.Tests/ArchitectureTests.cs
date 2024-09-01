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
            PersistenceNamespace,
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

}

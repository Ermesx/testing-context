using System.Collections.Immutable;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoTesting;

/// <summary>
/// Represents configuration of default and additional features of <see cref="Fixture" />.
/// </summary>
/// <remarks>Default customization is <see cref="AutoMoqCustomization" /></remarks>
public class ContextConfiguration
{
    private static Action<IFixture> _defaults = _ => _.Customize(new AutoMoqCustomization());

    private readonly IFixture _fixture;

    /// <summary>
    /// Create configuration.
    /// </summary>
    /// <param name="fixture"><see cref="Fixture" /> to configure</param>
    public ContextConfiguration(IFixture fixture)
    {
        _fixture = fixture;
        _defaults(_fixture);
    }

    /// <summary>
    /// Get <see cref="Fixture" /> customizations.
    /// </summary>
    public ImmutableList<ISpecimenBuilder> Customizations => _fixture.Customizations.ToImmutableList();

    /// <summary>
    /// Clear all default customizations.
    /// </summary>
    public static void ClearDefaults() => _defaults = _ => { };

    /// <summary>
    /// Customizes the creation algorithm for all objects of a given type.
    /// </summary>
    /// <param name="composerTransformation">
    /// A function that customizes a given <see cref="ICustomizationComposer{T}" /> and
    /// returns the modified composer.
    /// </param>
    /// <typeparam name="TData">The type of object to customize.</typeparam>
    /// <remarks>The resulting <see cref="ISpecimenBuilder" /> is added to Customizations.</remarks>
    public void Customize<TData>(Func<ICustomizationComposer<TData>, ISpecimenBuilder> composerTransformation) =>
        _fixture.Customize(composerTransformation);

    /// <summary>
    /// Applies a customization.
    /// </summary>
    /// <param name="customization">The customization to apply.</param>
    public void Customize(ICustomization customization) => _fixture.Customize(customization);
}
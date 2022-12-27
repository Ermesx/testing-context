using System;
using System.Collections.Immutable;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoTesting;

public class ContextConfiguration
{
    private static Action<IFixture> _defaults = f => f.Customize(new AutoMoqCustomization());

    private readonly IFixture _fixture;

    public ContextConfiguration(IFixture fixture)
    {
        _fixture = fixture;
        _defaults(_fixture);
    }

    public ImmutableList<ISpecimenBuilder> Customizations => _fixture.Customizations.ToImmutableList();

    public static void ClearDefaults()
    {
        _defaults = _ => { };
    }

    public void Customize<TData>(Func<ICustomizationComposer<TData>, ISpecimenBuilder> composerTransformation)
    {
        _fixture.Customize(composerTransformation);
    }

    public void Customize(ICustomization customization)
    {
        _fixture.Customize(customization);
    }
}
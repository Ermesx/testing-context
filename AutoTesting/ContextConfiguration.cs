using System.Collections.Immutable;

namespace AutoTesting
{
    using System;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Dsl;
    using AutoFixture.Kernel;
    
    public class ContextConfiguration
    {
        public static void ClearDefaults() => _defaults = _ => { };

        private static Action<IFixture> _defaults = f => f.Customize(new AutoMoqCustomization());
        
        private readonly IFixture _fixture;

        public ImmutableList<ISpecimenBuilder> Customizations => _fixture.Customizations.ToImmutableList();

        public ContextConfiguration(IFixture fixture)
        {
            _fixture = fixture;
            _defaults(_fixture);
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
}
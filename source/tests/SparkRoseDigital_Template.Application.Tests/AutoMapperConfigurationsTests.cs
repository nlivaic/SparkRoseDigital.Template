using System.Linq;
using AutoMapper;
using Xunit;

namespace SparkRoseDigital_Template.Application.Tests;

public class AutoMapperConfigurationsTests
{
    [Fact]
    public void AutoMapperConfigurations_AreAllValid()
    {
        // Arrange - get all profiles in the SparkRoseDigital_Template.Application assembly.
        var profiles = ApplicationAssemblyInfo.Value.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t));

        // Act
        var target = new MapperConfiguration(cfg =>
        {
            foreach (var profile in profiles)
            {
                cfg.AddProfile(profile);
            }
        }).CreateMapper();

        // Assert
        target.ConfigurationProvider.AssertConfigurationIsValid();
    }
}

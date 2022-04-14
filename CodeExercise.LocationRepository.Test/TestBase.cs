using System;
using CodeExercise.LocationService;
using CodeExercise.Model;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodeExercise.LocationRepository.Test;

public abstract class TestBase
{
    protected ILocationSearchService CreateLocationService(ILocationRepository repo)
    {
        if (repo == null) throw new ArgumentNullException(nameof(repo));

        return new LocationSearchService(new NullLogger<LocationSearchService>(), repo);
    }
}
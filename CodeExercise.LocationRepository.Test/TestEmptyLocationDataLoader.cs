using System;
using System.Collections.Generic;
using CodeExercise.Model;

namespace CodeExercise.LocationRepository.Test;

internal class TestEmptyLocationDataLoader : ILocationDataProvider
{
    public IEnumerable<ILocation> GetData()
    {
        return Array.Empty<ILocation>();
    }
}
// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging.Abstractions;
using LocationRepository = CodeExercise.LocationRepository.LocationRepository;

Console.WriteLine("Hello, World!");

var repo = new LocationRepository(new NullLogger<LocationRepository>());


Console.ReadLine();
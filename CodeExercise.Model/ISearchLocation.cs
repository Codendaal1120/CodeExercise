namespace CodeExercise.Model;

public interface ISearchLocation : ILocation
{
    /// <summary>
    /// Distance to the reference location
    /// </summary>
    double Distance { get; }
}
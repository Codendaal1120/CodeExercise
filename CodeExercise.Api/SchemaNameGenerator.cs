using NJsonSchema.Generation;

namespace CodeExercise.Api
{
    /// <summary>
    /// Transforms internal schema names into public ones for swagger documentation
    /// </summary>
    internal class SchemaNameGenerator : ISchemaNameGenerator
    {
        /// <inheritdoc cref="ISchemaNameGenerator"/>
        public string Generate(Type type)
        {
            return type.Name.EndsWith("Api")
                ? type.Name.Substring(0, type.Name.Length - 3)
                : type.Name;
        }
    }
}

namespace Dexter.IndexStrategies.Converters
{
    public interface IPropertyConverter<in TSource, out TDestination>
    {
        /// <summary>
        /// Converts a source to a specific property type
        /// </summary>
        /// <param name="source">The source</param>
        /// <returns>The converted model</returns>
        TDestination Convert(TSource source);
    }
}

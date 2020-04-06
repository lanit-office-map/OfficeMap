namespace OfficeService.Mappers.Interfaces
{
  /// <summary>
  ///  Represents an interface for mappers.
  /// </summary>
  /// <typeparam name="TTargetModel">
  ///  Model type from target namespace.
  /// </typeparam>
  /// <typeparam name="TSourceModel">
  ///  Model type from source namespace.
  /// </typeparam>
  public interface IMapper<
    out TTargetModel,
    in TSourceModel>
    where TTargetModel : class
    where TSourceModel : class
  {
    /// <summary>
    ///  Maps source model to target model.
    /// </summary>
    /// <param name="model">Source model.</param>
    /// <returns>Target model.</returns>
    TTargetModel Map(TSourceModel model);
  }
}

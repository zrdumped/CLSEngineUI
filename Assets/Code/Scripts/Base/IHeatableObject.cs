namespace Chemix
{
    /// <summary>
    ///	IHeatableObject can respond to heating events
    /// </summary>
    public interface IHeatableObject
    {
        void SetIsHeating(bool isHeating);
    }
}
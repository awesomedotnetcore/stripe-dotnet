namespace Stripe
{
    /// <summary>
    /// Base class for Stripe options classes for which parameters should be encoded as
    /// <c>multipart/form-data</c> rather than the usual <c>application/x-www-form-urlencoded</c>.
    /// </summary>
    public class MultipartOptions : BaseOptions
    {
    }
}

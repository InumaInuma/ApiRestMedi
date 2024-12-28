namespace ApiRestMedi.ManejoErrores
{
    public class ProductoException :Exception
    {
        public ProductoException(string message) : base(message) { }

        public ProductoException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

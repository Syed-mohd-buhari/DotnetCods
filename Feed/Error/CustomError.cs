namespace TEMS.Error
{
    public class CustomError : Exception
    {
        public CustomError(String message, string module_name)
            : base(message)
        {

        }
    }
}

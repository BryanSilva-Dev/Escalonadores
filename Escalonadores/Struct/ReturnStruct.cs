namespace Escalonadores.Struct
{
    public class ReturnStruct
    {
        public ReturnStruct() 
        {

        }

        public bool isError { get; set; }
        public int errorCode { get; set; }
        public string? errorDescription { get; set; }
    }
}
